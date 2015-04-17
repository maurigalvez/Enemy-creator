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
	private EntityStateData state;
   private GroupData squad;
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
		// obtain state
		state = this.GetComponent<EntityStateData>();
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
		// check if health is zero
		if(Health <= 0)
			state.GoToDeadState();

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
		if(HealthRatio < 0)
			HealthRatio = 0;
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
   public void inflictDamage(float dmg)
   {
		Health -= dmg;
   }
   /// <summary>
   /// Sets squad of this entity to given group
   /// </summary>
   /// <param name="g">Squad this entity belongs to</param>
   public void SetSquad(GroupData g)
   {
      squad = g;
   }
   /// <summary>
   /// Returns squad of this entity
   /// </summary>
   /// <returns>squad of this entity</returns>
   public GroupData GetSquad()
   {
      return squad;
   }
}
