using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WanderState : BaseState
{
   /// ===================
   /// PROPERTIES
   /// ===================  
   private Transform eTransform;        // Instance of entity's transform
   private Animator animator;           // Instance of Animator
   public Vector3 target;               // Current Target position
   public float minDistance = 3;       // Minumum distance between target
   public List<IsWaypoint> targets;     // List of targets to wander at
   /// ===================
   /// ENTER
   /// <summary>
   /// Enters this state
   /// </summary>
   /// ===================
   public override void Enter(FSM fsmComp)
   {
      base.Enter(fsmComp);
      // Obtain transform
      if (!eTransform)
         eTransform = this.transform;
      // Obtain animator
      if (!animator)
         animator = GetComponent<Animator>();
      // Obtain list of targets
      targets = new List<IsWaypoint>(GameObject.FindObjectsOfType<IsWaypoint>());
      // set target
      StartCoroutine(SetNewDestination());
   }
   /// ===================
   /// EXECUTE
   /// <summary>
   /// Execute this state
   /// </summary>
   /// ===================
   public override void Execute()
   {
      base.Execute();
  
      //--------------
      // IS PLAYER SEEN
      //--------------
      // Check if player is seen
      //if(player && IsPlayerSeen())
         // go to new state
         //fsmComponent.ChangeState(PlayerSpotted);
      // -------------
      // DESTINATIONS
      // -------------
      // check distance between position and target
      float distance = (target - eTransform.position).magnitude;
      // check if distance < minDistance
      if (distance < minDistance)
         // set a new Destination 
         StartCoroutine(SetNewDestination());
	  if(path.status == NavMeshPathStatus.PathPartial)
		   // set a new Destination 
		   StartCoroutine(SetNewDestination());
   }
   /// ===================
   /// SET NEW DESTINATION
   /// <summary>
   /// Sets a new path Destination
   /// </summary>
   /// ===================
   IEnumerator SetNewDestination()
   {
      // Obtain new position
      target = targets[Random.Range(0, targets.Count)].gameObject.transform.position;
      // Obtain new destination
      agent.SetDestination(target);
      // calculate path
      NavMesh.CalculatePath(eTransform.position, target, -1, path);	 
      // Go to walking state
      state.setState(EntityStateData.eEntityState.WALKING);
      yield return null;
   }

}
