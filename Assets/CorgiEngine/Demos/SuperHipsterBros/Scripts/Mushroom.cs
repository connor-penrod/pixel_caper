using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// Add this class to an object and it'll double the size of a character behavior if it touches one
	/// </summary>
	public class Mushroom : PickableItem
	{
		/// <summary>
		/// Checks if the object is pickable.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		protected override bool CheckIfPickable()
		{
			_character = _collider.GetComponent<Character>();

			// if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
			if ((_character == null) || (_collider.GetComponent<SuperHipsterBrosHealth>() == null))
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
		/// doubles the size of the character behavior when the object gets picked
		/// </summary>
		protected override void Pick()
		{
			// double the size of the character behavior
			_collider.GetComponent<SuperHipsterBrosHealth>().Grow(2f);
		}
	}
}