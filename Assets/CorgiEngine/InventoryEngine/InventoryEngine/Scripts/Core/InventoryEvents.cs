using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.InventoryEngine
{	
	/// <summary>
	/// The possible inventory related events
	/// </summary>
	public enum MMInventoryEventType { Pick, Select, Click, Move, UseRequest, ItemUsed, EquipRequest, ItemEquipped, UnEquipRequest, ItemUnEquipped, Drop, Error, Redraw, ContentChanged, InventoryOpens, InventoryCloseRequest, InventoryCloses, InventoryLoaded }

	/// <summary>
	/// Inventory events are used throughout the Inventory Engine to let other interested classes know that something happened to an inventory.  
	/// </summary>
	public struct MMInventoryEvent
	{
		/// the type of event
		public MMInventoryEventType InventoryEventType;
		/// the slot involved in the event
		public InventorySlot Slot;
		/// the name of the inventory where the event happened
		public string TargetInventoryName;
		/// the item involved in the event
		public InventoryItem EventItem;
		/// the quantity involved in the event
		public int Quantity;
		/// the index inside the inventory at which the event happened
		public int Index;

		public MMInventoryEvent(MMInventoryEventType eventType, InventorySlot slot, string targetInventoryName, InventoryItem eventItem, int quantity, int index)
		{
			InventoryEventType = eventType;
			Slot = slot;
			TargetInventoryName = targetInventoryName;
			EventItem = eventItem;
			Quantity = quantity;
			Index = index;
		}
	}
}
