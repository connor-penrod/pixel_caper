using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// This base class, meant to be extended (see ProjectileWeapon.cs for an example of that) handles rate of fire (rate of use actually), and ammo reloading
	/// </summary>
	[SelectionBase]
	public class Weapon : MonoBehaviour 
	{
		/// the possible use modes for the trigger
		public enum TriggerModes { SemiAuto, Auto }
		/// the possible states the weapon can be in
		public enum WeaponStates { WeaponIdle, WeaponStart, WeaponDelayBeforeUse, WeaponUse, WeaponDelayBetweenUses, WeaponStop, WeaponReloadNeeded, WeaponReloadStart, WeaponReload, WeaponReloadStop }

		[Header("Use")]
		/// is this weapon on semi or full auto ?
		public TriggerModes TriggerMode = TriggerModes.Auto;
		// the delay before use, that will be applied for every shot
		public float DelayBeforeUse = 0f;
		// the time (in seconds) between two shots		
		public float TimeBetweenUses = 1f;

		[Header("Magazine")]
		/// whether or not the weapon is magazine based. If it's not, it'll just take its ammo inside a global pool
		public bool MagazineBased = false;
		/// the size of the magazine
		public int MagazineSize = 30;
		/// if this is true, pressing the fire button when a reload is needed will reload the weapon. Otherwise you'll need to press the reload button
		public bool AutoReload;
		/// the time it takes to reload the weapon
		public float ReloadTime = 2f;
		/// the amount of ammo consumed everytime the weapon fires
		public int AmmoConsumedPerShot = 1;
		[ReadOnly]
		/// the current amount of ammo loaded inside the weapon
		public int CurrentAmmoLoaded = 0;

		[Header("Position")]
		/// an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.
		public Vector3 WeaponAttachmentOffset = Vector3.zero;
		/// should that weapon be flipped when the character flips ?
		public bool FlipWeaponOnCharacterFlip = true;
		/// the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs
		public Vector3 FlipValue = new Vector3(-1,1,1);

		[Header("Hands Position")]
		/// the transform to which the character's left hand should be attached to
		public Transform LeftHandHandle;
		/// the transform to which the character's right hand should be attached to
		public Transform RightHandHandle;

		[Header("Effects")]
		/// a list of effects to trigger when the weapon is used
		public List<ParticleSystem> ParticleEffects;

		[Header("Animation Parameters Names")]
		/// the name of the weapon's idle animation parameter : this will be true all the time except when the weapon is being used
		public string IdleAnimationParameter;
		/// the name of the weapon's start animation parameter : true at the frame where the weapon starts being used
		public string StartAnimationParameter;
		/// the name of the weapon's delay before use animation parameter : true when the weapon has been activated but hasn't been used yet
		public string DelayBeforeUseAnimationParameter;
		/// the name of the weapon's single use animation parameter : true at each frame the weapon activates (shoots)
		public string SingleUseAnimationParameter;
		/// the name of the weapon's in use animation parameter : true at each frame the weapon has started firing but hasn't stopped yet
		public string UseAnimationParameter;
		/// the name of the weapon's delay between each use animation parameter : true when the weapon is in use
		public string DelayBetweenUsesAnimationParameter;
		/// the name of the weapon stop animation parameter : true after a shot and before the next one or the weapon's stop 
		public string StopAnimationParameter;
		/// the name of the weapon reload start animation parameter
		public string ReloadStartAnimationParameter;
		/// the name of the weapon reload animation parameter
		public string ReloadAnimationParameter;
		/// the name of the weapon reload end animation parameter
		public string ReloadStopAnimationParameter;
		/// the name of the weapon's angle animation parameter
		public string WeaponAngleAnimationParameter;
		/// the name of the weapon's angle animation parameter, adjusted so it's always relative to the direction the character is currently facing
		public string WeaponAngleRelativeAnimationParameter;

		[Header("Sounds")]
		/// the sound to play when the weapon starts being used
		public AudioClip WeaponStartSfx;
		/// the sound to play while the weapon is in use
		public AudioClip WeaponUsedSfx;
		/// the sound to play when the weapon stops being used
		public AudioClip WeaponStopSfx;
		/// the sound to play when the weapon gets reloaded
		public AudioClip WeaponReloadSfx; 
		/// the sound to play when the weapon gets reloaded
		public AudioClip WeaponReloadNeededSfx; 

		/// the weapon's owner
		public Character Owner { get; protected set; }
		/// the weapon's owner's CharacterHandleWeapon component
		public CharacterHandleWeapon CharacterHandleWeapon {get; set;}
		/// if true, the weapon is flipped
	    public bool Flipped { get; protected set; }
		/// the WeaponAmmo component optionnally associated to this weapon
		public WeaponAmmo WeaponAmmo { get; protected set; }
		/// the weapon's state machine
		public MMStateMachine<WeaponStates> WeaponState;
		protected SpriteRenderer _spriteRenderer;

		protected float _delayBeforeUseCounter = 0f;
		protected float _delayBetweenUsesCounter = 0f;
		protected float _reloadingCounter = 0f;
		protected bool _triggerReleased = false;
		protected bool _reloading = false;

	    protected Vector3 _weaponOffset;

	    /// <summary>
	    /// Initialize this weapon.
	    /// </summary>
		public virtual void Initialize()
		{
			Flipped = false;
			_spriteRenderer = GetComponent<SpriteRenderer> ();
			SetParticleEffects (false);
			WeaponState = new MMStateMachine<WeaponStates>(gameObject,true);
			WeaponState.ChangeState(WeaponStates.WeaponIdle);
			WeaponAmmo = GetComponent<WeaponAmmo> ();
			if (WeaponAmmo == null)
			{
				CurrentAmmoLoaded = MagazineSize;
			}
		}

		/// <summary>
		/// Sets the weapon's owner
		/// </summary>
		/// <param name="newOwner">New owner.</param>
		public virtual void SetOwner(Character newOwner)
		{
			Owner = newOwner;
			CharacterHandleWeapon = Owner.GetComponent<CharacterHandleWeapon>();
		}

		/// <summary>
		/// Called by input, turns the weapon on
		/// </summary>
		public virtual void WeaponInputStart()
		{
			if (WeaponState.CurrentState == WeaponStates.WeaponIdle)
			{
				_triggerReleased = false;
				TurnWeaponOn ();
			}
		}

		/// <summary>
		/// Describes what happens when the weapon starts
		/// </summary>
		protected virtual void TurnWeaponOn()
		{
			SfxPlayWeaponStartSound();
			WeaponState.ChangeState(WeaponStates.WeaponStart);
		}

		/// <summary>
		/// On Update, we check if the weapon is or should be used
		/// </summary>
		protected virtual void Update()
		{

		}

		/// <summary>
		/// On LateUpdate, processes the weapon state
		/// </summary>
		protected virtual void LateUpdate()
		{
			ProcessWeaponState();
		}

		/// <summary>
		/// Called every lastUpdate, processes the weapon's state machine
		/// </summary>
		protected virtual void ProcessWeaponState()
		{
			if (WeaponState == null) { return; }

			switch (WeaponState.CurrentState)
			{
				case WeaponStates.WeaponStart:
					if (DelayBeforeUse > 0)
					{
						_delayBeforeUseCounter = DelayBeforeUse;
						WeaponState.ChangeState(WeaponStates.WeaponDelayBeforeUse);
					}
					else
					{
						ShootRequest ();
					}
					break;	

				case WeaponStates.WeaponDelayBeforeUse:
					_delayBeforeUseCounter -= Time.deltaTime;
					if (_delayBeforeUseCounter <= 0)
					{
						ShootRequest ();
					}
					break;

				case WeaponStates.WeaponUse:
					WeaponUse();
					_delayBetweenUsesCounter = TimeBetweenUses;
					WeaponState.ChangeState(WeaponStates.WeaponDelayBetweenUses);
					break;

				case WeaponStates.WeaponDelayBetweenUses:
					_delayBetweenUsesCounter -= Time.deltaTime;
					if (_delayBetweenUsesCounter <= 0)
					{
						if ((TriggerMode == TriggerModes.Auto) && !_triggerReleased)
						{
							ShootRequest ();
						}
						else
						{
							TurnWeaponOff ();
						}
					}
					break;

				case WeaponStates.WeaponStop:
					WeaponState.ChangeState(WeaponStates.WeaponIdle);
					break;

				case WeaponStates.WeaponReloadNeeded:
					ReloadNeeded ();
					WeaponState.ChangeState (WeaponStates.WeaponIdle);
					break;

				case WeaponStates.WeaponReloadStart:
					ReloadWeapon ();
					_reloadingCounter = ReloadTime;
					WeaponState.ChangeState(WeaponStates.WeaponReload);
					break;

				case WeaponStates.WeaponReload:
					_reloadingCounter -= Time.deltaTime;
					if (_reloadingCounter <= 0)
					{
						WeaponState.ChangeState(WeaponStates.WeaponReloadStop);						
					}
					break;

				case WeaponStates.WeaponReloadStop:
					_reloading = false;
					WeaponState.ChangeState(WeaponStates.WeaponIdle);
					if (WeaponAmmo == null)
					{
						CurrentAmmoLoaded = MagazineSize;
					}
					break;								
			}
		}

		/// <summary>
		/// Determines whether or not the weapon can fire
		/// </summary>
		protected virtual void ShootRequest()
		{
			// if we have a weapon ammo component, we determine if we have enough ammunition to shoot
			if (_reloading)
			{
				return;
			}

			if (MagazineBased)
			{
				if (WeaponAmmo != null)
				{
					if (WeaponAmmo.EnoughAmmoToFire())
					{
						WeaponState.ChangeState(WeaponStates.WeaponUse);	
					}
					else
					{
						if (AutoReload && MagazineBased)
						{
							InitiateReloadWeapon ();
						}
						else
						{
							WeaponState.ChangeState(WeaponStates.WeaponReloadNeeded);		
						}
					}
				}
				else
				{
					if (CurrentAmmoLoaded > 0)
					{
						WeaponState.ChangeState(WeaponStates.WeaponUse);	
						CurrentAmmoLoaded -= AmmoConsumedPerShot;
					}
					else
					{
						if (AutoReload)
						{
							InitiateReloadWeapon ();
						}
						else
						{
							WeaponState.ChangeState(WeaponStates.WeaponReloadNeeded);	
						}
					}
				}
			}
			else
			{
				if (WeaponAmmo != null)
				{
					if (WeaponAmmo.EnoughAmmoToFire())
					{
						WeaponState.ChangeState(WeaponStates.WeaponUse);	
					}
					else
					{
						WeaponState.ChangeState(WeaponStates.WeaponReloadNeeded);	
					}	
				}
				else
				{
					WeaponState.ChangeState(WeaponStates.WeaponUse);						
				}					
			}
		}

		/// <summary>
		/// When the weapon is used, plays the corresponding sound
		/// </summary>
		protected virtual void WeaponUse()
		{	
			SetParticleEffects (true);		
			SfxPlayWeaponUsedSound();
		}

		/// <summary>
		/// Called by input, turns the weapon off if in auto mode
		/// </summary>
		public virtual void WeaponInputStop()
		{
			_triggerReleased = true	;		
		}

		/// <summary>
		/// Turns the weapon off.
		/// </summary>
		protected virtual void TurnWeaponOff()
		{
			SfxPlayWeaponStopSound();
			WeaponState.ChangeState(WeaponStates.WeaponStop);
		}

		/// <summary>
		/// Sets the particle effects on or off
		/// </summary>
		/// <param name="status">If set to <c>true</c> status.</param>
		protected virtual void SetParticleEffects(bool status)
		{
			foreach (ParticleSystem system in ParticleEffects)
			{
				if (system == null) { return; }

				if (status)
				{
					system.Play();
				}
				else
				{
					system.Pause();
				}
			}
		}	

		/// <summary>
		/// Describes what happens when the weapon needs a reload
		/// </summary>
		protected virtual void ReloadNeeded()
		{
			SfxPlayWeaponReloadNeededSound ();
		}

		public virtual void InitiateReloadWeapon()
		{
			// if we're already reloading, we do nothing and exit
			if (_reloading)
			{
				return;
			}
			WeaponState.ChangeState(WeaponStates.WeaponReloadStart);
			_reloading = true;
		}

		/// <summary>
		/// Reloads the weapon
		/// </summary>
		/// <param name="ammo">Ammo.</param>
		protected virtual void ReloadWeapon()
		{
			if (MagazineBased)
			{
				SfxPlayWeaponReloadSound();	
			}
		}

		/// <summary>
		/// Flips the weapon.
		/// </summary>
		public virtual void FlipWeapon()
		{			
			Flipped = !Flipped;
		}

		/// <summary>
		/// Flips the weapon model.
		/// </summary>
		public virtual void FlipWeaponModel()
		{			
			if (_spriteRenderer != null)
			{
				_spriteRenderer.flipX = !_spriteRenderer.flipX;
			} 
			else
			{
				transform.localScale = Vector3.Scale (transform.localScale, FlipValue);		
			}	
			// we apply the offset
			if (CharacterHandleWeapon != null)
			{
				_weaponOffset = CharacterHandleWeapon.WeaponAttachment.transform.position + WeaponAttachmentOffset;
				if (Flipped)
				{
					_weaponOffset.x = CharacterHandleWeapon.WeaponAttachment.transform.position.x - WeaponAttachmentOffset.x;
				}
				transform.position = _weaponOffset;

			}		
		}

		/// <summary>
		/// Plays the weapon's start sound
		/// </summary>
		protected virtual void SfxPlayWeaponStartSound()
		{
			if (WeaponStartSfx!=null) {	SoundManager.Instance.PlaySound(WeaponStartSfx,transform.position);	}
		}	

		/// <summary>
		/// Plays the weapon's used sound
		/// </summary>
		protected virtual void SfxPlayWeaponUsedSound()
		{
			if (WeaponUsedSfx!=null) {	SoundManager.Instance.PlaySound(WeaponUsedSfx,transform.position);	}
		}	

		/// <summary>
		/// Plays the weapon's stop sound
		/// </summary>
		protected virtual void SfxPlayWeaponStopSound()
		{
			if (WeaponStopSfx!=null) {	SoundManager.Instance.PlaySound(WeaponStopSfx,transform.position);	}
		}	

		/// <summary>
		/// Plays the weapon's reload needed sound
		/// </summary>
		protected virtual void SfxPlayWeaponReloadNeededSound()
		{
			if (WeaponReloadNeededSfx!=null) {	SoundManager.Instance.PlaySound(WeaponReloadNeededSfx,transform.position); }
		}	

		/// <summary>
		/// Plays the weapon's reload sound
		/// </summary>
		protected virtual void SfxPlayWeaponReloadSound()
		{
			if (WeaponReloadSfx!=null) {	SoundManager.Instance.PlaySound(WeaponReloadSfx,transform.position); }
		}		
	}
}