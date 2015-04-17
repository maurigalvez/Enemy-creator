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
   #region PROPERTIES
   /// ===================
	// EDITOR PROPERTIES
   /// =================== 
   private bool DebugEnabled = false;	     					      // True if Debug Scripts should be added to enemies, false otherwise   
   private bool AIBehaviour = false;                           
   private bool FOV = false;                                   // True if FOV Debug Scripts should be attached to enemies, false otherwise                        
	private GameObject EntityDef;								         // EntityDefinition	
	private bool showDetails = false;                           // True if details of Enemy should be shown, false toherwise
	private EntityData preset = null;                           // Instance of EntityData to be obtain from preset
	private bool presetAdded = false;                           // True if a preset has been given, otherwise its false
   private bool ShowEnemyCustomization = true;                 // True if enemy customization should be folded out, otherwise false
   private bool ShowGroupOverview = true;                      // True if group overview should be folded out, otherwise false
   private bool ShowDebugScripts = true;                       // True if Debug scripts should be shown
   /// ===================
	// ENTITY DATA PROPERTIES
   /// ===================
	private string Name;						    			            // Name of enemy entity
	private int HealthPoints = 0;								         // HealthPoints of enemy entity
   private GameObject Model;									         // New Model for enemy
   /// ===================
	// WEAPON SELECTION
   /// ===================
	private Object[] Weapons;									         // Reference to Weapons Prefabs
	private string[] WeaponList;								         // List of Weapons to choose from
	private int WeaponIndex = 0;								         // Current Weapon Index
   /// ===================
	// ARMOR SELECTION
   /// ===================
	private Object[] Armor;										         // Reference to Armor Prefabs
	private string[] ArmorList;								         // List of Weapons to choose from
	private int ArmorIndex = 0;								         // Current Weapon Index
   /// ===================
   // ENEMIES GROUP PROPERTIES
   /// ===================
   private string GroupName;                                   // Name of group of enemies
   private List<EntityData> Enemies = new List<EntityData>();	// List of Enemies to be deployed
   public GroupData Group;                  					      // Enemy Group
   private FormationData.eFormation Formation;   				   // Formation that enemy group will be distributed in 	
   private Transform DeployPosition = null;
   /// ===================
   // ENEMIES GROUP PROPERTIES
   /// ===================
   private int RectWidth = 0;                                  // Width of rectangle formation
   private int RectHeight = 0;                                 // Height of rectangle formation
   private int Radius = 0;                                     // Radius of circle formation
   // --------------------
   // HEALTH DEBUG VALUES
   // -------------------
   private int BarWidth = 0;                                   // Width of health bar debug
   private int BarHeight = 0;                                  // Height of health bar debug
   private Vector2 Offset = new Vector2();                     // Offset of bar on screen
   private Texture barTexture;                                 // Texture to be used for bar
   //---------------------
   // FOV CTRL VALUES
   //---------------------
   private string TargetName;                                  // Name of target
   private bool showGizmos = false;                            // True if gizmos should be shown , false otherwise.
   private int DetectionRadius = 0;                            // Detection radius
   private int ViewAngle = 0;                                  // View Angle for FOV
   #endregion
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
		EditorWindow window = (EnemyCreator) EditorWindow.GetWindow(typeof(EnemyCreator));
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
      #region LoadPresets      
		GUILayout.Label ("Enemy Creator", EditorStyles.boldLabel);
		// Obtain Model from property field
      if (!presetAdded)
      {
         GUILayout.BeginHorizontal();
         GUILayout.Label("Enemy Def/Preset");
         preset = (EntityData)EditorGUILayout.ObjectField(preset, typeof(EntityData), true);
         GUILayout.EndHorizontal();   
         if (preset)
         {
             // Load lists
             LoadWeapons();
             LoadArmors();
             LoadDefValues();
             showDetails = true;
             presetAdded = true;
         }
      }
      else
      {
         // Replace for button
         if (GUILayout.Button("Assign New Enemy Def/Preset"))
         {            
            preset = null;
            presetAdded = false;
            showDetails = false;
         }
      }
      #endregion
      if (showDetails == true)
      {  
         ShowEnemyCustomization = EditorGUILayout.Foldout(ShowEnemyCustomization, "Enemy Customization");
         if (ShowEnemyCustomization)
         {
            #region Customization
            //--------------
            // Display Entity Data
            //--------------	

            EditorGUI.indentLevel++;
            //--------------
            // ENEMY DATA DETAILS
            //-------------
            GUILayout.Label("Enemy Data Details", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            GUILayout.BeginHorizontal();
            GUILayout.Label("3D Model");
            // Obtain new Model for enemy
            Model = (GameObject)EditorGUILayout.ObjectField(Model, typeof(GameObject), true);
            GUILayout.EndHorizontal();
            // Obtain name
            Name = EditorGUILayout.TextField("Name", Name);
            // Obtain healthpoints
            HealthPoints = EditorGUILayout.IntSlider("Health Points", HealthPoints, 0, 100);
            // Weapon Drop Down
            WeaponIndex = EditorGUILayout.Popup("Weapon", WeaponIndex, WeaponList);
            // Armor Drop Down
            ArmorIndex = EditorGUILayout.Popup("Armor", ArmorIndex, ArmorList);
            EditorGUI.indentLevel--;
            //--------------
            // FOV CONTROL DETAILS
            //-------------
            GUILayout.Label("FOV Control Details", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            // Obtain target name
            TargetName = EditorGUILayout.TextField("Target", TargetName);
            // obtain showGixmos
            showGizmos = EditorGUILayout.Toggle("Show Gizmos", showGizmos);
            // obtain detection radius
            DetectionRadius = EditorGUILayout.IntSlider("Detection Radius", DetectionRadius, 0, 100);
            // obtain view angle
            ViewAngle = EditorGUILayout.IntSlider("View Angle", ViewAngle, 0, 100);
            EditorGUI.indentLevel--;
            //-------------
            // FOV DEBUG DETAILS
            //-------------
            EditorGUI.indentLevel--;
            #endregion
            #region Buttons
            // ------------
            // BUTTONS
            // ------------
            GUILayout.BeginHorizontal();
            // Save Changes
            if (GUILayout.Button("Add To Group"))
            {
               // Add Enemies to List			
               preset.Name = Name;
               preset.MaxHealthPoints = HealthPoints;
               // obtain weapon component
               GameObject weapon = (GameObject)Weapons[WeaponIndex];
               preset.Weapon = weapon.GetComponent<WeaponData>();
               // obtain armor component
               GameObject armor = (GameObject)Armor[ArmorIndex];
               preset.Armor = armor.GetComponent<ArmorData>();
               // Add to list
               Enemies.Add(preset);
               // Display pop up
               Debug.Log(Name + " has been added to enemy group");
            }
            //------------------
            // Save As New Preset
            //------------------
            if (GUILayout.Button("Save As Enemy Prefab"))
            {
               if (!Model)
               {
                  EditorUtility.DisplayDialog("Not Model Provided", "You must provide a 3D model to create an enemy!", "Ok");
                  return;
               }
               // Assign localpath
               string LocalPath = "Assets/Resources/Enemies/" + Name + ".prefab";
               //----------
               // CHECK IF WEAPON ALREADY EXISTS
               //-----------
               // create instance of game object
               GameObject prefab = null;
               // invoke prefab as a gameobject
               prefab = (GameObject)AssetDatabase.LoadAssetAtPath(LocalPath, typeof(GameObject));
               // check if prefab exists
               if (prefab)
               {
                  // If it exists, prompt message
                  if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
                     OverrideEnemy(prefab);
               }
               else
                  // Create new enemy
                  CreateNewEnemy(LocalPath);
            }
            if (GUILayout.Button("Reset Values"))
               LoadDefValues();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndFadeGroup();
         }
               #endregion         
      }     
      //------------
      // ENEMY GROUP
      //------------
      // Check if Number of Enemies is not ZERO
      if (Enemies.Count > 0)
      {
         ShowGroupOverview = EditorGUILayout.Foldout(ShowGroupOverview, "Group Overview");   
         if (ShowGroupOverview)
         { 
            #region GroupOverview
            //-----------
            // DISPLAY ENEMY GROUP
            //----------            
            EditorGUI.indentLevel++;
            GroupName = EditorGUILayout.TextField("Group Name", GroupName);
            for (int i = 0; i < Enemies.Count; i++)
            {
               // Layout
               EditorGUILayout.BeginHorizontal();
               // Enemy Number and name
               EditorGUILayout.LabelField((i + 1).ToString() + ". " + Enemies[i].Name);
               if (GUILayout.Button("Remove From Group"))
               {
                  // Check if Enemies should be removed
                  if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to remove " + Enemies[i].Name + " from group?", "Yes", "No"))
                  {  // remove enemy
                     Enemies.RemoveAt(i);
                     return;
                  }
               }
               EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Delete Group"))
               // empty list
               Enemies.Clear();
            EditorGUI.indentLevel--;
            #endregion
         }         
         
         ShowDebugScripts = EditorGUILayout.Foldout(ShowDebugScripts, "Debug Scripts To Attach");
         if (ShowDebugScripts)
         {
            #region ScriptOptions
            //------------
            // ATTACH SCRIPTS
            //------------
            EditorGUI.indentLevel++;
            //------------
            // HEALTH SCRIPT
            //------------
            DebugEnabled = EditorGUILayout.Toggle("Attach Health Debug Script", DebugEnabled);
            if (DebugEnabled)
            {
               EditorGUI.indentLevel++;
               // height
               BarHeight = EditorGUILayout.IntField("Bar Height", BarHeight);
               // width
               BarWidth = EditorGUILayout.IntField("Bar Width", BarWidth);
               Offset = EditorGUILayout.Vector2Field("Screen offset", Offset);
               // texture
               EditorGUILayout.BeginHorizontal();
               GUILayout.Label("Texture", EditorStyles.label);
               barTexture = (Texture)EditorGUILayout.ObjectField(barTexture, typeof(Texture), true);
               EditorGUILayout.EndHorizontal();
               EditorGUI.indentLevel--;
            }
            //-----------
            // FOV SCRIPT
            //-----------
            //FOV = EditorGUILayout.Toggle("Attach FOV", FOV);
            //AIBehaviour = EditorGUILayout.Toggle("Attach AI Behaviour", AIBehaviour);        
            EditorGUI.indentLevel--;
            #endregion
         }
         #region Deployment
            //------------
            // DEPLOY GROUP
            //------------
            GUILayout.Label("Deployment Details", EditorStyles.boldLabel);
            // start group details
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();         
            GUILayout.Label("Deploy Position", EditorStyles.label);        
            // Click button to obtain position for deployment
            DeployPosition = (Transform)EditorGUILayout.ObjectField(DeployPosition, typeof(Transform), true);        
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;        
            //---------------
            // Display Formation
            //---------------
            GUILayout.Label("Formation Details", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;        
            Formation = (FormationData.eFormation)EditorGUILayout.EnumPopup("Initial Formation", Formation);
            if(Formation == FormationData.eFormation.CIRCLE)         
               Radius = EditorGUILayout.IntField("Radius", Radius);         
            else
            {
               EditorGUILayout.BeginHorizontal();
               RectWidth = EditorGUILayout.IntField("Width", RectWidth);
               RectHeight = EditorGUILayout.IntField("Height", RectHeight);
               EditorGUILayout.EndHorizontal();
            }
            // Check if Deploy group button has been pressed
            if (GUILayout.Button("Deploy Group"))
            {
               if(!DeployPosition)
               {
                  EditorUtility.DisplayDialog("No Deploy Position", "You must provide assign a position where group should deployed!", "Ok");
                  return;
               }  
               if(DebugEnabled)
               {
                  if(!barTexture)
                  {
                     EditorUtility.DisplayDialog("No Texture Found", "You must provide a texture for Health Debug script!", "Ok");
                     return;
                  }
               }
               // Create Group Parent object
               GameObject instance = new GameObject(GroupName);
               // set instance to deploy position
               instance.transform.position = DeployPosition.position;
               // Add EnemyGroup isntance
               Group = instance.AddComponent<GroupData>();
               // Add Members
               Group.Members = Enemies;
               // check if Circle formation was selected
               if (Formation == FormationData.eFormation.CIRCLE)
                  CircleFormation(DeployPosition.position,instance.transform);
               else
                  RectangleFormation(DeployPosition.position,instance.transform);
               // Display pop up
               EditorUtility.DisplayDialog("Success", "Enemy Group has been deployed", "Ok");
            }
            EditorGUI.indentLevel--;  
   #endregion    
      }// ENEMY GROUP		
	}
   #region FORMATIONS
   /// ====================
   /// CIRCLE FORMATION
   /// <summary>
   /// Instantiate enemies in a circle formation
   /// </summary>
   /// <param name="center">Position of group</param>
   /// ====================
   private void CircleFormation(Vector3 center, Transform parent)
   {
      int n = Enemies.Count;
      // iterate over enemies
      foreach(EntityData e in Enemies)
      {         
			float theta = Random.Range(1, 2*Mathf.PI);
			// calculate r
         float r = Radius * Random.Range(0.0f,1.0f);
         //float pdf_r = (2 / (Radius * Radius)) * r;
			// calculate position x and y
			float x = center.x + r * Mathf.Cos (theta);
         float z = center.z + r * Mathf.Sin(theta);
         // create vector3 position
         Vector3 position = new Vector3(x, 1, z);
         // Instantiate
         GameObject enemy = (GameObject)GameObject.Instantiate(e.gameObject, position, Quaternion.identity);
         // set parent
         enemy.transform.SetParent(parent);        
         // set group
         e.SetSquad(Group);
         // attach scripts
         AttachScripts(enemy);
      }
   }
   /// ====================
   /// RECTANGLE FORMATION
   /// <summary>
   /// Instantiate enemies in a rectangular formation
   /// </summary>
   /// <param name="center">Position of group</param>
   /// ====================
   private void RectangleFormation(Vector3 center, Transform parent)
   {
      int n = Enemies.Count;
      // obtain value of half h
      float hWidth = RectWidth * 0.5f;
      float hHeight = RectHeight * 0.5f;
      // iterate over enemies
      foreach(EntityData e in Enemies)
      {         
         float x = center.x  + Random.Range(-hWidth, hWidth);
         float z = center.z + Random.Range(-hHeight,hHeight);
         Vector3 position = new Vector3(x, 1, z);
         // Instantiate
         GameObject enemy = (GameObject) GameObject.Instantiate(e.gameObject, position, Quaternion.identity);
         // set parent
         enemy.transform.SetParent(parent);
         // set group
         e.SetSquad(Group);
         // attach scripts
         AttachScripts(enemy);		
      }
   }
   /// ====================
   /// ATTACH SCRIPTS
   /// <summary>
   /// Attaches scripts to given object
   /// </summary>
   /// <param name="obj"></param>
   /// ====================
   private void AttachScripts(GameObject enemy)
   {
      //------------
      // ATTACH SCRIPTS
      //-----------
      // attach entity state data
      enemy.AddComponent<EntityStateData>();
      // Add Enemy script
      IsEnemy edata = enemy.GetComponent<IsEnemy>();
      if (!edata)
         enemy.AddComponent<IsEnemy>();
      // Add FOV script
      FOV_Ctrl fovctrl = enemy.GetComponent<FOV_Ctrl>();
      if (!fovctrl)
         fovctrl = enemy.AddComponent<FOV_Ctrl>();
      // Assign fovscrpit properties
      fovctrl.target = GameObject.Find(TargetName);
      fovctrl.showGizmos = showGizmos;
      fovctrl.detectionRadius = DetectionRadius;
      fovctrl.viewAngle = ViewAngle;
      //------------
      // ATTACH DEBUG SCRIPTS
      //-----------
      if (DebugEnabled)
      {
         // attach debug script
         EntityHealthDebug h = enemy.AddComponent<EntityHealthDebug>();
         // assign values
         h.HealthBarHeight = BarHeight;
         h.HealthBarWidth = BarWidth;
         h.scrollbar = barTexture;
         h.Offset = Offset;
      }
   }
   #endregion
   #region PrefabScripts
   /// ================
   /// <summary>
   /// Overrides given Prefab 
   /// </summary>
   /// ================ 
	private void OverrideEnemy(GameObject prefab)
	{
      // obtain Entity Data
      EntityData data = prefab.GetComponent<EntityData>();
      // check if data was found
      if(data)
      {
         // override values
         data.Name = Name;
         data.MaxHealthPoints = HealthPoints;
         // obtain armor values
         GameObject armor = (GameObject) Armor[ArmorIndex];
         data.Armor = armor.GetComponent<ArmorData>();
         // obtain weapon values
         GameObject weapon = (GameObject)Weapons[WeaponIndex];
         data.Weapon = weapon.GetComponent<WeaponData>();
      }
      // display message
      EditorUtility.DisplayDialog("Success", data.Name + " was overriden.", "Ok");
	}
   /// ================
   /// <summary>
   /// Creates A new Enemy prefab at given path
   /// </summary>
   /// <param name="path"></param>
   /// ================
   private void CreateNewEnemy(string path)
   {
	  // obtain entity data
	  EntityData data = Model.GetComponent<EntityData>();
      // check if model didnt have an Entity Data
	  if(!data)
		 // add entity data to model
      	 data = Model.AddComponent<EntityData>();
      // Populate data
      data.Name = Name;
      data.MaxHealthPoints = HealthPoints;
      // obtain armor values
      GameObject armor = (GameObject)Armor[ArmorIndex];
      data.Armor = armor.GetComponent<ArmorData>();
      // obtain weapon values
      GameObject weapon = (GameObject)Weapons[WeaponIndex];
      data.Weapon = weapon.GetComponent<WeaponData>();
      // Create an Empty Prefab
      Object prefab = PrefabUtility.CreatePrefab(path, Model);	
      // display message
      EditorUtility.DisplayDialog("Success", data.Name + " was created!", "Ok");
   }
   #endregion
   # region LOADING
   /// ================
	/// LOAD DEF VALUES
	/// <summary>
	/// Loads the def values.
	/// </summary>
	/// ================
	private void LoadDefValues()
	{
		// set values from preset
		Name = preset.Name;
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
   #endregion
}
