using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this component to an object and it will cause damage to objects that collide with it. 
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Damage/DamageOnTouch")] 
	public class DamageOnTouch : MonoBehaviour 
	{
		/// the possible ways to add knockback : noKnockback, which won't do nothing, set force, or add force
		public enum KnockbackStyles { NoKnockback, SetForce, AddForce }
		[Header("Targets")]
		[Information("This component will make your object cause damage to objects that collide with it. Here you can define what layers will be affected by the damage (for a standard enemy, choose Player), how much damage to give, and how much force should be applied to the object that gets the damage on hit. You can also specify how long the post-hit invincibility should last (in seconds).",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		// the layers that will be damaged by this object
		public LayerMask TargetLayerMask;

		[Header("Damage Caused")]
		/// The amount of health to remove from the player's health
		public int DamageCaused = 10;
		/// the type of knockback to apply when causing damage
		public KnockbackStyles DamageCausedKnockbackType = KnockbackStyles.SetForce;
		/// The force to apply to the object that gets damaged
		public Vector2 DamageCausedKnockbackForce = new Vector2(10,2);
		/// The duration of the invincibility frames after the hit (in seconds)
		public float InvincibilityDuration = 0.5f;

		[Header("Damage Taken")]
		[Information("After having applied the damage to whatever it collided with, you can have this object hurt itself. A bullet will explode after hitting a wall for example. Here you can define how much damage it'll take every time it hits something, or only when hitting something that's damageable, or non damageable. Note that this object will need a Health component too for this to be useful.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// The amount of damage taken every time, whether what we collide with is damageable or not
		public int DamageTakenEveryTime = 0;
		/// The amount of damage taken when colliding with a damageable object
		public int DamageTakenDamageable = 0;
		/// The amount of damage taken when colliding with something that is not damageable
		public int DamageTakenNonDamageable = 0;
		/// the type of knockback to apply when taking damage
		public KnockbackStyles DamageTakenKnockbackType = KnockbackStyles.NoKnockback;
		/// The force to apply to the object that gets damaged
		public Vector2 DamageTakenKnockbackForce = Vector2.zero;
		/// The duration of the invincibility frames after the hit (in seconds)
		public float DamageTakenInvincibilityDuration = 0.5f;

	    // storage		
		protected Vector2 _lastPosition, _velocity, _knockbackForce;
		protected float _startTime = 0f;
		protected Health _colliderHealth;
		protected CorgiController _corgiController;
		protected CorgiController _colliderCorgiController;
		protected Health _health;
		protected List<GameObject> _ignoredGameObjects;

		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Awake()
		{
			_ignoredGameObjects = new List<GameObject>();
			_health = GetComponent<Health>();
			_corgiController = GetComponent<CorgiController> ();
		}

		/// <summary>
		/// OnEnable we set the start time to the current timestamp
		/// </summary>
		protected virtual void OnEnable()
		{
			_startTime = Time.time;
		}

		/// <summary>
		/// During last update, we store the position and velocity of the object
		/// </summary>
		protected virtual void Update () 
		{
			ComputeVelocity();
		}

		/// <summary>
		/// Adds the gameobject set in parameters to the ignore list
		/// </summary>
		/// <param name="newIgnoredGameObject">New ignored game object.</param>
		public virtual void IgnoreGameObject(GameObject newIgnoredGameObject)
		{
			_ignoredGameObjects.Add(newIgnoredGameObject);
		}

		/// <summary>
		/// Removes the object set in parameters from the ignore list
		/// </summary>
		/// <param name="ignoredGameObject">Ignored game object.</param>
		public virtual void StopIgnoringObject(GameObject ignoredGameObject)
		{
			_ignoredGameObjects.Remove(ignoredGameObject);
		}

		/// <summary>
		/// Clears the ignore list.
		/// </summary>
		public virtual void ClearIgnoreList()
		{
			_ignoredGameObjects.Clear();
		}

		/// <summary>
		/// Computes the velocity based on the object's last position
		/// </summary>
		protected virtual void ComputeVelocity()
		{
			_velocity = (_lastPosition - (Vector2)transform.position) /Time.deltaTime;
			_lastPosition = transform.position;
		}
		
		/// <summary>
		/// When a collision with the player is triggered, we give damage to the player and knock it back
		/// </summary>
		/// <param name="collider">what's colliding with the object.</param>
		public virtual void OnTriggerStay2D(Collider2D collider)
		{			
			Colliding (collider);
	    }

		public virtual void OnTriggerEnter2D(Collider2D collider)
		{			
			Colliding (collider);
		}

		protected virtual void Colliding(Collider2D collider)
		{
			if (!this.isActiveAndEnabled)
			{
				return;
			}

			// if the object we're colliding with is part of our ignore list, we do nothing and exit
			if (_ignoredGameObjects.Contains(collider.gameObject))
			{
				return;
			}

			// if what we're colliding with isn't part of the target layers, we do nothing and exit
			if (!MMLayers.LayerInLayerMask(collider.gameObject.layer,TargetLayerMask))
			{
				return;
			}

			/*if (Time.time - _knockbackTimer < InvincibilityDuration)
			{
				return;
			}
			else
			{
				_knockbackTimer = Time.time;
			}*/

			_colliderHealth = collider.gameObject.GetComponentNoAlloc<Health>();

			// if what we're colliding with is damageable
			if (_colliderHealth != null)
			{
			    if(_colliderHealth.CurrentHealth > 0)
			    {
			        OnCollideWithDamageable(_colliderHealth);
			    }
			} 

			// if what we're colliding with can't be damaged
			else
			{
				OnCollideWithNonDamageable();
			}
		}

	    /// <summary>
	    /// Describes what happens when colliding with a damageable object
	    /// </summary>
	    /// <param name="health">Health.</param>
	    protected virtual void OnCollideWithDamageable(Health health)
	    {
			// if what we're colliding with is a CorgiController, we apply a knockback force
			_colliderCorgiController = health.gameObject.GetComponentNoAlloc<CorgiController>();

			if ((_colliderCorgiController != null) && (DamageCausedKnockbackForce != Vector2.zero) && (!_colliderHealth.Invulnerable))
			{
				Vector2 totalVelocity=_colliderCorgiController.Speed + _velocity;
				_knockbackForce.x = -1 * Mathf.Sign (totalVelocity.x) * DamageCausedKnockbackForce.x;
				_knockbackForce.y = DamageCausedKnockbackForce.y;	

				if (DamageCausedKnockbackType == KnockbackStyles.SetForce)
				{
					_colliderCorgiController.SetForce(_knockbackForce);	
				}
				if (DamageCausedKnockbackType == KnockbackStyles.AddForce)
				{
					_colliderCorgiController.AddForce(_knockbackForce);	
				}
			}

	    	// we apply the damage to the thing we've collided with
			_colliderHealth.Damage(DamageCaused, gameObject,InvincibilityDuration,InvincibilityDuration);
			SelfDamage(DamageTakenEveryTime + DamageTakenDamageable);
	    }

		/// <summary>
	    /// Describes what happens when colliding with a non damageable object
	    /// </summary>
	    protected virtual void OnCollideWithNonDamageable()
	    {
			SelfDamage(DamageTakenEveryTime + DamageTakenNonDamageable);
	    }

	    /// <summary>
	    /// Applies damage to itself
	    /// </summary>
	    /// <param name="damage">Damage.</param>
	    protected virtual void SelfDamage(int damage)
	    {
	    	if (_health != null)
	    	{
				_health.Damage(damage,gameObject,0f,DamageTakenInvincibilityDuration);
	    	}	

			// if what we're colliding with is a CorgiController, we apply a knockback force
			if (_corgiController != null)
			{
				Vector2 totalVelocity=_colliderCorgiController.Speed + _velocity;
				Vector2 knockbackForce = new Vector2(
					-1 * Mathf.Sign(totalVelocity.x) * DamageTakenKnockbackForce.x,
					-1 * Mathf.Sign(totalVelocity.y) * DamageTakenKnockbackForce.y	);	

				if (DamageTakenKnockbackType == KnockbackStyles.SetForce)
				{
					_corgiController.SetForce(knockbackForce);	
				}
				if (DamageTakenKnockbackType == KnockbackStyles.AddForce)
				{
					_corgiController.AddForce(knockbackForce);	
				}
			}
	    }
	}
}