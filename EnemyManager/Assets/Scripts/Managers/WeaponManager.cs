using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 28/03/15
/// Last Edited: 28/03/15
/// Weapon Manager - Class to be used to create Weapons Prefabs
/// </summary>
public class WeaponManager : MonoBehaviour 
{
	public WeaponData[] Weapons;
	public string name;
	public int damage;
	public GameObject model;

}
