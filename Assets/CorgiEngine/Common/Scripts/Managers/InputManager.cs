using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// This persistent singleton handles the inputs and sends commands to the player.
	/// IMPORTANT : this script's Execution Order MUST be -100.
	/// You can define a script's execution order by clicking on the script's file and then clicking on the Execution Order button at the bottom right of the script's inspector.
	/// See https://docs.unity3d.com/Manual/class-ScriptExecution.html for more details
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Input Manager")]
	public class InputManager : Singleton<InputManager>
	{
		[Header("Player binding")]
		[Information("The first thing you need to set on your InputManager is the PlayerID. This ID will be used to bind the input manager to your character(s). You'll want to go with Player1, Player2, Player3 or Player4.",InformationAttribute.InformationType.Info,false)]
		/// a string identifying the target player(s). You'll need to set this exact same string on your Character, and set its type to Player
		public string PlayerID = "Player1";
		/// the possible modes for this input manager
		public enum InputForcedMode { None, Mobile, Desktop }
		/// the possible kinds of control used for movement
		public enum MovementControls { Joystick, Arrows }
		[Header("Mobile controls")]
		[Information("If you check Auto Mobile Detection, the engine will automatically switch to mobile controls when your build target is Android or iOS. You can also force mobile or desktop (keyboard, gamepad) controls using the dropdown below.\nNote that if you don't need mobile controls and/or GUI this component can also work on its own, just put it on an empty GameObject instead.",InformationAttribute.InformationType.Info,false)]
		/// if this is set to true, the InputManager will try to detect what mode it should be in, based on the current target device
		public bool AutoMobileDetection = true;
		/// use this to force desktop (keyboard, pad) or mobile (touch) mode
		public InputForcedMode ForcedMode;
		/// if this is true, mobile controls will be hidden in editor mode, regardless of the current build target or the forced mode
		public bool HideMobileControlsInEditor = false;
		/// use this to specify whether you want to use the default joystick or arrows to move your character
		public MovementControls MovementControl = MovementControls.Joystick;
		/// if this is true, we're currently in mobile mode
		public bool IsMobile { get; protected set; }

		[Header("Movement settings")]
		[Information("Turn SmoothMovement on to have inertia in your controls (meaning there'll be a small delay between a press/release of a direction and your character moving/stopping). You can also define here the horizontal and vertical thresholds.",InformationAttribute.InformationType.Info,false)]
		/// If set to true, acceleration / deceleration will take place when moving / stopping
		public bool SmoothMovement=true;
		/// the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)
		public Vector2 Threshold = new Vector2(0.1f, 0.4f);

		/// the jump button, used for jumps and validation
		public MMInput.IMButton JumpButton { get; protected set; }
		/// the jetpack button
		public MMInput.IMButton JetpackButton { get; protected set; }
		/// the run button
		public MMInput.IMButton RunButton { get; protected set; }
		/// the dash button
		public MMInput.IMButton DashButton { get; protected set; }
		/// the shoot button
		public MMInput.IMButton ShootButton { get; protected set; }
		/// the reload button
		public MMInput.IMButton ReloadButton { get; protected set; }
		/// the pause button
		public MMInput.IMButton PauseButton { get; protected set; }
		/// the switch weapon button
		public MMInput.IMButton SwitchWeaponButton { get; protected set; }
		/// the shoot axis, used as a button (non analogic)
		public MMInput.ButtonStates ShootAxis { get; protected set; }
		/// the primary movement value (used to move the character around)
		public Vector2 PrimaryMovement {get { return _primaryMovement; } }
		/// the secondary movement (usually the right stick on a gamepad), used to aim
		public Vector2 SecondaryMovement {get { return _secondaryMovement; } }

		protected List<MMInput.IMButton> ButtonList;
		protected Vector2 _primaryMovement = Vector2.zero;
		protected Vector2 _secondaryMovement = Vector2.zero;
		protected string _axisHorizontal;
		protected string _axisVertical;
		protected string _axisSecondaryHorizontal;
		protected string _axisSecondaryVertical;
		protected string _axisShoot;

	    /// <summary>
	    /// On Start we look for what mode to use, and initialize our axis and buttons
	    /// </summary>
	    protected virtual void Start()
		{
			ControlsModeDetection();
			InitializeButtons ();
			InitializeAxis();
		}

		/// <summary>
		/// Turns mobile controls on or off depending on what's been defined in the inspector, and what target device we're on
		/// </summary>
		protected virtual void ControlsModeDetection()
		{
			if (GUIManager.Instance!=null)
			{
				GUIManager.Instance.SetMobileControlsActive(false);
				IsMobile=false;
				if (AutoMobileDetection)
				{
					#if UNITY_ANDROID || UNITY_IPHONE
					GUIManager.Instance.SetMobileControlsActive(true,MovementControl);
					IsMobile = true;
					 #endif
				}
				if (ForcedMode==InputForcedMode.Mobile)
				{
					GUIManager.Instance.SetMobileControlsActive(true,MovementControl);
					IsMobile = true;
				}
				if (ForcedMode==InputForcedMode.Desktop)
				{
					GUIManager.Instance.SetMobileControlsActive(false);
					IsMobile = false;					
				}
				if (HideMobileControlsInEditor)
				{
					#if UNITY_EDITOR
						GUIManager.Instance.SetMobileControlsActive(false);
						IsMobile = false;	
					#endif
				}
			}
		}

		/// <summary>
		/// Initializes the buttons. If you want to add more buttons, make sure to register them here.
		/// </summary>
		protected virtual void InitializeButtons()
		{
			ButtonList = new List<MMInput.IMButton> ();
			ButtonList.Add(JumpButton = new MMInput.IMButton (PlayerID, "Jump", JumpButtonDown, JumpButtonPressed, JumpButtonUp));
			ButtonList.Add(JetpackButton = new MMInput.IMButton (PlayerID, "Jetpack", JetpackButtonDown, JetpackButtonPressed, JetpackButtonUp)); 
			ButtonList.Add(RunButton  = new MMInput.IMButton (PlayerID, "Run", RunButtonDown, RunButtonPressed, RunButtonUp));
			ButtonList.Add(DashButton  = new MMInput.IMButton (PlayerID, "Dash", DashButtonDown, DashButtonPressed, DashButtonUp));
			ButtonList.Add(ShootButton = new MMInput.IMButton (PlayerID, "Shoot", ShootButtonDown, ShootButtonPressed, ShootButtonUp)); 
			ButtonList.Add(ReloadButton = new MMInput.IMButton (PlayerID, "Reload", ReloadButtonDown, ReloadButtonPressed, ReloadButtonUp));
			ButtonList.Add(SwitchWeaponButton = new MMInput.IMButton (PlayerID, "SwitchWeapon", SwitchWeaponButtonDown, SwitchWeaponButtonPressed, SwitchWeaponButtonUp));
			ButtonList.Add(PauseButton = new MMInput.IMButton (PlayerID, "Pause", PauseButtonDown, PauseButtonPressed, PauseButtonUp));
		}

		/// <summary>
		/// Initializes the axis strings.
		/// </summary>
		protected virtual void InitializeAxis()
		{
			_axisHorizontal = PlayerID+"_Horizontal";
			_axisVertical = PlayerID+"_Vertical";
			_axisSecondaryHorizontal = PlayerID+"_SecondaryHorizontal";
			_axisSecondaryVertical = PlayerID+"_SecondaryVertical";
			_axisShoot = PlayerID+"_ShootAxis";
		}

		/// <summary>
		/// On LateUpdate, we process our button states
		/// </summary>
		protected virtual void LateUpdate()
		{
			ProcessButtonStates();
		}

	    /// <summary>
	    /// At update, we check the various commands and update our values and states accordingly.
	    /// </summary>
	    protected virtual void Update()
		{		
			if (!IsMobile)
			{	
				SetMovement();	
				SetSecondaryMovement ();
				SetShootAxis ();
				GetInputButtons ();
			}									
		}

		/// <summary>
		/// If we're not on mobile, watches for input changes, and updates our buttons states accordingly
		/// </summary>
		protected virtual void GetInputButtons()
		{
			foreach(MMInput.IMButton button in ButtonList)
			{
				if (Input.GetButton(button.ButtonID))
				{
					button.TriggerButtonPressed ();
				}
				if (Input.GetButtonDown(button.ButtonID))
				{
					button.TriggerButtonDown ();
				}
				if (Input.GetButtonUp(button.ButtonID))
				{
					button.TriggerButtonUp ();
				}
			}
		}

		/// <summary>
		/// Called at LateUpdate(), this method processes the button states of all registered buttons
		/// </summary>
		public virtual void ProcessButtonStates()
		{
			// for each button, if we were at ButtonDown this frame, we go to ButtonPressed. If we were at ButtonUp, we're now Off
			foreach (MMInput.IMButton button in ButtonList)
			{
				if (button.State.CurrentState == MMInput.ButtonStates.ButtonDown)
				{
					button.State.ChangeState(MMInput.ButtonStates.ButtonPressed);				
				}	
				if (button.State.CurrentState == MMInput.ButtonStates.ButtonUp)
				{
					button.State.ChangeState(MMInput.ButtonStates.Off);				
				}	
			}
		}

		/// <summary>
		/// Called every frame, if not on mobile, gets primary movement values from input
		/// </summary>
		public virtual void SetMovement()
		{
			if (!IsMobile)
			{
				if (SmoothMovement)
				{
					_primaryMovement.x = Input.GetAxis(_axisHorizontal);
					_primaryMovement.y = Input.GetAxis(_axisVertical);		
				}
				else
				{
					_primaryMovement.x = Input.GetAxisRaw(_axisHorizontal);
					_primaryMovement.y = Input.GetAxisRaw(_axisVertical);		
				}
			}
		}

		/// <summary>
		/// Called every frame, if not on mobile, gets secondary movement values from input
		/// </summary>
		public virtual void SetSecondaryMovement()
		{
			if (!IsMobile)
			{
				if (SmoothMovement)
				{
					_secondaryMovement.x = Input.GetAxis(_axisSecondaryHorizontal);
					_secondaryMovement.y = Input.GetAxis(_axisSecondaryVertical);		
				}
				else
				{
					_secondaryMovement.x = Input.GetAxisRaw(_axisSecondaryHorizontal);
					_secondaryMovement.y = Input.GetAxisRaw(_axisSecondaryVertical);		
				}
			}
		}

		/// <summary>
		/// Called every frame, if not on mobile, gets shoot axis values from input
		/// </summary>
		protected virtual void SetShootAxis()
		{
			if (!IsMobile)
			{
				ShootAxis = MMInput.ProcessAxisAsButton (_axisShoot, Threshold.y, ShootAxis);
			}
		}

		/// <summary>
		/// If you're using a touch joystick, bind your main joystick to this method
		/// </summary>
		/// <param name="movement">Movement.</param>
		public virtual void SetMovement(Vector2 movement)
		{
			if (IsMobile)
			{
				_primaryMovement.x = movement.x;
				_primaryMovement.y = movement.y;	
			}
		}

		/// <summary>
		/// If you're using a touch joystick, bind your secondary joystick to this method
		/// </summary>
		/// <param name="movement">Movement.</param>
		public virtual void SetSecondaryMovement(Vector2 movement)
		{
			if (IsMobile)
			{
				_secondaryMovement.x = movement.x;
				_secondaryMovement.y = movement.y;	
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your left/right arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetHorizontalMovement(float horizontalInput)
		{
			if (IsMobile)
			{
				_primaryMovement.x = horizontalInput;
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your secondary down/up arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetVerticalMovement(float verticalInput)
		{
			if (IsMobile)
			{
				_primaryMovement.y = verticalInput;
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your secondary left/right arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetSecondaryHorizontalMovement(float horizontalInput)
		{
			if (IsMobile)
			{
				_secondaryMovement.x = horizontalInput;
			}
		}

		/// <summary>
		/// If you're using touch arrows, bind your down/up arrows to this method
		/// </summary>
		/// <param name="">.</param>
		public virtual void SetSecondaryVerticalMovement(float verticalInput)
		{
			if (IsMobile)
			{
				_secondaryMovement.y = verticalInput;
			}
		}

		public virtual void JumpButtonDown()		{ JumpButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void JumpButtonPressed()		{ JumpButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void JumpButtonUp()			{ JumpButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void DashButtonDown()		{ DashButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void DashButtonPressed()		{ DashButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void DashButtonUp()			{ DashButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void RunButtonDown()			{ RunButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void RunButtonPressed()		{ RunButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void RunButtonUp()			{ RunButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void JetpackButtonDown()		{ JetpackButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void JetpackButtonPressed()	{ JetpackButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void JetpackButtonUp()		{ JetpackButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void ReloadButtonDown()		{ ReloadButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void ReloadButtonPressed()	{ ReloadButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void ReloadButtonUp()		{ ReloadButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void ShootButtonDown()		{ ShootButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void ShootButtonPressed()	{ ShootButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void ShootButtonUp()			{ ShootButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void PauseButtonDown()		{ PauseButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void PauseButtonPressed()	{ PauseButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void PauseButtonUp()			{ PauseButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

		public virtual void SwitchWeaponButtonDown()		{ SwitchWeaponButton.State.ChangeState (MMInput.ButtonStates.ButtonDown); }
		public virtual void SwitchWeaponButtonPressed()		{ SwitchWeaponButton.State.ChangeState (MMInput.ButtonStates.ButtonPressed); }
		public virtual void SwitchWeaponButtonUp()			{ SwitchWeaponButton.State.ChangeState (MMInput.ButtonStates.ButtonUp); }

	}
}