using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this class to a character so it can use weapons
	/// Note that this component will trigger animations (if their parameter is present in the Animator), based on 
	/// the current weapon's Animations
	/// Animator parameters : defined from the Weapon's inspector
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Handle Weapon")] 
	public class CharacterHandleWeapon : CharacterAbility 
	{
		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public override string HelpBoxText() { return "This component will allow your character to pickup and use weapons. What the weapon will do is defined in the Weapon classes. This just describes the behaviour of the 'hand' holding the weapon, not the weapon itself. Here you can set an initial weapon for your character to start with, allow weapon pickup, and specify a weapon attachment (a transform inside of your character, could be just an empty child gameobject, or a subpart of your model."; }

		/// the initial weapon owned by the character
		public Weapon InitialWeapon;	
		/// the position the weapon will be attached to. If left blank, will be this.transform.
		public Transform WeaponAttachment;
		/// if this is set to true, the character can pick up PickableWeapons
		public bool CanPickupWeapons = true;
		/// returns the current equipped weapon
		public Weapon CurrentWeapon { get; protected set; }

	    protected float _fireTimer = 0f;
		protected float _secondaryHorizontalMovement;
		protected float _secondaryVerticalMovement;
		protected WeaponAim _aimableWeapon;
		protected WeaponIK _weaponIK;
		protected Transform _leftHandTarget = null;
		protected Transform _rightHandTarget = null;

	    // Initialization
		protected override void Initialization () 
		{
			base.Initialization();
								
			Setup ();
		}

		/// <summary>
		/// Grabs various components and inits stuff
		/// </summary>
		public virtual void Setup()
		{
			// filler if the WeaponAttachment has not been set
			if (WeaponAttachment==null)
			{
				WeaponAttachment=transform;
			}		
			if (_animator != null)
			{
				_weaponIK = _animator.GetComponent<WeaponIK> ();
			}	
			// we set the initial weapon
			if (InitialWeapon != null)
			{
				ChangeWeapon(InitialWeapon);			
			}
			_character = gameObject.GetComponentNoAlloc<Character> ();
		}

		/// <summary>
		/// Every frame we check if it's needed to update the ammo display
		/// </summary>
		public override void ProcessAbility()
		{
			base.ProcessAbility ();
			UpdateAmmoDisplay ();
		}

		/// <summary>
		/// Gets input and triggers methods based on what's been pressed
		/// </summary>
		protected override void HandleInput ()
		{			

			if (_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
			{
				ShootStart();
			}

			if (_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonUp)
			{
				ShootStop();
			}

			if (_inputManager.ShootAxis == MMInput.ButtonStates.ButtonDown)
			{
				ShootStart();
			}

			if (_inputManager.ShootAxis == MMInput.ButtonStates.ButtonUp)
			{
				ShootStop();
			}

			if (_inputManager.ReloadButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
			{
				Reload();
			}
		}
						
		/// <summary>
		/// Causes the character to start shooting
		/// </summary>
		public virtual void ShootStart()
		{
			// if the Shoot action is enabled in the permissions, we continue, if not we do nothing.  If the player is dead we do nothing.
			if ( !AbilityPermitted
				|| (CurrentWeapon == null)
				|| (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
				|| (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing))
			{
				return;
			}									

			CurrentWeapon.WeaponInputStart();
		}
		
		/// <summary>
		/// Causes the character to stop shooting
		/// </summary>
		public virtual void ShootStop()
		{
			// if the Shoot action is enabled in the permissions, we continue, if not we do nothing
			if (!AbilityPermitted
				|| (CurrentWeapon == null))
			{
				return;		
			}		

			if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing && CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle)
			{
				return;
			}

			CurrentWeapon.WeaponInputStop();
		}

		/// <summary>
		/// Reloads the weapon
		/// </summary>
		protected virtual void Reload()
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.InitiateReloadWeapon ();
			}
		}
		
		/// <summary>
		/// Changes the character's current weapon to the one passed as a parameter
		/// </summary>
		/// <param name="newWeapon">The new weapon.</param>
		public virtual void ChangeWeapon(Weapon newWeapon)
		{

			// if the character already has a weapon, we make it stop shooting
			if(CurrentWeapon!=null)
			{
				ShootStop();
				Destroy(CurrentWeapon.gameObject);
			}

			if (newWeapon != null)
			{			
				CurrentWeapon=(Weapon)Instantiate(newWeapon,WeaponAttachment.transform.position + newWeapon.WeaponAttachmentOffset,WeaponAttachment.transform.rotation);	
				CurrentWeapon.transform.parent = WeaponAttachment.transform;
				CurrentWeapon.SetOwner (_character);
				_aimableWeapon = CurrentWeapon.GetComponent<WeaponAim> ();
				// we handle (optional) inverse kinematics (IK) 
				if (_weaponIK != null)
				{
					_weaponIK.SetHandles(CurrentWeapon.LeftHandHandle, CurrentWeapon.RightHandHandle);
				}
				// we turn off the gun's emitters.
				CurrentWeapon.Initialize();
				InitializeAnimatorParameters();
				if (!_character.IsFacingRight)
				{
					if (CurrentWeapon != null)
					{
						CurrentWeapon.FlipWeapon();
						CurrentWeapon.FlipWeaponModel();
					}
				}
			}
			else
			{
				CurrentWeapon = null;
			}
		}	

		/// <summary>
		/// Flips the current weapon if needed
		/// </summary>
		public override void Flip()
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.FlipWeapon();
				if (CurrentWeapon.FlipWeaponOnCharacterFlip)
				{
					CurrentWeapon.FlipWeaponModel();
				}
			}
		}

		/// <summary>
		/// Updates the ammo display bar and text.
		/// </summary>
		public virtual void UpdateAmmoDisplay()
		{
			if ( (GUIManager.Instance != null) && (_character.CharacterType == Character.CharacterTypes.Player) )
			{
				if (CurrentWeapon == null)
				{
					GUIManager.Instance.SetAmmoDisplays (false, _character.PlayerID);
					return;
				}

				if (!CurrentWeapon.MagazineBased && (CurrentWeapon.WeaponAmmo == null))
				{
					GUIManager.Instance.SetAmmoDisplays (false, _character.PlayerID);
					return;
				}

				if (CurrentWeapon.WeaponAmmo == null)
				{					
					GUIManager.Instance.SetAmmoDisplays (true, _character.PlayerID);
					GUIManager.Instance.UpdateAmmoDisplays(CurrentWeapon.MagazineBased, 0, 0, CurrentWeapon.CurrentAmmoLoaded, CurrentWeapon.MagazineSize, _character.PlayerID, false);	
					return;
				}
				else
				{
					GUIManager.Instance.SetAmmoDisplays (true, _character.PlayerID);
					GUIManager.Instance.UpdateAmmoDisplays(CurrentWeapon.MagazineBased, CurrentWeapon.WeaponAmmo.CurrentAmmoAvailable, CurrentWeapon.WeaponAmmo.MaxAmmo, CurrentWeapon.CurrentAmmoLoaded, CurrentWeapon.MagazineSize, _character.PlayerID, true);	
					return;
				}
			}
		}

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		protected override void InitializeAnimatorParameters()
		{
			if (CurrentWeapon == null)
			{	return; }

			RegisterAnimatorParameter(CurrentWeapon.WeaponAngleAnimationParameter, AnimatorControllerParameterType.Float);
			RegisterAnimatorParameter(CurrentWeapon.WeaponAngleRelativeAnimationParameter, AnimatorControllerParameterType.Float);
			RegisterAnimatorParameter(CurrentWeapon.IdleAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.StartAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.DelayBeforeUseAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.DelayBetweenUsesAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.StopAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.ReloadStartAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.ReloadStopAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.ReloadAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.SingleUseAnimationParameter, AnimatorControllerParameterType.Bool);
			RegisterAnimatorParameter(CurrentWeapon.UseAnimationParameter, AnimatorControllerParameterType.Bool);
		}

		/// <summary>
		/// Override this to send parameters to the character's animator. This is called once per cycle, by the Character
		/// class, after Early, normal and Late process().
		/// </summary>
		public override void UpdateAnimator()
		{
			if (CurrentWeapon == null)
			{	return; }

			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.IdleAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.StartAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponStart),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.DelayBeforeUseAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBeforeUse),_character._animatorParameters);

			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.UseAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBeforeUse || CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse || CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses ),_character._animatorParameters);

			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.SingleUseAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.DelayBetweenUsesAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.StopAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponStop),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.ReloadStartAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStart),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.ReloadAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReload),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,CurrentWeapon.ReloadStopAnimationParameter,(CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStop),_character._animatorParameters);

			if (_aimableWeapon != null)
			{
				MMAnimator.UpdateAnimatorFloat (_animator, CurrentWeapon.WeaponAngleAnimationParameter, _aimableWeapon.CurrentAngle,_character._animatorParameters);
				MMAnimator.UpdateAnimatorFloat (_animator, CurrentWeapon.WeaponAngleRelativeAnimationParameter, _aimableWeapon.CurrentAngleRelative,_character._animatorParameters);
			}
			else
			{
				MMAnimator.UpdateAnimatorFloat (_animator, CurrentWeapon.WeaponAngleAnimationParameter, 0f,_character._animatorParameters);
				MMAnimator.UpdateAnimatorFloat (_animator, CurrentWeapon.WeaponAngleRelativeAnimationParameter, 0f,_character._animatorParameters);
			}
		}
	}
}