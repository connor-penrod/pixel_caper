using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[AddComponentMenu("Corgi Engine/Environment/Surface Modifier")]
	/// <summary>
	/// Add this component to a platform and define its new friction or force which will be applied to any CorgiController that walks on it
	/// </summary>
	public class SurfaceModifier : MonoBehaviour 
	{
		[Header("Friction")]
		[Information("Set a friction between 0.01 and 0.99 to get a slippery surface (close to 0 is very slippery, close to 1 is less slippery).\nOr set it above 1 to get a sticky surface. The higher the value, the stickier the surface.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the amount of friction to apply to a CorgiController walking over this surface		
		public float Friction;

		[Header("Force")]
		[Information("Use these to add X or Y (or both) forces to any CorgiController that gets grounded on this surface. Adding a X force will create a treadmill (negative value > treadmill to the left, positive value > treadmill to the right). A positive y value will create a trampoline, a bouncy surface, or a jumper for example.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the amount of force to add to a CorgiController walking over this surface
		public Vector2 AddedForce=Vector2.zero;

		/// <summary>
		/// Triggered when a CorgiController collides with the surface
		/// </summary>
		/// <param name="collider">Collider.</param>
		public virtual void OnTriggerStay2D(Collider2D collider)
		{
			CorgiController controller=collider.GetComponent<CorgiController>();
			if (controller==null)
				return;
			
			controller.AddHorizontalForce(AddedForce.x);
			controller.AddVerticalForce(Mathf.Sqrt( 2f * AddedForce.y * -controller.Parameters.Gravity ));
		}
	}
}