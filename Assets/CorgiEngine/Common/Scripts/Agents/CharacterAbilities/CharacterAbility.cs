using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// A class meant to be overridden that handles a character's ability. 
	/// </summary>
	[RequireComponent(typeof(Character))]
	public class CharacterAbility : MonoBehaviour 
	{
		/// the sound fx to play when the ability starts
		public AudioClip AbilityStartSfx;
		/// the sound fx to play while the ability is running
		public AudioClip AbilityInProgressSfx;
		/// the sound fx to play when the ability stops
		public AudioClip AbilityStopSfx;

		/// if true, this ability can perform as usual, if not, it'll be ignored. You can use this to unlock abilities over time for example
		public bool AbilityPermitted = true;

		public bool AbilityInitialized { get { return _abilityInitialized; } }

		protected Character _character;
		protected Health _health;
		protected CharacterHorizontalMovement _characterBasicMovement;
		protected CorgiController _controller;
		protected InputManager _inputManager;
		protected CameraController _sceneCamera;
		protected Animator _animator;
		protected CharacterStates _state;
		protected SpriteRenderer _spriteRenderer;
		protected MMStateMachine<CharacterStates.MovementStates> _movement;
		protected MMStateMachine<CharacterStates.CharacterConditions> _condition;
		protected AudioSource _abilityInProgressSfx;
		protected bool _abilityInitialized = false;

		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public virtual string HelpBoxText() { return ""; }

		/// <summary>
		/// On Start(), we call the ability's intialization
		/// </summary>
		protected virtual void Start () 
		{
			Initialization();
		}

		/// <summary>
		/// Gets and stores components for further use
		/// </summary>
		protected virtual void Initialization()
		{
			_character = GetComponent<Character>();
			_controller = GetComponent<CorgiController>();
			_characterBasicMovement = GetComponent<CharacterHorizontalMovement>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_health = GetComponent<Health> ();
			_animator = _character._animator;
			_sceneCamera = _character.SceneCamera;
			_inputManager = _character.LinkedInputManager;
			_state = _character.CharacterState;
			_movement = _character.MovementState;
			_condition = _character.ConditionState;
			_abilityInitialized = true;
			if (_animator != null)
			{
				InitializeAnimatorParameters();
			}
		}

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		protected virtual void InitializeAnimatorParameters()
		{

		}

		/// <summary>
		/// Internal method to check if an input manager is present or not
		/// </summary>
		protected virtual void InternalHandleInput()
		{
			if (_inputManager==null) { return; }
			HandleInput();
		}

		/// <summary>
		/// Called at the very start of the ability's cycle, and intended to be overridden, looks for input and calls methods if conditions are met
		/// </summary>
		protected virtual void HandleInput()
		{

		}

		/// <summary>
		/// The first of the 3 passes you can have in your ability. Think of it as EarlyUpdate() if it existed
		/// </summary>
		public virtual void EarlyProcessAbility()
		{
			InternalHandleInput();
		}

		/// <summary>
		/// The second of the 3 passes you can have in your ability. Think of it as Update()
		/// </summary>
		public virtual void ProcessAbility()
		{
			
		}

		/// <summary>
		/// The last of the 3 passes you can have in your ability. Think of it as LateUpdate()
		/// </summary>
		public virtual void LateProcessAbility()
		{
			
		}

		/// <summary>
		/// Override this to send parameters to the character's animator. This is called once per cycle, by the Character class, after Early, normal and Late process().
		/// </summary>
		public virtual void UpdateAnimator()
		{

		}

		/// <summary>
		/// Changes the status of the ability's permission
		/// </summary>
		/// <param name="abilityPermitted">If set to <c>true</c> ability permitted.</param>
		public virtual void PermitAbility(bool abilityPermitted)
		{
			AbilityPermitted = abilityPermitted;
		}

		/// <summary>
		/// Override this to specify what should happen in this ability when the character flips
		/// </summary>
		public virtual void Flip()
		{
			
		}

		/// <summary>
		/// Override this to reset this ability's parameters. It'll be automatically called when the character gets killed, in anticipation for its respawn.
		/// </summary>
		public virtual void Reset()
		{
			
		}

		/// <summary>
		/// Plays the ability start sound effect
		/// </summary>
		protected virtual void PlayAbilityStartSfx()
		{
			if (AbilityStartSfx!=null) 
			{	
				SoundManager.Instance.PlaySound(AbilityStartSfx,transform.position);	
			}
		}	

		/// <summary>
		/// Plays the ability used sound effect
		/// </summary>
		protected virtual void PlayAbilityUsedSfx()
		{
			if (AbilityInProgressSfx!=null) 
			{	
				if (_abilityInProgressSfx == null)
				{
					_abilityInProgressSfx = SoundManager.Instance.PlaySound(AbilityInProgressSfx, transform.position, true);	
				}
			}
		}	

		/// <summary>
		/// Stops the ability used sound effect
		/// </summary>
		protected virtual void StopAbilityUsedSfx()
		{
			if (AbilityInProgressSfx!=null) 
			{	
				SoundManager.Instance.StopLoopingSound(_abilityInProgressSfx);	
				_abilityInProgressSfx = null;
			}
		}	

		/// <summary>
		/// Plays the ability stop sound effect
		/// </summary>
		protected virtual void PlayAbilityStopSfx()
		{
			if (AbilityStopSfx!=null) 
			{	
				SoundManager.Instance.PlaySound(AbilityStopSfx,transform.position);	
			}
		}

		/// <summary>
		/// Registers a new animator parameter to the list
		/// </summary>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="parameterType">Parameter type.</param>
		protected virtual void RegisterAnimatorParameter(string parameterName, AnimatorControllerParameterType parameterType)
		{
			if (_animator == null) 
			{
				return;
			}
			if (_animator.HasParameterOfType(parameterName, parameterType))
			{
				_character._animatorParameters.Add(parameterName);
			}
		}

		/// <summary>
		/// Override this to describe what should happen to this ability when the character respawns
		/// </summary>
		protected virtual void OnRespawn()
		{
		}

		/// <summary>
		/// Override this to describe what should happen to this ability when the character respawns
		/// </summary>
		protected virtual void OnDeath()
		{
			StopAbilityUsedSfx ();
		}

		/// <summary>
		/// On enable, we bind our respawn delegate
		/// </summary>
		protected virtual void OnEnable()
		{
			if (_health == null)
			{
				_health = GetComponent<Health> ();
			}

			if (_health != null)
			{
				_health.OnRevive += OnRespawn;
				_health.OnDeath += OnDeath;
			}
		}

		/// <summary>
		/// On disable, we unbind our respawn delegate
		/// </summary>
		protected virtual void OnDisable()
		{
			if (_health != null)
			{
				_health.OnRevive -= OnRespawn;
				_health.OnDeath -= OnDeath;
			}			
		}
	}
}