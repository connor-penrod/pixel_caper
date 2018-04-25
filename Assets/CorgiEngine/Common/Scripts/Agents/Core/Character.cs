using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;


namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// This class will pilot the CorgiController component of your character.
	/// This is where you'll implement all of your character's game rules, like jump, dash, shoot, stuff like that.
	/// Animator parameters : Grounded (bool), xSpeed (float), ySpeed (float), 
	/// CollidingLeft (bool), CollidingRight (bool), CollidingBelow (bool), CollidingAbove (bool), Idle (bool)
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Core/Character")] 
	public class Character : MonoBehaviour
	{		
		/// the possible character types : player controller or AI (controlled by the computer)
		public enum CharacterTypes { Player, AI }
		/// the possible initial facing direction for your character
		public enum FacingDirections { Left, Right }
		/// the possible directions you can force your character to look at after its spawn
		public enum SpawnFacingDirections { Default, Left, Right }

		[Information("The Character script is the mandatory basis for all Character abilities. Your character can either be a Non Player Character, controlled by an AI, or a Player character, controlled by the player. In this case, you'll need to specify a PlayerID, which must match the one specified in your InputManager. Usually 'Player1', 'Player2', etc.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// Is the character player-controlled or controlled by an AI ?
		public CharacterTypes CharacterType = CharacterTypes.AI;
		/// Only used if the character is player-controlled. The PlayerID must match an input manager's PlayerID. It's also used to match Unity's input settings. So you'll be safe if you keep to Player1, Player2, Player3 or Player4
		public string PlayerID = "";				
		/// the various states of the character
		public CharacterStates CharacterState { get; protected set; }
	
		[Header("Direction")]
		/// true if the player is facing right
		[Information("It's usually good practice to build all your characters facing right. If that's not the case of this character, select Left instead.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		public FacingDirections InitialFacingDirection = FacingDirections.Right;

		/// the direction the character will face on spawn
		[Information("Here you can force a direction the character should face when spawning. If set to default, it'll match your model's initial facing direction.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		public SpawnFacingDirections DirectionOnSpawn = SpawnFacingDirections.Default;

		/// if this is true, the character is currently facing right
		public bool IsFacingRight { get; set; }
	
		[Header("Animator")]
		[Information("The engine will try and find an animator for this character. If it's on the same gameobject it should have found it. If it's nested somewhere, you'll need to bind it below. You can also decide to get rid of it altogether, in that case, just uncheck 'use mecanim'.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the character animator
		public Animator CharacterAnimator;
		/// Set this to false if you want to implement your own animation system
		public bool UseDefaultMecanim = true;

		[Header("Model")]
		[Information("Leave this unbound if this is a regular, sprite-based character, and if the SpriteRenderer and the Character are on the same GameObject. If not, you'll want to parent the actual model to the Character object, and bind it below. See the 3D demo characters for an example of that. The idea behind that is that the model may move, flip, but the collider will remain unchanged.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the 'model' (can be any gameobject) used to manipulate the character. Ideally it's separated (and nested) from the collider/corgi controller/abilities, to avoid messing with collisions.
		public GameObject CharacterModel;
		[Information("You can also decide if the character must automatically flip when going backwards or not. Additionnally, if you're not using sprites, you can define here how the character's model's localscale will be affected by flipping. By default it flips on the x axis, but you can change that to fit your model.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// if true, the model will be flipped on direction change. By default it'll be a sprite inversion, and a localscale change if you're using a model (or Spine, or anything other than 'regular' sprites)
		public bool FlipOnDirectionChange = true;
		/// the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs
		public Vector3 FlipValue = new Vector3(-1,1,1);

		[Header("Events")]
		[Information("Here you can define whether or not you want to have that character trigger events when changing state. See the MMTools' State Machine doc for more info.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// If this is true, the Character's state machine will emit events when entering/exiting a state
		public bool SendStateChangeEvents = true;
		/// If this is true, a state machine processor component will be added and it'll emit events on updates (see state machine processor's doc for more details)
		public bool SendStateUpdateEvents = true;

		// State Machines
		public MMStateMachine<CharacterStates.MovementStates> MovementState;
		public MMStateMachine<CharacterStates.CharacterConditions> ConditionState;

		// associated camera and input manager
	    public CameraController SceneCamera { get; protected set; }
		public InputManager LinkedInputManager { get; protected set; }

	    public Animator _animator { get; protected set; }
		public List<string> _animatorParameters { get; set; }

		protected CorgiController _controller;
		protected SpriteRenderer _spriteRenderer;
	    protected Color _initialColor;
		protected CharacterAbility[] _characterAbilities;
	    protected float _originalGravity;
		protected Health _health;
		protected bool _spawnDirectionForced = false;

	    		
		/// <summary>
		/// Initializes this instance of the character
		/// </summary>
		protected virtual void Awake()
		{		
			Initialization();
		}

		/// <summary>
		/// Gets and stores input manager, camera and components
		/// </summary>
		protected virtual void Initialization()
		{
			// we initialize our state machines
			MovementState = new MMStateMachine<CharacterStates.MovementStates>(gameObject,SendStateChangeEvents);
			ConditionState = new MMStateMachine<CharacterStates.CharacterConditions>(gameObject,SendStateChangeEvents);

			if (InitialFacingDirection == FacingDirections.Left)
			{
				IsFacingRight = false;
			}
			else
			{
				IsFacingRight = true;
			}

			// we get the current input manager
			GetInputManager();
			// we get the main camera
			if (Camera.main == null) { return; }
			SceneCamera = Camera.main.GetComponent<CameraController>();
			// we store our components for further use 
			CharacterState = new CharacterStates();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_controller = GetComponent<CorgiController>();
			_characterAbilities = GetComponents<CharacterAbility>();
			_health = GetComponent<Health> ();
			if (CharacterAnimator != null)
			{
				_animator = CharacterAnimator;
			}
	        else
	        {
				_animator = GetComponent<Animator>();
	        }

	        if (_animator != null)
	        {
				InitializeAnimatorParameters();
	        }

			_originalGravity = _controller.Parameters.Gravity;		

			ForceSpawnDirection ();
		}

		/// <summary>
		/// Initializes the animator parameters.
		/// </summary>
		protected virtual void InitializeAnimatorParameters()
		{
			if (_animator == null) { return; }

			_animatorParameters = new List<string>();

			MMAnimator.AddAnimatorParamaterIfExists(_animator,"Grounded",AnimatorControllerParameterType.Bool,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"xSpeed",AnimatorControllerParameterType.Float,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"ySpeed",AnimatorControllerParameterType.Float,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"CollidingLeft",AnimatorControllerParameterType.Bool,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"CollidingRight",AnimatorControllerParameterType.Bool,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"CollidingBelow",AnimatorControllerParameterType.Bool,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"CollidingAbove",AnimatorControllerParameterType.Bool,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"Idle",AnimatorControllerParameterType.Bool,_animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(_animator,"Alive",AnimatorControllerParameterType.Bool,_animatorParameters);
		}

		/// <summary>
		/// Gets (if it exists) the InputManager matching the Character's Player ID
		/// </summary>
		protected virtual void GetInputManager()
		{
			// we get the corresponding input manager
			if (!string.IsNullOrEmpty (PlayerID))
			{
				LinkedInputManager = null;
				InputManager[] foundInputManagers = FindObjectsOfType(typeof(InputManager)) as InputManager[];
				foreach (InputManager foundInputManager in foundInputManagers) 
		        {
					if (foundInputManager.PlayerID == PlayerID)
					{
						LinkedInputManager = foundInputManager;
					}
		        }
			}
		}

		/// <summary>
		/// Sets the player ID
		/// </summary>
		/// <param name="newPlayerID">New player ID.</param>
		public virtual void SetPlayerID(string newPlayerID)
		{
			PlayerID = newPlayerID;
			GetInputManager();
		}
		
		/// <summary>
		/// This is called every frame.
		/// </summary>
		protected virtual void Update()
		{		
			EveryFrame();
				
		}

		/// <summary>
		/// We do this every frame. This is separate from Update for more flexibility.
		/// </summary>
		protected virtual void EveryFrame()
		{
			HandleCharacterStatus();

			// we process our abilities
			EarlyProcessAbilities();
			ProcessAbilities();
			LateProcessAbilities();

			// we send our various states to the animator.		 
			UpdateAnimators ();
			
		}

		/// <summary>
		/// Calls all registered abilities' Early Process methods
		/// </summary>
		protected virtual void EarlyProcessAbilities()
		{
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled && ability.AbilityInitialized)
				{
					ability.EarlyProcessAbility();
				}
			}
		}

		/// <summary>
		/// Calls all registered abilities' Process methods
		/// </summary>
		protected virtual void ProcessAbilities()
		{
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled && ability.AbilityInitialized)
				{
					ability.ProcessAbility();
				}
			}
		}

		/// <summary>
		/// Calls all registered abilities' Late Process methods
		/// </summary>
		protected virtual void LateProcessAbilities()
		{
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled && ability.AbilityInitialized)
				{
					ability.LateProcessAbility();
				}
			}
		}

	    /// <summary>
	    /// This is called at Update() and sets each of the animators parameters to their corresponding State values
	    /// </summary>
	    protected virtual void UpdateAnimators()
		{	
			if ((UseDefaultMecanim) && (_animator!= null))
			{ 
				MMAnimator.UpdateAnimatorBool(_animator,"Grounded",_controller.State.IsGrounded,_animatorParameters);
				MMAnimator.UpdateAnimatorBool(_animator,"Alive",(ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead),_animatorParameters);
				MMAnimator.UpdateAnimatorFloat(_animator,"xSpeed",_controller.Speed.x,_animatorParameters);
				MMAnimator.UpdateAnimatorFloat(_animator,"ySpeed",_controller.Speed.y,_animatorParameters);
				MMAnimator.UpdateAnimatorBool(_animator,"CollidingLeft",_controller.State.IsCollidingLeft,_animatorParameters);
				MMAnimator.UpdateAnimatorBool(_animator,"CollidingRight",_controller.State.IsCollidingRight,_animatorParameters);
				MMAnimator.UpdateAnimatorBool(_animator,"CollidingBelow",_controller.State.IsCollidingBelow,_animatorParameters);
				MMAnimator.UpdateAnimatorBool(_animator,"CollidingAbove",_controller.State.IsCollidingAbove,_animatorParameters);
				MMAnimator.UpdateAnimatorBool(_animator,"Idle",(MovementState.CurrentState == CharacterStates.MovementStates.Idle),_animatorParameters);

				foreach (CharacterAbility ability in _characterAbilities)
				{
					if (ability.enabled && ability.AbilityInitialized)
					{	
						ability.UpdateAnimator();
					}
				}
	        }
	    }

	    /// <summary>
	    /// Handles the character status.
	    /// </summary>
		protected virtual void HandleCharacterStatus()
		{
			// if the character is dead, we prevent it from moving horizontally		
			if (ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
			{
				_controller.SetHorizontalForce(0);
				return;
			}

			// if the character is frozen, we prevent it from moving
			if (ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)
			{
				_controller.GravityActive(false);
				_controller.SetForce(Vector2.zero);			
			}
		}

		/// <summary>
		/// Freezes this character.
		/// </summary>
		public virtual void Freeze()
		{
			_controller.GravityActive(false);
			_controller.SetForce(Vector2.zero);	
			ConditionState.ChangeState(CharacterStates.CharacterConditions.Frozen);
		}

		/// <summary>
		/// Unfreezes this character
		/// </summary>
		public virtual void UnFreeze()
		{
			_controller.GravityActive(true);
			ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
		}	    
		
		/// <summary>
		/// Use this method to force the controller to recalculate the rays, especially useful when the size of the character has changed.
		/// </summary>
		public virtual void RecalculateRays()
		{
			_controller.SetRaysParameters();
		}

		/// <summary>
		/// Called to disable the player (at the end of a level for example. 
		/// It won't move and respond to input after this.
		/// </summary>
		public virtual void Disable()
		{
			enabled=false;
			_controller.enabled=false;
			GetComponent<Collider2D>().enabled=false;		
		}
		
		/// <summary>
		/// Makes the player respawn at the location passed in parameters
		/// </summary>
		/// <param name="spawnPoint">The location of the respawn.</param>
		public virtual void RespawnAt(Transform spawnPoint, FacingDirections facingDirection)
		{
			if (!gameObject.activeInHierarchy)
			{
				//Debug.LogError("Spawn : your Character's gameobject is inactive");
				return;
			}

			// we make sure the character is facing right
			Face(facingDirection);

			// we raise it from the dead (if it was dead)
			ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
			// we re-enable its 2D collider
			GetComponent<Collider2D>().enabled=true;
			// we make it handle collisions again
			_controller.CollisionsOn();
			transform.position=spawnPoint.position;
			if (_health != null)
			{
				_health.ResetHealthToMaxHealth();
				_health.Revive ();
			}
		}
		
		/// <summary>
		/// Flips the character and its dependencies (jetpack for example) horizontally
		/// </summary>
		public virtual void Flip()
		{
			// if we don't want the character to flip, we do nothing and exit
			if (!FlipOnDirectionChange)
			{
				return;
			}

			// Flips the character horizontally
			if (CharacterModel != null)
			{
				CharacterModel.transform.localScale = Vector3.Scale(CharacterModel.transform.localScale,FlipValue) ;	
			}
			else
			{
				// if we're sprite renderer based, we revert the flipX attribute
				if (_spriteRenderer != null)
				{
					_spriteRenderer.flipX = !_spriteRenderer.flipX;
				}					
			}

			IsFacingRight = !IsFacingRight;

			// we tell all our abilities we should flip
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled)
				{
					ability.Flip();
				}
			}
		}

		/// <summary>
		/// Forces the character to face left or right on spawn (and respawn)
		/// </summary>
		protected virtual void ForceSpawnDirection()
		{
			if ((DirectionOnSpawn == SpawnFacingDirections.Default) || _spawnDirectionForced)
			{
				return;
			}
			else
			{
				_spawnDirectionForced = true;
				if (DirectionOnSpawn == SpawnFacingDirections.Left)
				{
					Face (FacingDirections.Left);
				}
				if (DirectionOnSpawn == SpawnFacingDirections.Right)
				{
					Face (FacingDirections.Right);					
				}
			}
		}

		/// <summary>
		/// Forces the character to face right or left
		/// </summary>
		/// <param name="facingDirection">Facing direction.</param>
		public virtual void Face(FacingDirections facingDirection)
		{
			// Flips the character horizontally
			if (facingDirection == FacingDirections.Right)
			{
				if (!IsFacingRight)
				{
					Flip();
				}
			}
			else
			{
				if (IsFacingRight)
				{
					Flip();
				}
			}
		}

		/// <summary>
		/// Called when the Character dies. 
		/// Calls every abilities' Reset() method, so you can restore settings to their original value if needed
		/// </summary>
		public virtual void Reset()
		{
			_spawnDirectionForced = false;
			if (_characterAbilities == null)
			{
				return;
			}
			if (_characterAbilities.Length == 0)
			{
				return;
			}
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled)
				{
					ability.Reset();
				}
			}
		}

		/// <summary>
		/// On revive, we force the spawn direction
		/// </summary>
		protected virtual void OnRevive()
		{
			ForceSpawnDirection ();
		}

		/// <summary>
		/// OnEnable, we register our OnRevive event
		/// </summary>
		protected virtual void OnEnable ()
		{
			if (_health != null)
			{
				_health.OnRevive += OnRevive;
			}
		}

		/// <summary>
		/// OnDisable, we unregister our OnRevive event
		/// </summary>
		protected virtual void OnDisable()
		{
			if (_health != null)
			{
				_health.OnRevive -= OnRevive;
			}			
		}
	}
}