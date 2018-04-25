using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Extend this class to activate something when a button is pressed in a certain zone
	/// </summary>
	public class ButtonActivated : MonoBehaviour 
	{
		[Header("Activation")]
		/// if this is true, this zone can only be activated if the character has the required ability
		public bool RequiresButtonActivationAbility = true;
		/// if this is true, this can only be activated by player Characters
		public bool RequiresPlayerCharacter = true;
		/// if true, the zone will activate whether the button is pressed or not
		public bool AutoActivation = false;
		/// Set this to true if you want the CharacterBehaviorState to be notified of the player's entry into the zone.
		public bool ShouldUpdateState = true;
		/// if this is set to false, the zone won't be activable while not grounded
		public bool CanOnlyActivateIfGrounded = false;

		[Header("Visual Prompt")]
		/// If true, the "buttonA" prompt will always be shown, whether the player is in the zone or not.
		public bool AlwaysShowPrompt = true;
		/// If true, the "buttonA" prompt will be shown when a player is colliding with the zone
		public bool ShowPromptWhenColliding = true;
		/// If true, the prompt will hide after use
		public bool HidePromptAfterUse = false;
		/// the position of the actual buttonA prompt relative to the object's center
		public Vector3 PromptRelativePosition = Vector3.zero;

		protected GameObject _buttonA;
	    protected Collider2D _collider;
		protected bool _promptHiddenForever = false;
		protected CharacterButtonActivation _characterButtonActivation;

	    /// <summary>
	    /// Determines whether this instance can show button prompt.
	    /// </summary>
	    /// <returns><c>true</c> if this instance can show prompt; otherwise, <c>false</c>.</returns>
	    public virtual bool CanShowPrompt()
	    {
	    	if (_buttonA==null)
	    	{
	    		return true;
	    	}
	    	return false;
	    }


		protected virtual void OnEnable()
		{
			_collider = (Collider2D)GetComponent<Collider2D>();
			if (AlwaysShowPrompt)
			{
				ShowPrompt();
			}
		}

		/// <summary>
		/// Override this to trigger an action when the main button is pressed within the zone
		/// </summary>
		public virtual void TriggerButtonAction()
		{
			if (HidePromptAfterUse)
			{
				_promptHiddenForever = true;
				StartCoroutine(HidePrompt());	
			}
		}

		/// <summary>
	    /// Shows the button A prompt.
	    /// </summary>
	    public virtual void ShowPrompt()
		{
			if (_promptHiddenForever)
			{
				return;
			}
			// we add a blinking A prompt to the top of the zone
			_buttonA = (GameObject)Instantiate(Resources.Load("GUI/ButtonA"));			
			_buttonA.transform.position=_collider.bounds.center + PromptRelativePosition; 
			_buttonA.transform.parent = transform;
			_buttonA.GetComponent<SpriteRenderer>().material.color=new Color(1f,1f,1f,0f);
			StartCoroutine(MMFade.FadeSprite(_buttonA.GetComponent<SpriteRenderer>(),0.2f,new Color(1f,1f,1f,1f)));	
		}

	    /// <summary>
	    /// Hides the button A prompt.
	    /// </summary>
		public virtual IEnumerator HidePrompt()
		{	
			StartCoroutine(MMFade.FadeSprite(_buttonA.GetComponent<SpriteRenderer>(),0.2f,new Color(1f,1f,1f,0f)));	
			yield return new WaitForSeconds(0.3f);
			Destroy(_buttonA);
		}

		/// <summary>
		/// Triggered when something collides with the button activated zone
		/// </summary>
		/// <param name="collider">Something colliding with the water.</param>
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{

            Character character = collider.gameObject.GetComponentNoAlloc<Character>();
			CharacterButtonActivation characterButtonActivation = collider.gameObject.GetComponentNoAlloc<CharacterButtonActivation>();

			if (!CheckConditions(character, characterButtonActivation))
			{
				return;
			}

			// if we can only activate this zone when grounded, we check if we have a controller and if it's not grounded,
			// we do nothing and exit
			if (CanOnlyActivateIfGrounded)
			{
				if (character != null)
				{
					CorgiController controller = collider.gameObject.GetComponentNoAlloc<CorgiController>();
					if (controller != null)
					{
						if (!controller.State.IsGrounded)
						{
							return;
						}
					}
				}
			}

			if (ShouldUpdateState)
			{
				_characterButtonActivation = characterButtonActivation;
				if (characterButtonActivation != null)
				{
					characterButtonActivation.InButtonActivatedZone = true;
					characterButtonActivation.ButtonActivatedZone = this;
				}
			}

			if (AutoActivation)
			{
				TriggerButtonAction();
			}	

			// if we're not already showing the prompt and if the zone can be activated, we show it
			if (CanShowPrompt() && ShowPromptWhenColliding)
			{
				ShowPrompt();	
			}
		}

		/// <summary>
		/// Triggered when something exits the water
		/// </summary>
		/// <param name="collider">Something colliding with the dialogue zone.</param>
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			// we check that the object colliding with the water is actually a corgi controller and a character
			Character character = collider.GetComponent<Character>();
			CharacterButtonActivation characterButtonActivation = collider.GetComponent<CharacterButtonActivation>();

			if (!CheckConditions(character, characterButtonActivation))
			{
				return;
			}

			if (ShouldUpdateState)
			{
				_characterButtonActivation=characterButtonActivation;
				if (characterButtonActivation != null)
				{
					characterButtonActivation.InButtonActivatedZone=false;
					characterButtonActivation.ButtonActivatedZone=null;		
				}
			}

			if ((_buttonA!=null) && !AlwaysShowPrompt)
			{
				StartCoroutine(HidePrompt());	
			}	
		}

		/// <summary>
		/// Determines whether or not this zone should be activated
		/// </summary>
		/// <returns><c>true</c>, if conditions was checked, <c>false</c> otherwise.</returns>
		/// <param name="character">Character.</param>
		/// <param name="characterButtonActivation">Character button activation.</param>
		protected virtual bool CheckConditions(Character character, CharacterButtonActivation characterButtonActivation)
		{
			if (character == null)
			{
				return false;
			}

			if (RequiresPlayerCharacter)
			{
				if (character.CharacterType != Character.CharacterTypes.Player)
				{
					return false;
				}
			}

			if (RequiresButtonActivationAbility)
			{
				// we check that the object colliding with the water is actually a corgi controller and a character
				if (characterButtonActivation==null)
					return false;	
			}

			return true;
		}
	}
}
