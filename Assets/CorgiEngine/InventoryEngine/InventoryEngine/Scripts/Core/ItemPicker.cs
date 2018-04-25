using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InventoryEngine
{	
	/// <summary>
	/// Add this component to an object so it can be picked and added to an inventory
	/// </summary>
	public class ItemPicker : MonoBehaviour 
	{
		/// the item that should be picked 
		[Information("Add this component to a Trigger box collider 2D and it'll make it pickable, and will add the specified item to its target inventory. Just drag a previously created item into the slot below. For more about how to create items, have a look at the documentation. Here you can also specify how many of that item should be picked when picking the object.",InformationAttribute.InformationType.Info,false)]
		public InventoryItem Item ;
		/// the quantity of that item that should be added to the inventory when picked
		public int Quantity = 1;

		/// <summary>
		/// Triggered when something collides with the coin
		/// </summary>
		/// <param name="collider">Other.</param>
		public virtual void OnTriggerEnter2D (Collider2D collider) 
		{
			// if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
			if (collider.tag!="Player")
				return;

			Pick();

			// we desactivate the gameobject
			gameObject.SetActive(false);
		}		

		/// <summary>
		/// Picks this item and adds it to its target inventory
		/// </summary>
		public virtual void Pick()
		{
			MMEventManager.TriggerEvent (new MMInventoryEvent (MMInventoryEventType.Pick, null, Item.TargetInventoryName, Item, Quantity, 0));
			Item.Pick();
		}

		/// <summary>
		/// Picks this item and adds it to the target inventory specified as a parameter
		/// </summary>
		/// <param name="targetInventoryName">Target inventory name.</param>
		public virtual void Pick(string targetInventoryName)
		{
			Inventory targetInventory = null;
			if (targetInventoryName==null)
			{
				return;
			}
			foreach (Inventory inventory in UnityEngine.Object.FindObjectsOfType<Inventory>())
			{				
				if (inventory.name==targetInventoryName)
				{
					targetInventory = inventory;
				}
			}
			if (targetInventory==null)
			{
				return;
			}
			targetInventory.AddItem(Item,Quantity);
			Item.Pick();
		}
	}
}