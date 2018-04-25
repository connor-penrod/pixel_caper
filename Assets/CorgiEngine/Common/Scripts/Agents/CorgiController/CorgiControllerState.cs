using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// The various states you can use to check if your character is doing something at the current frame
	/// </summary>

	public class CorgiControllerState 
	{
		/// is the character colliding right ?
		public bool IsCollidingRight { get; set; }
		/// is the character colliding left ?
		public bool IsCollidingLeft { get; set; }
		/// is the character colliding with something above it ?
		public bool IsCollidingAbove { get; set; }
		/// is the character colliding with something above it ?
		public bool IsCollidingBelow { get; set; }
		/// is the character colliding with anything ?
		public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; }}

		/// returns the slope angle met horizontally
		public float LateralSlopeAngle { get; set; }
		/// returns the slope the character is moving on angle
		public float BelowSlopeAngle { get; set; }
		/// returns true if the slope angle is ok to walk on
		public bool SlopeAngleOK { get; set; }
		/// returns true if the character is standing on a moving platform
		public bool OnAMovingPlatform { get; set; }
		
		/// Is the character grounded ? 
		public bool IsGrounded { get { return IsCollidingBelow; } }
		/// is the character falling right now ?
		public bool IsFalling { get; set; }
		/// was the character grounded last frame ?
		public bool WasGroundedLastFrame { get ; set; }
		/// was the character grounded last frame ?
		public bool WasTouchingTheCeilingLastFrame { get ; set; }
		/// did the character just become grounded ?
		public bool JustGotGrounded { get ; set;  }
				
		/// <summary>
		/// Reset all collision states to false
		/// </summary>
		public virtual void Reset()
		{
			IsCollidingLeft = false;
			IsCollidingRight = false;
			IsCollidingAbove = false;
			SlopeAngleOK = false;
			JustGotGrounded = false;
			IsFalling=true;
			LateralSlopeAngle = 0;
		}
		
		/// <summary>
		/// Serializes the collision states
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current collision states.</returns>
		public override string ToString ()
		{
			return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}",
			IsCollidingRight,
			IsCollidingLeft,
			IsCollidingAbove,
			IsCollidingBelow,
			LateralSlopeAngle);
		}	
	}
}