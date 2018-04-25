using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this ability to a Character to have it handle horizontal movement (walk, and potentially run, crawl, etc)
	/// Animator parameters : Speed (float), Walking (bool)
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement")] 
	public class CharacterHorizontalMovement : CharacterAbility 
	{
		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public override string HelpBoxText() { return "This component handles basic left/right movement, friction, and ground hit detection. Here you can define standard movement speed, walk speed, and what effects to use when the character hits the ground after a jump/fall."; }

		/// the current reference movement speed
		public float MovementSpeed { get; set; }
		[Header("Speed")]
		/// the speed of the character when it's walking
		public float WalkSpeed = 6f;

		[Header("Effects")]
		/// the effect that will be instantiated everytime the character touches the ground
		public ParticleSystem TouchTheGroundEffect;
		/// the sound effect to play when the character touches the ground
		public AudioClip TouchTheGroundSfx;

		protected float _horizontalMovement;
	    protected float _normalizedHorizontalSpeed;

		/// <summary>
		/// On Initialization, we set our movement speed to WalkSpeed.
		/// </summary>
		protected override void Initialization()
		{
			base.Initialization ();
			MovementSpeed = WalkSpeed;
		}

	    /// <summary>
	    /// The second of the 3 passes you can have in your ability. Think of it as Update()
	    /// </summary>
		public override void ProcessAbility()
	    {
			base.ProcessAbility();
			HandleHorizontalMovement();
	    }

	    /// <summary>
	    /// Called at the very start of the ability's cycle, and intended to be overridden, looks for input and calls
	    /// methods if conditions are met
	    /// </summary>
	    protected override void HandleInput()
	    {
			_horizontalMovement = _inputManager.PrimaryMovement.x;
	    }

		/// <summary>
		/// Sets the horizontal move value.
		/// </summary>
		/// <param name="value">Horizontal move value, between -1 and 1 - positive : will move to the right, negative : will move left </param>
		public virtual void SetHorizontalMove(float value)
		{
			_horizontalMovement=value;
		}

		/// <summary>
	    /// Called at Update(), handles horizontal movement
	    /// </summary>
	    protected virtual void HandleHorizontalMovement()
		{	
			// if we're not walking anymore, we stop our walking sound
			if (_movement.CurrentState != CharacterStates.MovementStates.Walking && _abilityInProgressSfx != null)
			{
				StopAbilityUsedSfx();
			}

			if (_movement.CurrentState == CharacterStates.MovementStates.Walking && _abilityInProgressSfx == null)
			{
				PlayAbilityUsedSfx();
			}

			// if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
			if ( !AbilityPermitted
				|| (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
				|| (_movement.CurrentState == CharacterStates.MovementStates.Gripping) )
			{
				return;				
			}

			// check if we just got grounded
			CheckJustGotGrounded();

			// If the value of the horizontal axis is positive, the character must face right.
			if (_horizontalMovement>0.1f)
			{
				_normalizedHorizontalSpeed = _horizontalMovement;
				if (!_character.IsFacingRight)
					_character.Flip();
			}		
			// If it's negative, then we're facing left
			else if (_horizontalMovement<-0.1f)
			{
				_normalizedHorizontalSpeed = _horizontalMovement;
				if (_character.IsFacingRight)
					_character.Flip();
			}
			else
			{
				_normalizedHorizontalSpeed=0;
			}

			// if we're grounded and moving, and currently Idle, Running or Dangling, we become Walking
			if ( (_controller.State.IsGrounded)
				&& (_normalizedHorizontalSpeed != 0)
				&& ( (_movement.CurrentState == CharacterStates.MovementStates.Idle)
					|| (_movement.CurrentState == CharacterStates.MovementStates.Dangling) ))
			{				
				_movement.ChangeState(CharacterStates.MovementStates.Walking);	
				PlayAbilityStartSfx();	
				PlayAbilityUsedSfx();		
			}

			// if we're walking and not moving anymore, we go back to the Idle state
			if ((_movement.CurrentState == CharacterStates.MovementStates.Walking) 
				&& (_normalizedHorizontalSpeed == 0))
			{
				_movement.ChangeState(CharacterStates.MovementStates.Idle);
				PlayAbilityStopSfx();
			}

			// if the character is not grounded, but currently idle or walking, we change its state to Falling
			if (!_controller.State.IsGrounded
				&& (
					(_movement.CurrentState == CharacterStates.MovementStates.Walking)
					 || (_movement.CurrentState == CharacterStates.MovementStates.Idle)
					))
			{
				_movement.ChangeState(CharacterStates.MovementStates.Falling);
			}

			// we pass the horizontal force that needs to be applied to the controller.
			float movementFactor = _controller.State.IsGrounded ? _controller.Parameters.SpeedAccelerationOnGround : _controller.Parameters.SpeedAccelerationInAir;
			float movementSpeed = _normalizedHorizontalSpeed * MovementSpeed * _controller.Parameters.SpeedFactor;
			float newHorizontalForce;

			newHorizontalForce = Mathf.Lerp(_controller.Speed.x, movementSpeed , Time.deltaTime * movementFactor);
						
			// we handle friction
			newHorizontalForce = HandleFriction(newHorizontalForce);

			// we set our newly computed speed to the controller
			_controller.SetHorizontalForce(newHorizontalForce);
		}

		/// <summary>
		/// Every frame, checks if we just hit the ground, and if yes, changes the state and triggers a particle effect
		/// </summary>
		protected virtual void CheckJustGotGrounded()
		{
			// if the character just got grounded
			if (_controller.State.JustGotGrounded)
			{
				_movement.ChangeState(CharacterStates.MovementStates.Idle);
				_controller.SlowFall (0f);			
	            if (TouchTheGroundEffect != null)
	            {
	                Instantiate(TouchTheGroundEffect, _controller.ColliderBottomPosition, transform.rotation);
	            }
				PlayTouchTheGroundSfx();
			}
		}

		/// <summary>
		/// Handles surface friction.
		/// </summary>
		/// <returns>The modified current force.</returns>
		/// <param name="force">the force we want to apply friction to.</param>
		protected virtual float HandleFriction(float force)
		{
			// if we have a friction above 1 (mud, water, stuff like that), we divide our speed by that friction
			if (_controller.Friction>1)
			{
				force = force/_controller.Friction;
			}

			// if we have a low friction (ice, marbles...) we lerp the speed accordingly
			if (_controller.Friction<1 && _controller.Friction > 0)
			{
				force = Mathf.Lerp(_controller.Speed.x, force, Time.deltaTime * _controller.Friction * 10);
			}

			return force;
		}

		/// <summary>
		/// Plays the touch the ground sfx. Triggered when hitting the ground from any state
		/// </summary>
		protected virtual void PlayTouchTheGroundSfx()
		{
			if (TouchTheGroundSfx!=null) {	SoundManager.Instance.PlaySound(TouchTheGroundSfx,transform.position); }
		}	


		/// <summary>
		/// A public method to reset the horizontal speed
		/// </summary>
		public virtual void ResetHorizontalSpeed()
		{
			MovementSpeed = WalkSpeed;
		}

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		protected override void InitializeAnimatorParameters()
		{
			RegisterAnimatorParameter ("Speed", AnimatorControllerParameterType.Float);
			RegisterAnimatorParameter ("Walking", AnimatorControllerParameterType.Bool);
		}

		/// <summary>
		/// Sends the current speed and the current value of the Walking state to the animator
		/// </summary>
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorFloat(_animator,"Speed",Mathf.Abs(_normalizedHorizontalSpeed),_character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(_animator,"Walking",(_movement.CurrentState == CharacterStates.MovementStates.Walking),_character._animatorParameters);
		}
	}
}