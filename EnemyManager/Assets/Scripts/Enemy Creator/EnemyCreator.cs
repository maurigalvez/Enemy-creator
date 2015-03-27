using UnityEngine;
using System.Collections;
/// <summary>
/// Author: Mauricio Galvez
/// Date Created: 24/03/15
/// Last Edited: 24/03/15
/// Enemy Creator - Class used to Create a Group of Enemies
/// </summary>
public class EnemyCreator : MonoBehaviour 
{
   public EntityData[] PreSetEnemies;		 		// List of all preset enemies
   public bool DebugEnabled;			     		// True if Debug Scripts should be added to enemies, false otherwise
   public EnemyGroup Group;                  		// Enemy Group
   public FormationData.eFormation Formation;   	// Formation that enemy group will be distributed in
   	


}
