using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this class to a platform to make it a jumping platform, a trampoline or whatever.
	/// It will automatically push any character that touches it up in the air.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Environment/Jumper")]
	public class Jumper : MonoBehaviour 
	{
		/// the force of the jump induced by the platform
		public float JumpPlatformBoost = 40;
		/// <summary>
		/// Triggered when a CorgiController touches the platform, applys a vertical force to it, propulsing it in the air.
		/// </summary>
		/// <param name="controller">The corgi controller that collides with the platform.</param>
			
		public virtual void OnTriggerStay2D(Collider2D collider)
		{
			CorgiController controller=collider.GetComponent<CorgiController>();
			if (controller==null)
				return;
			
			controller.SetVerticalForce(Mathf.Sqrt( 2f * JumpPlatformBoost * -controller.Parameters.Gravity ));
		}
	}
}