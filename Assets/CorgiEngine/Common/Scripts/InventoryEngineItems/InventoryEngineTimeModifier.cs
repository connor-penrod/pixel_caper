using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{	
	[CreateAssetMenu(fileName = "InventoryEngineTimeModifier", menuName = "MoreMountains/CorgiEngine/InventoryEngineTimeModifier", order = 2)]
	[Serializable]
	/// <summary>
	/// Pickable health item
	/// </summary>
	public class InventoryEngineTimeModifier : InventoryItem 
	{
		[Header("Time Modifier")]
		[Information("Here you need to specify the new time speed and the duration for which the timescale will be changed.",InformationAttribute.InformationType.Info,false)]
		/// the time speed to apply while the effect lasts
		public float TimeSpeed = 0.5f;
		/// how long the duration will last , in seconds
		public float Duration = 1.0f;

		protected WaitForSeconds _changeTimeWFS;

		public override void Use()
		{
			base.Use();
			MMEventManager.TriggerEvent (new MMInventoryEvent (MMInventoryEventType.InventoryCloseRequest, null, this.name, null, 0, 0));
			MMEventManager.TriggerEvent (new CorgiEngineTimeScaleEvent (TimeScaleMethods.For, TimeSpeed, Duration));
		}	
	}
}
