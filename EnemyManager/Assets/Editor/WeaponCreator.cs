using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 28/03/15
/// Last Edited: 30/03/15
/// Weapon Creator - Editor Window used to create Weapons
/// </summary>
public class WeaponCreator: EditorWindow
{
	private int Damage;						// Damage Value
	private string Name;					// Name Value
	private GameObject Model;				// 3D Model Value
	/// ===================
	/// INIT
	/// <summary>
	/// Init this instance.
	/// </summary>
	/// ===================
	[MenuItem ("Window/Weapon Creator")]
	static void Init()
	{
		WeaponCreator window = (WeaponCreator) EditorWindow.GetWindow(typeof(WeaponCreator));
		window.Show ();
	}
	/// ====================
	/// ON GUI
	/// <summary>
	/// Updates window
	/// </summary>
	/// ====================
	public void OnGUI()
	{
		GUILayout.Label ("Create New Weapon", EditorStyles.boldLabel);
		// Obtain Name from textfield
		Name= EditorGUILayout.TextField("Name", Name);
		// Obtain damage from int slider
		Damage = EditorGUILayout.IntSlider ("Damage",Damage,0,100);
		ProgressBar (Damage/ 100.0f, "Damage");
		// Obtain Model from property field
		Model = (GameObject) EditorGUILayout.ObjectField(Model, typeof(GameObject),true);
		if(GUILayout.Button("Create Weapon"))
		{
			// Assign localpath
			string LocalPath = "Assets/Resources/Weapons/"+ Name + ".prefab";
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
					OverrideWeapon (prefab, Name, Damage, Model);
			}
			else
				// Create new weapon
				CreateNewWeapon(LocalPath,Name,Damage,Model);
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
		if(prefab != model)
			prefab = model;
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
