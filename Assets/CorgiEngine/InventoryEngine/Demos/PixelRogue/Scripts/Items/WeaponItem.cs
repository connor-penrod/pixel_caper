using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.InventoryEngine
{	
	[CreateAssetMenu(fileName = "WeaponItem", menuName = "MoreMountains/InventoryEngine/WeaponItem", order = 2)]
	[Serializable]
	/// <summary>
	/// Demo class for a weapon item
	/// </summary>
	public class WeaponItem : InventoryItem 
	{
		[Header("Weapon")]
		/// the sprite to use to show the weapon when equipped
		public Sprite WeaponSprite;

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override void Equip()
		{
			base.Equip();
			InventoryDemoGameManager.Instance.Player.SetWeapon(WeaponSprite,this);
		}

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override void UnEquip()
		{
			base.UnEquip();
			InventoryDemoGameManager.Instance.Player.SetWeapon(null,this);
		}
		
	}
}