using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 24/03/15
/// Last Edited: 24/03/15
/// Enemy Creator - Class used to Create a Group of Enemies
/// </summary>
public class EnemyCreator : MonoBehaviour 
{
   public int NumberOfEnemies;               // Total Number of Enemies 
   public List<EntityData> PreSets;          // Presets
   public EnemyGroup Group;                  // Enemy Group
}
