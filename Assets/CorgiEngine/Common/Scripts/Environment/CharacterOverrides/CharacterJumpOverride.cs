using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[AddComponentMenu("Corgi Engine/Environment/Character Jump Override")]
	/// <summary>
	/// Add this component to a trigger zone, and it'll override the CharacterJump settings for all characters that cross it 
	/// </summary>
	public class CharacterJumpOverride : MonoBehaviour 
	{
		[Header("Jumps")]
		/// defines how high the character can jump
		public float JumpHeight = 3.025f;
		/// the minimum time in the air allowed when jumping - this is used for pressure controlled jumps
		public float JumpMinimumAirTime = 0.1f;
		/// the maximum number of jumps allowed (0 : no jump, 1 : normal jump, 2 : double jump, etc...)
		public int NumberOfJumps=3;
		/// basic rules for jumps : where can the player jump ?
		public CharacterJump.JumpBehavior JumpRestrictions;
		/// if this is set to true, on enter/exit, the number of jumps will be set to its maximum, otherwise it'll retain the value it had on entry
		public bool ResetNumberOfJumpsLeft = true;

		protected float _previousJumpHeight;
		protected float _previousJumpMinimumAirTime;
		protected int _previousNumberOfJumps;
		protected int _previousNumberOfJumpsLeft;
		protected CharacterJump.JumpBehavior _previousJumpRestrictions;

		/// <summary>
	    /// Triggered when something collides with the override zone
	    /// </summary>
		/// <param name="collider">Something colliding with the override zone.</param>
	    protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			// we check that the object colliding with the override zone is actually a characterJump
			CharacterJump characterJump = collider.GetComponent<CharacterJump>();
			if (characterJump==null)
			{
				return;	
			}	

			_previousJumpHeight = characterJump.JumpHeight ;
			_previousJumpMinimumAirTime = characterJump.JumpMinimumAirTime ;
			_previousNumberOfJumps = characterJump.NumberOfJumps ;
			_previousJumpRestrictions = characterJump.JumpRestrictions ;	
			_previousNumberOfJumpsLeft = characterJump.NumberOfJumpsLeft;

			characterJump.JumpHeight = JumpHeight;
			characterJump.JumpMinimumAirTime = JumpMinimumAirTime;
			characterJump.NumberOfJumps = NumberOfJumps;
			characterJump.JumpRestrictions = JumpRestrictions;	
			if (ResetNumberOfJumpsLeft)
			{
				characterJump.SetNumberOfJumpsLeft(NumberOfJumps);	
			}
		}

	    /// <summary>
	    /// Triggered when something exits the water
	    /// </summary>
	    /// <param name="collider">Something colliding with the water.</param>
	    protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			// we check that the object colliding with the water is actually a characterJump
			CharacterJump characterJump = collider.GetComponent<CharacterJump>();
			if (characterJump==null)
			{
				return;	
			}

			characterJump.JumpHeight = _previousJumpHeight;
			characterJump.JumpMinimumAirTime = _previousJumpMinimumAirTime;
			characterJump.NumberOfJumps = _previousNumberOfJumps;
			characterJump.JumpRestrictions = _previousJumpRestrictions;	
			if (ResetNumberOfJumpsLeft)
			{
				characterJump.SetNumberOfJumpsLeft(characterJump.NumberOfJumps);	
			}
			else
			{
				characterJump.SetNumberOfJumpsLeft (_previousNumberOfJumpsLeft);
			}
		}
	}
}
