using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this class to an area (water for example) and it will pass its parameters to any character that gets into it.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Environment/Corgi Controller Override")]
	public class CorgiControllerPhysicsVolume2D : MonoBehaviour 
	{
		public CorgiControllerParameters ControllerParameters;
	}
}