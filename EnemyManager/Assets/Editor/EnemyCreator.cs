using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;
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
   private int NumberOfEnemies = 0;								// Total Number of Enemies
   private Object[] Presets;	 								// List of all preset enemies
   private string[] PresetList;									// List to be added to editor
   private bool DebugEnabled = false;	     					// True if Debug Scripts should be added to enemies, false otherwise
   private EnemyGroup Group;                  					// Enemy Group
   public FormationData.eFormation Formation;   				// Formation that enemy group will be distributed in 	
	//---------------------
	// ENTITY DATA PROPERTIES
	//----------------------
	private string Name;						    			// Name of enemy entity
	private int HealthPoints;									// HealthPoints of enemy entity
	//private WeaponData Weapon;									// Instance of Weapon Data
	//private ArmorData Armor;									// Instance of Armor Data
	AnimBool ShowCustomFields;								    // True if custom fields should be visible, false otherwise
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
	[MenuItem ("Window/Enemy Creator")]
	static void Init()
	{
		//-----------
		// Initialize window
		//-----------
		EnemyCreator window = (EnemyCreator) EditorWindow.GetWindow(typeof(EnemyCreator));
		window.Show ();

	}

	void OnEnable(){
		ShowCustomFields = new AnimBool(false);
		ShowCustomFields.valueChanged.AddListener(Repaint);
	}

	/// ====================
	/// ON GUI
	/// <summary>
	/// Updates window
	/// </summary>
	/// ====================
	public void OnGUI()
	{	
		// Obtain number of Enemies
		NumberOfEnemies = EditorGUILayout.IntSlider ("Number Of Enemies",NumberOfEnemies,0,20);
		//--------------
		// Display Entity Data
		//--------------
		GUILayout.Label ("Enemy Presets", EditorStyles.boldLabel);
		ShowCustomFields.target = EditorGUILayout.ToggleLeft("Customized Selected Preset", ShowCustomFields.target);		
		// -------------
		// Extra block that can be toggled on and off.
		// -------------
		if (EditorGUILayout.BeginFadeGroup(ShowCustomFields.faded))
		{
			// Load lists
			LoadWeapons();
			LoadArmors ();
			// Obtain name
			Name = EditorGUILayout.TextField("Name",Name);
			// Obtain healthpoints
			HealthPoints = EditorGUILayout.IntSlider("Health Points", HealthPoints, 0,100);		
			// Weapon Drop Down
			WeaponIndex = EditorGUILayout.Popup("Weapon",WeaponIndex,WeaponList);
			// Armor Drop Down
			ArmorIndex = EditorGUILayout.Popup("Armor",ArmorIndex,ArmorList);
			// ------------
			// SAVE BUTTONS
			// ------------
			GUILayout.BeginHorizontal();
			// Save Changes
			if(GUILayout.Button("Save Changes"))
			{
				// Create as New Preset
			}
			// Save As New Preset
			if(GUILayout.Button("Save As New Preset"))
			{
				// Create as New Preset
			}
			GUILayout.EndHorizontal();
		}
		EditorGUILayout.EndFadeGroup();

		GUILayout.Label ("Deploy Enemies", EditorStyles.boldLabel);
		// Check if Debug Scripts should be attached
		DebugEnabled = EditorGUILayout.Toggle("Attach Debug Scripts", DebugEnabled);
		// Display Formation
		Formation = (FormationData.eFormation) EditorGUILayout.EnumPopup("Initial Formation", Formation);
	}
	private void SaveAsNewPreset()
	{

	}

	/// ================
	/// LOAD PRESETS
	/// <summary>
	/// Loads Enemy Presets
	/// </summary>
	/// ================
	private void LoadPresets()
	{
		// Obtain enemy presets
		Presets = Resources.LoadAll ("Enemies",typeof(GameObject));
		// Initialize presetList
		PresetList = new string[Presets.Length];
		int e = 0;
		// Add enemies to PresetList
		for(int i = 0; i < Presets.Length; i++)
		{
			GameObject enemy = (GameObject)Presets[i];
			// Check if its an enemy
			if(enemy.GetComponent<EntityData>())
				// Add to PresetList
				PresetList[i++] = enemy.GetComponent<EntityData>().Name;
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
