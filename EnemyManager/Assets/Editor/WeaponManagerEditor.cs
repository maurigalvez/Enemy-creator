using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 28/03/15
/// Last Edited: 28/03/15
/// Weapon manager editor - Custom Editor for Weapon Manager
/// </summary>
[CustomEditor(typeof(WeaponManager))]
public class WeaponManagerEditor: Editor 
{
	SerializedProperty DamageProp;		// Damage serialized property
	SerializedProperty NameProp;		// Name serialized property
	SerializedProperty ModelProp;		// Model serialized property
	SerializedProperty Weapons;			// Weapons serialized property
	WeaponManager myTarget;				// Instance of WeaponManager
	/// ====================
	/// ON ENABLE
	/// <summary>
	/// Initializes properties
	/// </summary>
	/// ====================
	public void OnEnable()
	{
		Weapons = serializedObject.FindProperty("Weapons");
		NameProp = serializedObject.FindProperty("name");
		DamageProp = serializedObject.FindProperty("damage");
		ModelProp = serializedObject.FindProperty("model");

	}
	/// ====================
	/// ON INSPECTOR GUI
	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	/// ====================
	public override void OnInspectorGUI()
	{
		myTarget = (WeaponManager)target;

		serializedObject.Update();
		EditorGUILayout.PropertyField(Weapons, new GUIContent("Weapons Preset"));
		// Name
		EditorGUILayout.PropertyField(NameProp,new GUIContent("Weapon Model"));
		// Damage
		EditorGUILayout.IntSlider(DamageProp,0,100, new GUIContent("Damage"));
		if (!DamageProp.hasMultipleDifferentValues)
			ProgressBar (DamageProp.intValue / 100.0f, "Damage");
		// 3D model
		EditorGUILayout.PropertyField(ModelProp,new GUIContent("Weapon Model"));
		// Apply changes to the serialized Property
		serializedObject.ApplyModifiedProperties ();
		//Debug.Log (DamageProp.intValue);
		if(GUILayout.Button("Create Weapon"))
		{
			// Obtain values
			string name = NameProp.stringValue;
			int damage =  DamageProp.intValue;
			GameObject model = ModelProp.objectReferenceValue as GameObject;
			// Assign localpath
			string LocalPath = "Assets/Weapons/"+ name + ".prefab";
			//----------
			// CHECK IF WEAPON ALREADY EXISTS
			//-----------
			// create instance of game object
			GameObject prefab = null;
			// invoke prefab as a gameobject
			prefab = (GameObject) AssetDatabase.LoadAssetAtPath(LocalPath, typeof(GameObject));
			// check if prefab exists
			if (prefab) 
			{
				// If it exists, prompt message
				if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes","No"))
					OverrideWeapon (prefab, name, damage, model);
			}
			else
				// Create new weapon
				CreateNewWeapon(LocalPath,name,damage,model);
		}
	}
	/// ====================
	/// CREATE NEW WEAPON
	/// <summary>
	/// Creates the new weapon.
	/// </summary>
	/// <param name="path">Path. Path where prefab will be stored</param>
	/// <param name="name">Name. Name of weapon</param>
	/// <param name="damage">Damage. Damage of weapon</param>
	/// <param name="model">Model. GameObject of weapon</param>
	/// ====================
	void CreateNewWeapon(string path, string name, int damage, GameObject model)
	{	
		// Add WeaponData to gameObject		/
		WeaponData weapon =	model.AddComponent<WeaponData>();	
		weapon.Name = name;
		weapon.DamagePoints = damage;
		// Create an Empty Prefab
		Object prefab = PrefabUtility.CreatePrefab(path,model);	
	}
	/// ====================
	/// OVERRIDE WEAPON
	/// <summary>
	/// Overrides the weapon.
	/// </summary>
	/// <param name="prefab">Prefab. Existent prefab of weapon</param>
	/// <param name="name">Name. New name of weapon</param>
	/// <param name="damage">Damage. New Damage value</param>
	/// <param name="model">Model. New Model</param>
	/// ====================
	void OverrideWeapon(GameObject prefab, string name, int damage, GameObject model)
	{
		WeaponData weapon = prefab.GetComponent<WeaponData>();
		if(weapon)
		{
			weapon.Name = name;
			weapon.DamagePoints = damage;
		}
	}
	// ====================
	//Custom GUILayout progress bar.
	// ====================
	void ProgressBar (float value, string label)
	{
		// Get a rect for the progress bar using the same margins as a textfield:
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space ();
	}
}
