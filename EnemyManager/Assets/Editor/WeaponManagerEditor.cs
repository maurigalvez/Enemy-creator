using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Weapon manager editor - Custom Editor for Weapon Manager
/// </summary>
[CustomEditor(typeof(WeaponManager))]
public class WeaponManagerEditor: Editor 
{
	public string[] Presets;			// List of created weapons
	SerializedProperty damage;			// Damage serialized property
	public void OnEnable()
	{

	}
	public override void OnInspectorGUI()
	{
		WeaponManager myTarget = (WeaponManager)target;

		if(GUILayout.Button("Create Weapon"))
		{
			Debug.Log("Weapon Created");
		}
	}

}
