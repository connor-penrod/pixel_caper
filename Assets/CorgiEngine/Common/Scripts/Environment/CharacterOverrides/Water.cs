using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this class to a body of water. It will handle splash effects on entering/exiting, and allow the player to jump out of it.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Environment/Water")]
	public class Water : MonoBehaviour 
	{
		/// the force to add to a character when it exits the water
		public float WaterExitForce=8f;
		/// the effect that will be instantiated everytime the character enters or exits the water
		public GameObject WaterEntryEffect;

	    //storage
	    protected int _numberOfJumpsSaved;

	    /// <summary>
	    /// Triggered when something collides with the water
	    /// </summary>
	    /// <param name="collider">Something colliding with the water.</param>
	    protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			// we check that the object colliding with the water is actually a corgi controller and a character
			CharacterJump characterJump = collider.GetComponent<CharacterJump>();
			if (characterJump==null)
			{
				return;	
			}
			CorgiController controller = collider.GetComponent<CorgiController>();
			if (controller==null)
			{
				return;		
			}
				
			Splash (characterJump.transform.position);
		}

	    /// <summary>
	    /// Triggered when something exits the water
	    /// </summary>
	    /// <param name="collider">Something colliding with the water.</param>
	    protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			// we check that the object colliding with the water is actually a corgi controller and a character
			CharacterJump characterJump = collider.GetComponent<CharacterJump>();
			if (characterJump==null)
			{
				return;	
			}
			CorgiController controller = collider.GetComponent<CorgiController>();
			if (controller==null)
			{
				return;		
			}


			// we also push it up in the air		
			Splash (characterJump.transform.position);
			controller.SetVerticalForce(Mathf.Abs( WaterExitForce ));
		}

	    /// <summary>
	    /// Creates a splash of water at the point of entry
	    /// </summary>
	    protected virtual void Splash(Vector3 splashPosition)
		{
			
			Instantiate(WaterEntryEffect,splashPosition,Quaternion.identity);		
		}
	}
}