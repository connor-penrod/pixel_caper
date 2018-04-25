using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// An event typically fired when picking an item, letting listeners know what item has been picked
	/// </summary>
	public struct PickableItemEvent
	{
		public PickableItem PickedItem;
		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.CorgiEngine.PickableItemEvent"/> struct.
		/// </summary>
		/// <param name="pickedItem">Picked item.</param>
		public PickableItemEvent(PickableItem pickedItem)
		{
			PickedItem = pickedItem;
		}
	}

	/// <summary>
	/// Coin manager
	/// </summary>
	public class PickableItem : MonoBehaviour
	{
		/// The effect to instantiate when the coin is hit
		public GameObject Effect;
		/// The sound effect to play when the object gets picked
		public AudioClip PickSfx;

		protected Collider2D _collider;
		protected Character _character = null;
		protected bool _pickable = false;

		/// <summary>
		/// Triggered when something collides with the coin
		/// </summary>
		/// <param name="collider">Other.</param>
		public virtual void OnTriggerEnter2D (Collider2D collider) 
		{
			_collider = collider;

			if (CheckIfPickable ())
			{
				Effects ();

				MMEventManager.TriggerEvent(new PickableItemEvent(this));

				Pick ();

				// we desactivate the gameobject
				gameObject.SetActive(false);	
			}
		}

		/// <summary>
		/// Checks if the object is pickable.
		/// </summary>
		/// <returns><c>true</c>, if if pickable was checked, <c>false</c> otherwise.</returns>
		protected virtual bool CheckIfPickable()
		{
			// if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
			_character = _collider.GetComponent<Character>();
			if (_character == null)
			{
				return false;
			}
			if (_character.CharacterType != Character.CharacterTypes.Player)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Triggers the various pick effects
		/// </summary>
		protected virtual void Effects()
		{
			if (PickSfx!=null) 
			{	
				SoundManager.Instance.PlaySound(PickSfx,transform.position);	
			}

			if (Effect != null)
			{
				// adds an instance of the effect at the coin's position
				Instantiate(Effect,transform.position,transform.rotation);				
			}
		}

		/// <summary>
		/// Override this to describe what happens when the object gets picked
		/// </summary>
		protected virtual void Pick()
		{
			
		}
	}
}