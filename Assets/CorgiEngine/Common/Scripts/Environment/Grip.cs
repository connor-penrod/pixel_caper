using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
	[RequireComponent(typeof(Collider2D))]
	/// <summary>
	/// Add this component to an object with a 2D collider and it'll be grippable by any Character equipped with a CharacterGrip
	/// </summary>
	[AddComponentMenu("Corgi Engine/Environment/Grip")]
	public class Grip : MonoBehaviour 
	{
		/// <summary>
		/// When an object collides with the grip, we check to see if it's a compatible character, and if yes, we change its state to Gripping
		/// </summary>
		/// <param name="collider">Collider.</param>
	    protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			CharacterGrip characterGrip = collider.GetComponent<CharacterGrip>();
			if (characterGrip==null)
			{
				return;					
			}			
			characterGrip.GetComponent<Character>().MovementState.ChangeState(CharacterStates.MovementStates.Gripping);

		}

		/// <summary>
		/// While the character is colliding with the grip, and if it's still gripping, we update its position
		/// </summary>
		/// <param name="collider">Collider.</param>
		protected virtual void OnTriggerStay2D(Collider2D collider)
		{
			// we check that the object colliding with the grip is actually a corgi controller and a character
			Character character = collider.GetComponent<Character>();
			if (character==null)
				return;		
			CorgiController controller = collider.GetComponent<CorgiController>();
			if (controller==null)
				return;	

			if (character.MovementState.CurrentState == CharacterStates.MovementStates.Gripping)
			{
				controller.transform.position=transform.position;
			}
		}	    
	}
}