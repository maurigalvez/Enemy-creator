using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 24/03/15
/// Last Edited: 24/03/15
/// Enemy Group - Class used to Group multiple Enemies
/// </summary>
[System.Serializable]
public class EnemyGroupData : MonoBehaviour
{
   //===============
   // PROPERTIES
   //=============== 
   public List<EntityData> Members;    
   private int CurrentIndex;                          // Current Index in Array
   private int TotalEnemies;                          // Total Number of Enemies

}
