using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Adds this class to your ladders so a Character can climb them.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Environment/Ladder")]
	public class Ladder : MonoBehaviour 
	{
		/// the different types of ladders
		public enum LadderTypes { Simple, BiDirectional }
		/// determines whether this ladder is simple (vertical) or bidirectional
		public LadderTypes LadderType = LadderTypes.Simple;
		/// should the character be centered horizontally on the ladder when climbing
		public bool CenterCharacterOnLadder = true;
		/// the platform at the top of the ladder - this can be a ground platform 
		public GameObject LadderPlatform;

		public BoxCollider2D LadderPlatformBoxCollider2D { get; protected set; }
		public EdgeCollider2D LadderPlatformEdgeCollider2D { get; protected set; }


		protected virtual void Start()
		{
			if (LadderPlatform == null)
			{
				return;
			}

			LadderPlatformBoxCollider2D = LadderPlatform.GetComponent<BoxCollider2D>();
			LadderPlatformEdgeCollider2D = LadderPlatform.GetComponent<EdgeCollider2D>();

			if (LadderPlatformBoxCollider2D == null && LadderPlatformEdgeCollider2D == null)
			{
				Debug.LogWarning(this.name+" : this ladder's LadderPlatform is missing a BoxCollider2D or an EdgeCollider2D.");
			}
		}

	    /// <summary>
	    /// Triggered when something collides with the ladder
	    /// </summary>
	    /// <param name="collider">Something colliding with the ladder.</param>
	    protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			// we check that the object colliding with the ladder is actually a corgi controller and a character
			CharacterLadder characterLadder = collider.GetComponent<CharacterLadder>();
			if (characterLadder==null)
			{
				return;					
			}			
			characterLadder.LadderColliding=true;
			characterLadder.CurrentLadder = this;
		}

	    /// <summary>
	    /// Triggered when something stays on the ladder
	    /// </summary>
	    /// <param name="collider">Something colliding with the ladder.</param>
	    protected virtual void OnTriggerStay2D(Collider2D collider)
		{		
			// we check that the object colliding with the ladder is actually a corgi controller and a character
			CharacterLadder characterLadder = collider.GetComponent<CharacterLadder>();
			if (characterLadder==null)
			{
				return;					
			}	

			characterLadder.LadderColliding=true;
			characterLadder.CurrentLadder = this;			
		}

	    /// <summary>
	    /// Triggered when something exits the ladder
	    /// </summary>
	    /// <param name="collider">Something colliding with the ladder.</param>
	    protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			// we check that the object colliding with the ladder is actually a corgi controller and a character
			CharacterLadder characterLadder = collider.GetComponent<CharacterLadder>();
			if (characterLadder==null)
			{
				return;					
			}
											
			// when the character is not colliding with the ladder anymore, we reset its various ladder related states			
			characterLadder.LadderColliding = false;	
			characterLadder.CurrentLadder = null;				
		}
	}
}