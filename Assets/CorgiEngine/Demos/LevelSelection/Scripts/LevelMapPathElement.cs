using UnityEngine;
using System.Collections;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// A component to add on level maps path points
	/// </summary>
	public class LevelMapPathElement : MonoBehaviour 
	{
		/// should the character automatically move to the next point when reaching this one
		public bool AutomaticMovement=false;

		[Header("Possible Directions")]
		/// the path element the character should go to when going up
		public LevelMapPathElement Up;
		/// the path element the character should go to when going right
		public LevelMapPathElement Right;
		/// the path element the character should go to when going down
		public LevelMapPathElement Down;
		/// the path element the character should go to when going left
		public LevelMapPathElement Left;


		/// <summary>
		/// Determines whether this instance can go up.
		/// </summary>
		/// <returns><c>true</c> if this instance can go up; otherwise, <c>false</c>.</returns>
		public virtual bool CanGoUp()
		{
			if (Up!=null) { return true; } else { return false; }
		}
		/// <summary>
		/// Determines whether this instance can go right.
		/// </summary>
		/// <returns><c>true</c> if this instance can go right; otherwise, <c>false</c>.</returns>
		public virtual bool CanGoRight()
		{
			if (Right!=null) { return true; } else { return false; }
		}
		/// <summary>
		/// Determines whether this instance can go down.
		/// </summary>
		/// <returns><c>true</c> if this instance can go down; otherwise, <c>false</c>.</returns>
		public virtual bool CanGoDown()
		{
			if (Down!=null) { return true; } else { return false; }
		}
		/// <summary>
		/// Determines whether this instance can go left.
		/// </summary>
		/// <returns><c>true</c> if this instance can go left; otherwise, <c>false</c>.</returns>
		public virtual bool CanGoLeft()
		{
			if (Left!=null) { return true; } else { return false; }
		}

		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Start () 
		{
			// we turn our path element invisible (we want it on for positioning on scene view but not in our game
			GetComponent<SpriteRenderer>().enabled=false;
		}

		/// <summary>
		/// Triggered when a character enters our path element
		/// </summary>
		/// <param name="collider">Collider.</param>
		public virtual void OnTriggerStay2D(Collider2D collider)
		{

			LevelMapCharacter mapCharacter=collider.GetComponent<LevelMapCharacter>();
			if (mapCharacter==null)
				return;

			// we tell our character it's now colliding with a path element
			mapCharacter.CollidingWithAPathElement=true;	
			mapCharacter.SetCurrentPathElement(this);		

			// if our path element is on automatic, we'll direct our character to its next target
			if (AutomaticMovement)
			{
				if (mapCharacter.LastVisitedPathElement!=Up && Up!=null) { mapCharacter.SetDestination(Up); }
				if (mapCharacter.LastVisitedPathElement!=Right && Right!=null) { mapCharacter.SetDestination(Right); }
				if (mapCharacter.LastVisitedPathElement!=Down && Down!=null) { mapCharacter.SetDestination(Down); }
				if (mapCharacter.LastVisitedPathElement!=Left && Left!=null) { mapCharacter.SetDestination(Left); }
			}		
		}

		/// <summary>
		/// Triggered when a character exits our path element
		/// </summary>
		/// <param name="collider">Collider.</param>
		public virtual void OnTriggerExit2D(Collider2D collider)
		{
			LevelMapCharacter mapCharacter=collider.GetComponent<LevelMapCharacter>();
			if (mapCharacter==null)
				return;

			GUIManager.Instance.GetComponent<LevelSelectorGUI>().TurnOffLevelName();

			// we tell our character it's now not colliding with any path element
			mapCharacter.CollidingWithAPathElement=false;	
			mapCharacter.SetCurrentPathElement(null);	
			mapCharacter.LastVisitedPathElement=this;


		}

		/// <summary>
		/// Triggered when a character enters our path element
		/// </summary>
		/// <param name="collider">Collider.</param>
		public virtual void OnTriggerEnter2D(Collider2D collider)
		{
			LevelMapCharacter mapCharacter=collider.GetComponent<LevelMapCharacter>();
			if (mapCharacter==null)
				return;

			if (GetComponent<LevelSelector>() != null)
			{
				if (GUIManager.Instance.GetComponent<LevelSelectorGUI>() != null)
				{
					GUIManager.Instance.GetComponent<LevelSelectorGUI>().SetLevelName(GetComponent<LevelSelector>().LevelName.ToUpper());
				}
			}
		}


		/// <summary>
		/// On scene view, draws a line between a path element and the other path elements it's connected to
		/// if the connection goes both ways (you can go from A to B and from B to A using opposite directions)
		/// the line will be blue, otherwise it'll be red (useful to make sure you've connected all dots the way you wanted to
		/// </summary>
		protected virtual void OnDrawGizmos()
		{			
			if (Up!=null)
			{
				if (Up.Down==this){Gizmos.color = Color.blue;} else {Gizmos.color = Color.red;} 
				Gizmos.DrawLine(transform.position,Up.transform.position);
			}
			if (Right!=null)
			{
				if (Right.Left==this){Gizmos.color = Color.blue;} else {Gizmos.color = Color.red;} 
				Gizmos.DrawLine(transform.position,Right.transform.position);
			}
			if (Down!=null)
			{
				if (Down.Up==this){Gizmos.color = Color.blue;} else {Gizmos.color = Color.red;} 
				Gizmos.DrawLine(transform.position,Down.transform.position);
			}
			if (Left!=null)
			{
				if (Left.Right==this){Gizmos.color = Color.blue;} else {Gizmos.color = Color.red;} 
				Gizmos.DrawLine(transform.position,Left.transform.position);
			}
		}
	}
}
