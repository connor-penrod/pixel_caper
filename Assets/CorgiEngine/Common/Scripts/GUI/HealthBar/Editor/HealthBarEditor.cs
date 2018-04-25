using UnityEngine;
using MoreMountains.Tools;
using UnityEditor;
using UnityEngine.UI;

namespace MoreMountains.CorgiEngine
{	
	[CustomEditor(typeof(HealthBar),true)]
	/// <summary>
	/// Custom editor for health bars (mostly a switch for prefab based / drawn bars
	/// </summary>
	public class HealthBarEditor : Editor 
	{
		public HealthBar HealthBarTarget 
		{ 
			get 
			{ 
				return (HealthBar)target;
			}
		} 

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (HealthBarTarget.HealthBarType == HealthBar.HealthBarTypes.Prefab)
			{
				Editor.DrawPropertiesExcluding(serializedObject, new string[] {"Size","ForegroundColor","BackgroundColor","BackgroundPadding" });
			}

			if (HealthBarTarget.HealthBarType == HealthBar.HealthBarTypes.Drawn)
			{
				Editor.DrawPropertiesExcluding(serializedObject, new string[] {"HealthBarPrefab" });
			}

			serializedObject.ApplyModifiedProperties();
		}

	}
}