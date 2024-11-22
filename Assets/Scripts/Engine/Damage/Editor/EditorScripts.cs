using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OnDamageEffect))]
public class OnDamageEffectEditor: Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		OnDamageEffect script = (OnDamageEffect)target;
		
		script.PlayAudio = EditorGUILayout.Toggle("Play Audio", script.PlayAudio);
		if(script.PlayAudio)
		{
			script.SFX = EditorGUILayout.ObjectField("", script.SFX, typeof(SFXList), true) as SFXList;
		}
	}
}
