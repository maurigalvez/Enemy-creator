using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 24/03/15
/// Last Edited: 24/03/15
/// Enemy Creator - Class used to Create a Group of Enemies
/// </summary>
public class EnemyCreator : EditorWindow
{
	//----------------------
	// EDITOR PROPERTIES
	//----------------------   
	private List<EntityData> Enemies = new List<EntityData>();	// List of Enemies to be deployed
	private string[] EnemyGroup;
    private bool DebugEnabled = false;	     					// True if Debug Scripts should be added to enemies, false otherwise
    private EnemyGroup Group;                  					// Enemy Group
    private FormationData.eFormation Formation;   				// Formation that enemy group will be distributed in 	
	private GameObject EntityDef;								// EntityDefinition
	private GameObject Model;									// New Model for enemy
	private bool showDetails = false;
	private EntityData preset = null;
	private bool presetAdded = false;
	//---------------------
	// ENTITY DATA PROPERTIES
	//----------------------
	private string Name;						    			// Name of enemy entity
	private int HealthPoints;									// HealthPoints of enemy entity
	//--------------
	// WEAPON SELECTION
	//--------------
	private Object[] Weapons;									// Reference to Weapons Prefabs
	private string[] WeaponList;								// List of Weapons to choose from
	private int WeaponIndex = 0;								// Current Weapon Index
	//--------------
	// ARMOR SELECTION
	//--------------
	private Object[] Armor;										// Reference to Armor Prefabs
	private string[] ArmorList;									// List of Weapons to choose from
	private int ArmorIndex = 0;									// Current Weapon Index
	/// ===================
	/// INIT
	/// <summary>
	/// Init this instance.
	/// </summary>
	/// ===================
   [MenuItem("Create/New Enemy... %#e")]
	static void Init()
	{
		//-----------
		// Initialize window
		//-----------
		EnemyCreator window = (EnemyCreator) EditorWindow.GetWindow(typeof(EnemyCreator));
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
		// Load lists
		LoadWeapons();
		LoadArmors ();
		GUILayout.Label ("Enemy Def", EditorStyles.boldLabel);
		// Obtain Model from property field
		EntityDef = (GameObject) EditorGUILayout.ObjectField(EntityDef, typeof(GameObject),true);
		if(EntityDef && preset && EntityDef.name != preset.gameObject.name)
			presetAdded = false;
		if(!presetAdded && EntityDef)
		{
			preset = EntityDef.GetComponent<EntityData>();
			if(preset)
			{
				LoadDefValues();
				showDetails = true;
				presetAdded = true;		
			}
			else
			{
				presetAdded = false;	
				showDetails = false;
			}
		}
		//--------------
		// Display Entity Data
		//--------------	
		if (showDetails == true)
		{
			GUILayout.Label ("Enemy Customization", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();
			GUILayout.Label ("3D Model");
			// Obtain new Model for enemy
			Model = (GameObject) EditorGUILayout.ObjectField(Model, typeof(GameObject),true);
			GUILayout.EndHorizontal();
			// Obtain name
			Name = EditorGUILayout.TextField("Name",Name);
			// Obtain healthpoints
			HealthPoints = EditorGUILayout.IntSlider("Health Points", HealthPoints, 0,100);		
			// Weapon Drop Down
			WeaponIndex = EditorGUILayout.Popup("Weapon",WeaponIndex,WeaponList);
			// Armor Drop Down
			ArmorIndex = EditorGUILayout.Popup("Armor",ArmorIndex,ArmorList);
			// ------------
			// BUTTONS
			// ------------
			GUILayout.BeginHorizontal();	
			// Save Changes
			if(GUILayout.Button("Add To Group"))
			{
				// Add Enemies to List			
				preset.name = Name;
				preset.MaxHealthPoints = HealthPoints;
				// obtain weapon component
				GameObject weapon = (GameObject) Weapons[WeaponIndex];
				preset.Weapon = weapon.GetComponent<WeaponData>();
				// obtain armor component
				GameObject armor = (GameObject) Armor[ArmorIndex];
				preset.Armor = armor.GetComponent<ArmorData>();
				// Add to list
				Enemies.Add(preset);			
				// Display pop up
				EditorUtility.DisplayDialog("Enemy Creator", "Enemy Has been Added To Group", "Ok");
			}
			// Save As New Preset
			if(GUILayout.Button("Save As New Preset"))
			{
				// Create as New Preset
				EditorUtility.DisplayDialog("Enemy Creator", "New Enemy Preset has been saved!", "Ok");
			}
			if(GUILayout.Button ("Reset Values"))			
				LoadDefValues ();
			GUILayout.EndHorizontal();
		}
		EditorGUILayout.EndFadeGroup();
		//------------
		// ENEMY GROUP
		//------------
		// Check if Number of Enemies is not ZERO
		if(Enemies.Count > 0)
		{
			// Display enemy group
			GUILayout.Label("Group Overview", EditorStyles.boldLabel);
		
		}
		//------------
		// DEPLOY GROUP
		//------------
		GUILayout.Label ("Deploy Group", EditorStyles.boldLabel);
		// Check if Debug Scripts should be attached
		DebugEnabled = EditorGUILayout.Toggle("Attach Debug Scripts", DebugEnabled);
		// Display Formation
		Formation = (FormationData.eFormation) EditorGUILayout.EnumPopup("Initial Formation", Formation);
		if(GUILayout.Button("Deploy Group"))
		{
			// Display pop up
			EditorUtility.DisplayDialog("Enemy Creator", "Enemy Group has been deployed", "Ok");
		}
	}
	/// ================
	/// SAVE AS NEW PRESET
	/// ================
	private void SaveAsNewPreset()
	{

	}
	/// ================
	/// LOAD DEF VALUES
	/// <summary>
	/// Loads the def values.
	/// </summary>
	/// ================
	private void LoadDefValues()
	{
		// set values from preset
		Name = preset.name;
		HealthPoints = (int) preset.MaxHealthPoints;
		//-----------
		// set weapon Index
		//------------
		for(int w = 0; w < WeaponList.Length; w++)
		{
			if(preset.Weapon && preset.Weapon.name == WeaponList[w])
				// set weapon index
				WeaponIndex = w;
		}
		//----------
		// set armor index
		//----------
		for(int a = 0; a < ArmorList.Length; a++)
		{
			if(preset.Armor && preset.Armor.name == ArmorList[a])
				// set armor index
				ArmorIndex = a;
		}
	}
	/// ================
	/// LOAD WEAPONS
	/// <summary>
	/// Loads Weapons presets to be added to the list
	/// </summary>
	///  ================
	private void LoadWeapons()
	{
		// Obtain all assets at weapons path
		Weapons = Resources.LoadAll ("Weapons", typeof(GameObject));
		//Debug.Log (Weapons.Length);
		// Initialize weaponsList
		WeaponList = new string[Weapons.Length];
		int w = 0;
		// Add weapons to WeaponList
		for(int i = 0; i < Weapons.Length; i++)
		{
			GameObject weapon = (GameObject) Weapons[i];
			// Check if its a weapon
			if(weapon.GetComponent<WeaponData>())
				// Add to WeaponsList
				WeaponList[w++] = weapon.GetComponent<WeaponData>().Name;
		}
	}
	/// ================
	/// LOAD ARMORS
	/// <summary>
	/// Loads Armor presets to be added to list
	/// </summary>
	/// ================
	private void LoadArmors()
	{
		Armor = Resources.LoadAll ("Armor",typeof(GameObject));
		// Initialize ArmorList
		ArmorList = new string[Armor.Length];
		int a = 0;
		// Add elements to array
		for(int i = 0; i < Armor.Length; i++)
		{
			GameObject armor = (GameObject) Armor[i];
			// Check if its armor
			if(armor.GetComponent<ArmorData>())
				ArmorList[a++] = armor.GetComponent<ArmorData>().Name;
		}
	}
}
