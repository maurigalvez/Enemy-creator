using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 30/03/15
/// Last Edited: 30/03/15
/// Armor Creator - Editor Window used to create Armor
/// </summary>
public class ArmorCreator : EditorWindow
{
	public string Name;				// Name value
	public int Defense;				// Defense points value
	public GameObject Model;		// Armor 3D Model
	/// ===================
	/// INIT
	/// <summary>
	/// Init this instance.
	/// </summary>
	/// ===================
   [MenuItem("Create/New Armor... %#a")]
	static void Init()
	{
		ArmorCreator window = (ArmorCreator) EditorWindow.GetWindow(typeof(ArmorCreator));
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
		GUILayout.Label ("Create New Armor", EditorStyles.boldLabel);
		// Obtain Name from textfield
		Name= EditorGUILayout.TextField("Name", Name);
		// Obtain damage from int slider
		Defense = EditorGUILayout.IntSlider ("Defense",Defense,0,100);
		ProgressBar (Defense/ 100.0f, "Defense");
		// Obtain Model from property field
		Model = (GameObject) EditorGUILayout.ObjectField(Model, typeof(GameObject),true);
		if(GUILayout.Button("Create Armor"))
		{
			// Assign localpath
			string LocalPath = "Assets/Resources/Armor/"+ Name + ".prefab";
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
					OverrideArmor (prefab);
			}
			else
				// Create new weapon
				CreateNewArmor(LocalPath);
		}
	}
	/// ====================
	/// CREATE NEW ARMOR
	/// <summary>
	/// Creates the new armor.
	/// </summary>
	/// <param name="path">Path. Path where prefab will be stored</param>
	/// <param name="name">Name. Name of armor</param>
	/// <param name="defense">defense. defense of armor</param>
	/// <param name="model">Model. GameObject of armor</param>
	/// ====================
	void CreateNewArmor(string path)
	{	
		// Add ArmorData to gameObject		/
		ArmorData armor =	Model.AddComponent<ArmorData>();	
		armor.Name = Name;
		armor.DefensePoints = Defense;
		// Create an Empty Prefab
		Object prefab = PrefabUtility.CreatePrefab(path,Model);	
	}
	/// ====================
	/// OVERRIDE armor
	/// <summary>
	/// Overrides the armor.
	/// </summary>
	/// <param name="prefab">Prefab. Existent prefab of armor</param>
	/// <param name="name">Name. New name of armor</param>
	/// <param name="defense">defense. New defense value</param>
	/// <param name="model">Model. New Model</param>
	/// ====================
	void OverrideArmor(GameObject prefab)
	{
		ArmorData armor = prefab.GetComponent<ArmorData>();
		if(armor)
		{
			armor.Name = Name;
			armor.DefensePoints = Defense;
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
