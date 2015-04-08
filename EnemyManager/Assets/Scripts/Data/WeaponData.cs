using UnityEngine;
using System.Collections;
[System.Serializable]
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 23/03/15
/// Last Edited: 28/03/15
/// Weapon Data - Class to be used to define Weapons for characters
/// </summary>
public class WeaponData : MonoBehaviour
{
	public string Name;			// Name of Weapon
	public int DamagePoints;	// Weapon's Damage Points
   public int Noise;          // Noise of Weapon when firing
   public int Fade;           // Fade rate of noise
   public int Ammo;           // Weapon Ammo
   public int Accuracy;       // Weapon Accuracy
}
