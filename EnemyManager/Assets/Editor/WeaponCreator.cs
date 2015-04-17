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
   /// ===================
   // WEAPON PROPERTIES
   /// ===================
   private int Damage;					      // Damage Value
	private string Name;					      // Name Value
   private int Accuracy;                  // Accuracy Value
   public int FireRate;                   // Weapon's Fire Rate
   public int BulletCount;                // Amount of bullets weapons fires per shot
   private int Spread;                    // Spread Value
   private int Ammo;                      // Ammo value
   private int Noise;                     // Noise Value
   private int Fade;                      // Fade Value
	private GameObject Model;			      // 3D Model Value
   private bool showMaxValues = false;    // True if max values should be shown, false otherwise
   /// ===================
   /// MAX VALUE PROPERTIES
   /// ===================
   private float MaxDamage = 200;
   private float MaxFireRate = 10;
   private float MaxBulletCount = 3;
   private float MaxAmmo = 100;
   private float MaxAccuracy = 100;
   private float MaxSpread = 60;
   private float MaxNoise = 5;
   private float MaxFade = 3;
	/// ===================
	/// INIT
	/// <summary>
	/// Init this instance.
	/// </summary>
	/// ===================
   [MenuItem("Create/New Weapon... %#w")]
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
      //----------
      // Create foldout for max values
      //---------
      showMaxValues = EditorGUILayout.Foldout(showMaxValues, "Edit Properties Values");
      if(showMaxValues)
      {
         MaxAmmo = EditorGUILayout.FloatField("Max Ammo", MaxAmmo);
         MaxFireRate = EditorGUILayout.FloatField("Max Fire Rate", MaxFireRate);
         MaxBulletCount = EditorGUILayout.FloatField("Max Bullet Count", MaxBulletCount);
         MaxDamage = EditorGUILayout.FloatField("Max Damage", MaxDamage);
         MaxAccuracy = EditorGUILayout.FloatField("Max Accuracy", MaxAccuracy);
         MaxSpread = EditorGUILayout.FloatField("Max Spread", MaxSpread);
         MaxNoise = EditorGUILayout.FloatField("Max Noise", MaxNoise);
         MaxFade = EditorGUILayout.FloatField("Max Fade", MaxFade);
      }
      //----------
      // Create Weapon
      //----------
      // Obtain Model from property field
      GUILayout.BeginHorizontal();
      GUILayout.Label("3D Model");
      Model = (GameObject)EditorGUILayout.ObjectField(Model, typeof(GameObject), true);
      GUILayout.EndHorizontal();
		// Obtain Name from textfield
		Name= EditorGUILayout.TextField("Name", Name);
      // Obtain Ammo from int slider
      Ammo = EditorGUILayout.IntSlider("Ammo", Ammo, 0, (int)MaxAmmo);
      ProgressBar(Ammo / MaxAmmo, "Ammo");
		// Obtain damage from int slider
		Damage = EditorGUILayout.IntSlider ("Damage",Damage,0,(int)MaxDamage);
		ProgressBar (Damage/ MaxDamage, "Damage");
      // Obtain Fire Rate
      FireRate = EditorGUILayout.IntSlider("Fire Rate", FireRate, 0, (int)MaxFireRate);
      ProgressBar(FireRate / MaxFireRate, "Fire Rate");
      // Obtain bullet count
      BulletCount = EditorGUILayout.IntSlider("Bullet Count", BulletCount, 0, (int)MaxBulletCount);
      ProgressBar(BulletCount / MaxBulletCount, "Bullet Count");
      // Obtain Accuracy from int slider
      Accuracy = EditorGUILayout.IntSlider("Accuracy", Accuracy, 0, (int)MaxAccuracy);
      ProgressBar(Accuracy / MaxAccuracy, "Accuracy");
      // Obtain Noise from int slider
      Noise = EditorGUILayout.IntSlider("Noise", Noise, 0, (int)MaxNoise);
      ProgressBar(Noise / MaxNoise, "Noise");
      // Obtain Fade from int slider
      Fade = EditorGUILayout.IntSlider("Fade", Fade, 0, (int)MaxFade);
      ProgressBar(Fade / MaxFade, "Fade");
      // Obtain Spread from int slider
      Spread = EditorGUILayout.IntSlider("Spread", Spread, 0, (int)MaxSpread);
      ProgressBar(Spread / MaxSpread, "Spread");	
		if(GUILayout.Button("Create Weapon"))
		{
         if (!Model)
         {
            EditorUtility.DisplayDialog("Not Model Provided", "You must provide a 3D model to create a weapon!", "Ok");
            return;
         }
			// Assign localpath
			string LocalPath = "Assets/Resources/Weapons/"+ Name + ".prefab";
			//-------------
			// CHECK IF WEAPON ALREADY EXISTS
			//-------------
			// create instance of game object
			GameObject prefab = null;
			// invoke prefab as a gameobject
			prefab = (GameObject) AssetDatabase.LoadAssetAtPath(LocalPath, typeof(GameObject));
			// check if prefab exists
			if (prefab) 
			{
				// If it exists, prompt message
				if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes","No"))
					OverrideWeapon (prefab);
			}
			else
				// Create new weapon
				CreateNewWeapon(LocalPath);
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
	void CreateNewWeapon(string path)
	{	
		// Add WeaponData to gameObject		
		WeaponData weapon =	Model.AddComponent<WeaponData>();	
		weapon.Name = Name;
		weapon.DamagePoints = Damage;
      weapon.Ammo = Ammo;
      weapon.Accuracy = Accuracy;
      weapon.Spread = Spread;
      weapon.Noise = Noise;
      weapon.Fade = Fade;
		// Create an Empty Prefab
		Object prefab = PrefabUtility.CreatePrefab(path,Model);	
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
	void OverrideWeapon(GameObject prefab)
	{
		if(prefab != Model)
			prefab = Model;
		WeaponData weapon = prefab.GetComponent<WeaponData>();
		if(weapon)
		{
			weapon.Name = Name;
			weapon.DamagePoints = Damage;
         weapon.Ammo = Ammo;
         weapon.Accuracy = Accuracy;
         weapon.Spread = Spread;
         weapon.Noise = Noise;
         weapon.Fade = Fade;
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
