using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[UnityEditor.CustomEditor(typeof(UISettingsMenu), true)]
//public class UISettingsEditor : Editor{
//	static float starting_volume = -25f;
//	SerializedProperty volume_prop;

//	void OnEnable()
//	{
//		Debug.Log("Hello from editor");
//	}

//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//		base.OnInspectorGUI();
//		GUI.enabled = false;
//		starting_volume = EditorGUILayout.Slider(starting_volume, -80, 0);
//		GUI.enabled = true;
//	}
//}