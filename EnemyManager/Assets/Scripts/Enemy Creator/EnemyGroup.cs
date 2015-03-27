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
public class EnemyGroup 
{
   //===============
   // PROPERTIES
   //=============== 
   public EntityData[] Enemies;                       // Initialize Enemies' array        
   private int CurrentIndex;                          // Current Index in Array
   private int TotalEnemies;                          // Total Number of Enemies
   ///====================
   /// ENEMY GROUP CONSTRUCTOR
   /// <summary>
   /// Enemy Group constructor. Initializes Enemies array with NumberOfEnemies
   /// </summary>
   /// <param name="NumberOfEnemies">Total Number of Enemies in Group</param>
   ///====================
   public EnemyGroup(int NumberOfEnemies) 
   {
      // Initialize enemies array
      Enemies = new EntityData[NumberOfEnemies];
      // Initialize Index
      CurrentIndex = 0;
      // Set Total number of enemies
      TotalEnemies = NumberOfEnemies;
   }
   ///====================
   /// ADD ENEMY
   /// <summary>
   /// Adds the given entity to the list of Enemies
   /// </summary>
   /// <param name="_enemy"> Enemy entity to be added as part of this group</param>
   ///=====================
   public void AddEnemy(EntityData _enemy)
   {
      // Check if CurrentIndex is Lower than TotalEnemies -1 
      if (CurrentIndex < TotalEnemies - 1)
      {
         // Add a new enemy
         Enemies[CurrentIndex] = _enemy;
         // Increase Current Index
         CurrentIndex++;
      }
      else
         Debug.LogWarning("Group of Enemies is Full!");
   }
   ///====================
   /// REMOVE ENEMY
   /// <summary>
   /// Removes the Enemy at the given IndexPosition from Enemies Array
   /// </summary>
   /// <param name="IndexPosition">Index Position of Enemy to be Removed</param>
   ///====================
   public void RemoveEnemy(int IndexPosition)
   {
      // Check if IndexPosition is negative OR higher than TotalEnemies
      if(IndexPosition < 0 || IndexPosition > TotalEnemies)
      {
         Debug.LogError("Invalid IndexPosition given!");
         return;
      }
      // Remove Enemy from IndexPosition
      Enemies[IndexPosition] = null;
   }
}
