using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.CorgiEngine
{		
	[AddComponentMenu("Corgi Engine/Weapons/Projectile Weapon")]
	/// <summary>
	/// A weapon class aimed specifically at allowing the creation of various projectile weapons, from shotgun to machine gun, via plasma gun or rocket launcher
	/// </summary>
	public class ProjectileWeapon : Weapon 
	{
		[Header("Spawn")]
		/// the offset position at which the projectile will spawn
		public Vector3 ProjectileSpawnOffset = Vector3.zero;

		public ObjectPooler ObjectPooler { get; set; }
		protected WeaponAim _aimableWeapon;
		protected Vector3 _spawnPosition = Vector3.zero;

		/// <summary>
		/// Initialize this weapon
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			_aimableWeapon = GetComponent<WeaponAim> ();

			if (GetComponent<MultipleObjectPooler>() != null)
			{
				ObjectPooler = GetComponent<MultipleObjectPooler>();
			}
			if (GetComponent<SimpleObjectPooler>() != null)
			{
				ObjectPooler = GetComponent<SimpleObjectPooler>();
			}
			if (ObjectPooler == null)
			{
				Debug.LogWarning(this.name+" : no object pooler (simple or multiple) is attached to this Projectile Weapon, it won't be able to shoot anything.");
				return;
			}
		}

		/// <summary>
		/// Called everytime the weapon is used
		/// </summary>
		protected override void WeaponUse()
		{
			base.WeaponUse ();

			DetermineSpawnPosition ();

			/*_spawnPosition = this.transform.localPosition + this.transform.localRotation * ProjectileSpawnOffset;
			_spawnPosition = this.transform.TransformPoint (_spawnPosition);*/

			SpawnProjectile(_spawnPosition);
		}

		/// <summary>
		/// Spawns a new object and positions/resizes it
		/// </summary>
		public virtual GameObject SpawnProjectile(Vector3 spawnPosition,bool triggerObjectActivation=true)
		{
			/// we get the next object in the pool and make sure it's not null
			GameObject nextGameObject = ObjectPooler.GetPooledGameObject();

			// mandatory checks
			if (nextGameObject==null)	{ return null; }
			if (nextGameObject.GetComponent<PoolableObject>()==null)
			{
				throw new Exception(gameObject.name+" is trying to spawn objects that don't have a PoolableObject component.");		
			}	
			// we position the object
			nextGameObject.transform.position = spawnPosition;
			// we set its direction

			Projectile projectile = nextGameObject.GetComponent<Projectile>();
			if (projectile != null)
			{				
				projectile.SetWeapon(this);
				projectile.SetOwner(Owner.gameObject);
			}
			// we activate the object
			nextGameObject.gameObject.SetActive(true);


			if (projectile != null)
			{
				projectile.SetDirection (transform.right * (Flipped ? -1 : 1), transform.rotation, Owner.IsFacingRight);
			}

			if (triggerObjectActivation)
			{
				if (nextGameObject.GetComponent<PoolableObject>()!=null)
				{
					nextGameObject.GetComponent<PoolableObject>().TriggerOnSpawnComplete();
				}
			}

			return (nextGameObject);
		}

		/// <summary>
		/// Determines the spawn position based on the spawn offset and whether or not the weapon is flipped
		/// </summary>
		protected virtual void DetermineSpawnPosition()
		{
			if (Flipped)
			{
				_spawnPosition = this.transform.position - this.transform.rotation * ProjectileSpawnOffset;
			}
			else
			{
				_spawnPosition = this.transform.position + this.transform.rotation * ProjectileSpawnOffset;
			}
		}

		/// <summary>
		/// When the weapon is selected, draws a circle at the spawn's position
		/// </summary>
		protected virtual void OnDrawGizmosSelected()
		{
			DetermineSpawnPosition ();

			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(_spawnPosition, 0.2f);	
		}
	}
}
