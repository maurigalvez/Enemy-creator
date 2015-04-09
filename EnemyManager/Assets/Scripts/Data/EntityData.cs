using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 23/03/15
/// Last Edited: 23/03/15
/// Entity Data - Class used to define properties for game's entities such as characters and enemies.
/// </summary>
[System.Serializable]
public class EntityData : MonoBehaviour 
{
	//=================
	// EDITOR PROPERTIES
	//=================
	public string Name;					// Entity's name
	public float MaxHealthPoints;		// Entity's Max Health
	public ArmorData Armor;				// Entity's Armor
	public WeaponData Weapon;			// Entity's Weapon
	//===================
	// PRIVATE PROPERTIES
	//===================
	public float Health;				// Current Health points
	private float HealthRatio;			// Ratio between MaxHP / Health
	/// =========================
	/// START
	/// <summary>
	/// Start this instance.
	/// </summary>
	/// =========================
	public virtual void Start()
	{
		// Assign Health
		Health = MaxHealthPoints;
		// initialize HealthRatio
		HealthRatio = 1.0f;
	}
	/// =========================
	/// UPDATE
	/// <summary>
	/// Update this instance.
	/// </summary>
	/// =========================
	public virtual void Update()
	{
		UpdateHealthRatio();
	}
	/// =========================
	/// UPDATE HEALTH RATIO
	/// <summary>
	/// Updates the health ratio.
	/// </summary>
	/// =========================
	private void UpdateHealthRatio()
	{
		HealthRatio = Health / MaxHealthPoints;
	}
	/// =========================
	/// GET HEALTH RATIO
	/// <summary>
	/// Gets the health ratio.
	/// </summary>
	/// <returns>The health ratio.</returns>
	/// ========================
	public float getHealthRatio()
	{
		return HealthRatio;
	}
   /// <summary>
   /// Resets Numeric Values of this Entity
   /// </summary>
   public void Reset()
   {
      Health = MaxHealthPoints;
   }
}
