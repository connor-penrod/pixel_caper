using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[AddComponentMenu("Corgi Engine/Weapons/Melee Weapon")] 
	/// <summary>
	/// A basic melee weapon class, that will activate a "hurt zone" when the weapon is used
	/// </summary>
	public class MeleeWeapon : Weapon 
	{
		/// the possible shapes for the melee weapon's damage area
		public enum MeleeDamageAreaShapes { Rectangle, Circle }

		[Header("Damage Area")]
		/// the shape of the damage area (rectangle or circle)
		public MeleeDamageAreaShapes DamageAreaShape = MeleeDamageAreaShapes.Rectangle;
		/// the size of the damage area
		public Vector2 AreaSize = new Vector2(1,1);
		/// the offset to apply to the damage area (from the weapon's attachment position
		public Vector2 AreaOffset = new Vector2(1,0);

		[Header("Damage Area Timing")]
		/// the initial delay to apply before triggering the damage area
		public float InitialDelay = 0f;
		/// the duration during which the damage area is active
		public float ActiveDuration = 1f;

		[Header("Damage Caused")]
		// the layers that will be damaged by this object
		public LayerMask TargetLayerMask;
		/// The amount of health to remove from the player's health
		public int DamageCaused = 10;
		/// the kind of knockback to apply
		public DamageOnTouch.KnockbackStyles Knockback;
		/// The force to apply to the object that gets damaged
		public Vector2 KnockbackForce = new Vector2(10,2);
		/// The duration of the invincibility frames after the hit (in seconds)
		public float InvincibilityDuration = 0.5f;

		protected Collider2D _damageArea;
		protected bool _attackInProgress = false;

		/// <summary>
		/// Initialization
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			CreateDamageArea();
			DisableDamageArea();
		}

		/// <summary>
		/// Creates the damage area.
		/// </summary>
		protected virtual void CreateDamageArea()
		{
			GameObject damageArea = new GameObject();

			damageArea.name = this.name+"DamageArea";
			damageArea.transform.position = this.transform.position;
			damageArea.transform.rotation = this.transform.rotation;
			damageArea.transform.SetParent(this.transform);

			if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
			{
				BoxCollider2D boxcollider2d = damageArea.AddComponent<BoxCollider2D>();
				boxcollider2d.offset = AreaOffset;
				boxcollider2d.size = AreaSize;
				_damageArea = boxcollider2d;
			}
			if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
			{
				CircleCollider2D circlecollider2d = damageArea.AddComponent<CircleCollider2D>();
				circlecollider2d.transform.position = this.transform.position + this.transform.rotation * AreaOffset;
				circlecollider2d.radius = AreaSize.x/2;
				_damageArea = circlecollider2d;
			}
			_damageArea.isTrigger = true;

			DamageOnTouch damageOnTouch = damageArea.AddComponent<DamageOnTouch>();
			damageOnTouch.TargetLayerMask = TargetLayerMask;
			damageOnTouch.DamageCaused = DamageCaused;
			damageOnTouch.DamageCausedKnockbackType = Knockback;
			damageOnTouch.DamageCausedKnockbackForce = KnockbackForce;
			damageOnTouch.InvincibilityDuration = InvincibilityDuration;
		}

		/// <summary>
		/// When the weapon is used, we trigger our attack routine
		/// </summary>
		protected override void WeaponUse()
		{
			base.WeaponUse ();
			StartCoroutine(MeleeWeaponAttack());
		}

		/// <summary>
		/// Triggers an attack, turning the damage area on and then off
		/// </summary>
		/// <returns>The weapon attack.</returns>
		protected virtual IEnumerator MeleeWeaponAttack()
		{
			if (_attackInProgress) { yield break; }

			_attackInProgress = true;
			yield return new WaitForSeconds(InitialDelay);
			EnableDamageArea();
			yield return new WaitForSeconds(ActiveDuration);
			DisableDamageArea();
			_attackInProgress = false;
		}

		/// <summary>
		/// Enables the damage area.
		/// </summary>
		protected virtual void EnableDamageArea()
		{
			_damageArea.enabled = true;
		}

		/// <summary>
		/// Disables the damage area.
		/// </summary>
		protected virtual void DisableDamageArea()
		{
			_damageArea.enabled = false;
		}

		/// <summary>
		/// When the weapon is selected in scene view, draws the damage area's shape
		/// </summary>
		protected virtual void OnDrawGizmosSelected()
		{
			if (_damageArea != null) { return; }

      		Gizmos.color = Color.white;
			if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
			{
				Gizmos.DrawWireSphere(this.transform.position + this.transform.rotation * AreaOffset, AreaSize.x/2);
			}
			if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
			{
				MMDebug.DrawGizmoRectangle(this.transform.position + this.transform.rotation * AreaOffset, AreaSize, Color.white);
			}
		}

	}
}
