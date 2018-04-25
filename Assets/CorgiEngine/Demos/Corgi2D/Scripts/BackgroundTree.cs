using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this script to a tree to make it dance slowly over time
	/// </summary>
	public class BackgroundTree : MonoBehaviour 
	{
		
		/// The speed (in seconds) at which a new target scale is determined. Bigger scaleSpeed means slower movement.
		public float scaleSpeed = 0.5f;
		/// The maximum distance between the transform and the new target scale 
		public float scaleDistance = 0.01f;	
		/// The rotation speed (in seconds). Bigger rotation speed means faster movement.
		public float rotationSpeed = 1f;
		/// The rotation amplitude (in degrees).
		public float rotationAmplitude = 3f;

	    protected Vector3 _scaleTarget;
	    protected Quaternion _rotationTarget;
	    protected float _accumulator = 0.0f;


	    /// <summary>
	    /// Initialize the targets
	    /// </summary>
	    protected virtual void Start () 
		{
			_scaleTarget = WiggleScale( );
			_rotationTarget = WiggleRotate();	
		}

	    /// <summary>
	    /// Every frame, we make the object dance
	    /// </summary>
	    protected virtual void Update () 
		{
			// Every scaleSpeed, a new scale target is determined.
			_accumulator += Time.deltaTime;
			if(_accumulator >= scaleSpeed)
			{
				_scaleTarget = WiggleScale();			
				_accumulator -= scaleSpeed;
			}
					
			// the local scale is lerped towards the target scale		
			var norm = Time.deltaTime/scaleSpeed;		
			Vector3 newLocalScale=Vector3.Lerp(transform.localScale, _scaleTarget, norm);		
			transform.localScale = newLocalScale;		
			
			// the transform rotation is rotated towards the target rotation
			var normRotation = Time.deltaTime*rotationSpeed;
			transform.rotation = Quaternion.RotateTowards( transform.rotation, _rotationTarget , normRotation );
			if(transform.rotation == _rotationTarget)
			{			
				_rotationTarget = WiggleRotate();
			}
			
		}

	    /// <summary>
	    /// Makes the scale of the object wiggle
	    /// </summary>
	    /// <returns>The object's new scale.</returns>
	    protected virtual Vector3 WiggleScale()
		{
			// Determines a new scale (only on x and y axis)
			return new Vector3((1 + Random.Range(-scaleDistance,scaleDistance)),(1 + Random.Range(-scaleDistance,scaleDistance)),1);
		}

	    /// <summary>
	    /// Makes the rotation of the object wiggle
	    /// </summary>
	    /// <returns>The object's new rotation.</returns>
	    protected virtual Quaternion WiggleRotate()
		{
			// Determines a new angle (only on the z axis)
			return Quaternion.Euler(0f, 0f, Random.Range(-rotationAmplitude,rotationAmplitude));
		}		
	}
}