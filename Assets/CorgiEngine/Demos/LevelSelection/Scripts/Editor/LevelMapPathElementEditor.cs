using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MoreMountains.CorgiEngine
{
	/// <summary>
	/// This class adds names for each LevelMapPathElement next to it on the scene view, for easier setup
	/// </summary>
	[CustomEditor(typeof(LevelMapPathElement))]
	[InitializeOnLoad]
	public class LevelMapPathElementEditor : Editor 
	{		
		protected LevelMapPathElement _levelMapPathElement;

		[DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
		static void DrawGameObjectName(LevelMapPathElement pathElement, GizmoType gizmoType)
		{   
			GUIStyle style = new GUIStyle();
	        style.normal.textColor = Color.blue;	 
			Handles.Label(pathElement.transform.position+(Vector3.down*0.1f)+(Vector3.left*0.1f), pathElement.gameObject.name,style);
		}


	}
}