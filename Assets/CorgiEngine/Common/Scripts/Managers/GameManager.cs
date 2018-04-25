using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// A list of the possible Corgi Engine base events
	/// </summary>
	public enum CorgiEngineEventTypes
	{
		LevelStart,
		LevelEnd,
		Pause,
		UnPause,
		PlayerDeath
	}

	/// <summary>
	/// A type of events used to signal level start and end (for now)
	/// </summary>
	public struct CorgiEngineEvent
	{
		public CorgiEngineEventTypes EventType;
		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.CorgiEngine.CorgiEngineEvent"/> struct.
		/// </summary>
		/// <param name="eventType">Event type.</param>
		public CorgiEngineEvent(CorgiEngineEventTypes eventType)
		{
			EventType = eventType;
		}
	} 

	/// <summary>
	/// A list of the methods available to change the current score
	/// </summary>
	public enum PointsMethods
	{
		Add,
		Set
	}

	/// <summary>
	/// A type of event used to signal changes to the current score
	/// </summary>
	public struct CorgiEnginePointsEvent
	{
		public PointsMethods PointsMethod;
		public int Points;
		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.CorgiEngine.CorgiEnginePointsEvent"/> struct.
		/// </summary>
		/// <param name="pointsMethod">Points method.</param>
		/// <param name="points">Points.</param>
		public CorgiEnginePointsEvent(PointsMethods pointsMethod, int points)
		{
			PointsMethod = pointsMethod;
			Points = points;
		}
	}

	/// <summary>
	/// A list of the methods available to change the current score
	/// </summary>
	public enum TimeScaleMethods
	{
		Set,
		For,
		Reset
	}

	public struct CorgiEngineTimeScaleEvent
	{
		public TimeScaleMethods TimeScaleMethod;
		public float TimeScale;
		public float Duration;

		public CorgiEngineTimeScaleEvent(TimeScaleMethods timeScaleMethod, float timeScale, float duration)
		{
			TimeScaleMethod = timeScaleMethod;
			TimeScale = timeScale;
			Duration = duration;
		}
	}

	public enum PauseMethods
	{
		PauseMenu,
		Inventory
	}

	/// <summary>
	/// The game manager is a persistent singleton that handles points and time
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Game Manager")]
	public class GameManager : 	PersistentSingleton<GameManager>, 
								MMEventListener<MMGameEvent>, 
								MMEventListener<CorgiEngineEvent>, 
								MMEventListener<CorgiEnginePointsEvent>,
								MMEventListener<CorgiEngineTimeScaleEvent>
	{		
		/// the target frame rate for the game
		public int TargetFrameRate=300;
		/// the current number of game points
		public int Points { get; private set; }
		/// true if the game is currently paused
		public bool Paused { get; set; } 
		// true if we've stored a map position at least once
		public bool StoredLevelMapPosition{ get; set; }
		/// the current player
		public Vector2 LevelMapPosition { get; set; }

	    // storage
		protected Stack<float> _savedTimeScale;
		protected bool _inventoryOpen = false;
		protected bool _pauseMenuOpen = false;
		protected InventoryInputManager _inventoryInputManager;

	    /// <summary>
	    /// On Start(), sets the target framerate to whatever's been specified
	    /// </summary>
	    protected virtual void Start()
	    {
			Application.targetFrameRate = TargetFrameRate;
			_savedTimeScale = new Stack<float> ();
	    }
					
		/// <summary>
		/// this method resets the whole game manager
		/// </summary>
		public virtual void Reset()
		{
			Points = 0;
			Time.timeScale = 1f;
			Paused = false;
			GUIManager.Instance.RefreshPoints ();
		}	
			
		/// <summary>
		/// Adds the points in parameters to the current game points.
		/// </summary>
		/// <param name="pointsToAdd">Points to add.</param>
		public virtual void AddPoints(int pointsToAdd)
		{
			Points += pointsToAdd;
			GUIManager.Instance.RefreshPoints ();
		}
		
		/// <summary>
		/// use this to set the current points to the one you pass as a parameter
		/// </summary>
		/// <param name="points">Points.</param>
		public virtual void SetPoints(int points)
		{
			Points = points;
			GUIManager.Instance.RefreshPoints ();
		}
		
		/// <summary>
		/// sets the timescale to the one in parameters
		/// </summary>
		/// <param name="newTimeScale">New time scale.</param>
		public virtual void SetTimeScale(float newTimeScale)
		{
			_savedTimeScale.Push(Time.timeScale);
			Time.timeScale = newTimeScale;
		}
		
		/// <summary>
		/// Resets the time scale to the last saved time scale.
		/// </summary>
		public virtual void ResetTimeScale()
		{
			if (_savedTimeScale.Count > 0)
			{
				Time.timeScale = _savedTimeScale.Peek();
				_savedTimeScale.Pop ();	
			}
			else
			{
				Time.timeScale = 1f;
			}
		}

		protected virtual void SetActiveInventoryInputManager(bool status)
		{
			_inventoryInputManager = GameObject.FindObjectOfType<InventoryInputManager> ();
			if (_inventoryInputManager != null)
			{
				_inventoryInputManager.enabled = status;
			}
		}
		
		/// <summary>
		/// Pauses the game or unpauses it depending on the current state
		/// </summary>
		public virtual void Pause(PauseMethods pauseMethod = PauseMethods.PauseMenu)
		{	
			if ((pauseMethod == PauseMethods.PauseMenu) && _inventoryOpen)
			{
				return;
			}

			// if time is not already stopped		
			if (Time.timeScale>0.0f)
			{
				Instance.SetTimeScale(0.0f);
				Instance.Paused=true;
				if ((GUIManager.Instance!= null) && (pauseMethod == PauseMethods.PauseMenu))
				{
					GUIManager.Instance.SetPause(true);	
					_pauseMenuOpen = true;
					SetActiveInventoryInputManager (false);
				}
				if (pauseMethod == PauseMethods.Inventory)
				{
					_inventoryOpen = true;
				}
			}
			else
			{
				UnPause(pauseMethod);
			}		
			LevelManager.Instance.ToggleCharacterPause();
		}

	    /// <summary>
	    /// Unpauses the game
	    /// </summary>
		public virtual void UnPause(PauseMethods pauseMethod = PauseMethods.PauseMenu)
	    {
	        Instance.ResetTimeScale();
	        Instance.Paused = false;
			if ((GUIManager.Instance!= null) && (pauseMethod == PauseMethods.PauseMenu))
	        { 
				GUIManager.Instance.SetPause(false);
				_pauseMenuOpen = false;
				SetActiveInventoryInputManager (true);
	        }
			if (_inventoryOpen)
			{
				_inventoryOpen = false;
			}
	    }

		/// <summary>
		/// Catches MMGameEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="gameEvent">MMGameEvent event.</param>
		public virtual void OnMMEvent(MMGameEvent gameEvent)
		{
			switch (gameEvent.EventName)
			{
				case "inventoryOpens":
					Pause (PauseMethods.Inventory);
					break;

				case "inventoryCloses":
					Pause (PauseMethods.Inventory);
					break;
			}
		}

		/// <summary>
		/// Catches CorgiEngineEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="engineEvent">CorgiEngineEvent event.</param>
		public virtual void OnMMEvent(CorgiEngineEvent engineEvent)
		{
			switch (engineEvent.EventType)
			{
				case CorgiEngineEventTypes.Pause:
					Pause ();
					break;
				
				case CorgiEngineEventTypes.UnPause:
					UnPause ();
					break;
			}
		}

		/// <summary>
		/// Catches CorgiEnginePointsEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="pointEvent">CorgiEnginePointsEvent event.</param>
		public virtual void OnMMEvent(CorgiEnginePointsEvent pointEvent)
		{
			switch (pointEvent.PointsMethod)
			{
				case PointsMethods.Set:
					SetPoints (pointEvent.Points);
					break;

				case PointsMethods.Add:
					AddPoints (pointEvent.Points);
					break;
			}
		}

		/// <summary>
		/// Catches CorgiEngineTimeScaleEvents and acts on them, playing the corresponding sounds
		/// </summary>
		/// <param name="timeScaleEvent">CorgiEngineTimeScaleEvent event.</param>
		public virtual void OnMMEvent(CorgiEngineTimeScaleEvent timeScaleEvent)
		{
			switch (timeScaleEvent.TimeScaleMethod)
			{
				case TimeScaleMethods.Reset:
					ResetTimeScale ();
					break;

				case TimeScaleMethods.Set:
					SetTimeScale (timeScaleEvent.TimeScale);
					break;

				case TimeScaleMethods.For:
					StartCoroutine (ChangeTimeScaleForCo (timeScaleEvent.TimeScale, timeScaleEvent.Duration));
					break;
			}
		}

		protected virtual IEnumerator ChangeTimeScaleForCo(float newTimeScale, float timeScaleDuration)
		{
			SetTimeScale (newTimeScale);
			GUIManager.Instance.SetTimeSplash (true);
			// we multiply the duration by the timespeed to get the real duration in seconds
			yield return new WaitForSeconds(timeScaleDuration);
			ResetTimeScale ();
			GUIManager.Instance.SetTimeSplash (false);
		}

		/// <summary>
		/// OnDisable, we start listening to events.
		/// </summary>
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent> ();
			this.MMEventStartListening<CorgiEngineEvent> ();
			this.MMEventStartListening<CorgiEnginePointsEvent> ();
			this.MMEventStartListening<CorgiEngineTimeScaleEvent> ();
		}

		/// <summary>
		/// OnDisable, we stop listening to events.
		/// </summary>
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent> ();
			this.MMEventStopListening<CorgiEngineEvent> ();
			this.MMEventStopListening<CorgiEnginePointsEvent> ();
			this.MMEventStopListening<CorgiEngineTimeScaleEvent> ();
		}
	}
}