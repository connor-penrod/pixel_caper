using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{	
	[CreateAssetMenu(fileName = "InventoryEngineWeapon", menuName = "MoreMountains/CorgiEngine/InventoryEngineWeapon", order = 2)]
	[Serializable]
	/// <summary>
	/// Weapon item in the Corgi Engine
	/// </summary>
	public class InventoryEngineWeapon : InventoryItem 
	{

		[Header("Weapon")]
		[Information("Here you need to bind the weapon you want to equip when picking that item.",InformationAttribute.InformationType.Info,false)]
		public Weapon EquippableWeapon;

		/// <summary>
		/// When we grab the weapon, we equip it
		/// </summary>
		public override void Equip()
		{
			EquipWeapon (EquippableWeapon);
		}

		/// <summary>
		/// When dropping or unequipping the weapon, we remove it
		/// </summary>
		public override void UnEquip()
		{
			EquipWeapon (null);
		}

		/// <summary>
		/// Grabs the CharacterHandleWeapon component and sets the weapon
		/// </summary>
		/// <param name="newWeapon">New weapon.</param>
		protected virtual void EquipWeapon(Weapon newWeapon)
		{
			if (EquippableWeapon == null)
			{
				return;
			}
			if (TargetInventory.Owner == null)
			{
				return;
			}
			CharacterHandleWeapon characterHandleWeapon = TargetInventory.Owner.GetComponent<CharacterHandleWeapon>();
			if (characterHandleWeapon != null)
			{
				characterHandleWeapon.ChangeWeapon (newWeapon);
			}
		}
	}
}
