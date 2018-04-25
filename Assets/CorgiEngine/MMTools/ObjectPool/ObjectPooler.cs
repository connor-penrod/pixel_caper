using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// A base class, meant to be extended depending on the use (simple, multiple object pooler), and used as an interface by the spawners.
	/// Still handles common stuff like singleton and initialization on start().
	/// DO NOT add this class to a prefab, nothing would happen. Instead, add SimpleObjectPooler or MultipleObjectPooler.
	/// </summary>
	public class ObjectPooler : MonoBehaviour
	{
		/// singleton pattern
		public static ObjectPooler Instance;
		/// if this is true, the pool will try not to create a new waiting pool if it finds one with the same name.
		public bool MutualizeWaitingPools = false;

		/// this object is just used to group the pooled objects
		protected GameObject _waitingPool;

		/// <summary>
		/// On awake we fill our object pool
		/// </summary>
	    protected virtual void Awake()
	    {
			Instance = this;
			FillObjectPool();
	    }

		/// <summary>
		/// Creates the waiting pool or tries to reuse one if there's already one available
		/// </summary>
		protected virtual void CreateWaitingPool()
		{
			if (!MutualizeWaitingPools)
			{
				// we create a container that will hold all the instances we create
				_waitingPool = new GameObject(DetermineObjectPoolName());
				return;
			}
			else
			{
				GameObject waitingPool = GameObject.Find (DetermineObjectPoolName ());
				if (waitingPool != null)
				{
					_waitingPool = waitingPool;
				}
				else
				{
					_waitingPool = new GameObject(DetermineObjectPoolName());
				}
			}
		}

		/// <summary>
		/// Determines the name of the object pool.
		/// </summary>
		/// <returns>The object pool name.</returns>
		protected virtual string DetermineObjectPoolName()
		{
			return ("[ObjectPooler] " + this.name);	
		}

		/// <summary>
		/// Implement this method to fill the pool with objects
		/// </summary>
	    protected virtual void FillObjectPool()
	    {
	        return ;
	    }

		/// <summary>
		/// Implement this method to return a gameobject
		/// </summary>
		/// <returns>The pooled game object.</returns>
		public virtual GameObject GetPooledGameObject()
	    {
	        return null;
	    }
	}
}