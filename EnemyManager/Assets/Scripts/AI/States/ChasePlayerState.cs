using UnityEngine;
using System.Collections;

public class ChaseEntityState : BaseState
{
   public Transform target;                     // Target to pursuit
   public BaseState targetReachedState;         // State to go to when player is reached
   public BaseState targetOutOfSight;           // State to go to when player is out of sight
   public float minDistance;                    // Minumum distance from player
   public float RangeOfSight;                   // Range of sight of entity
   private Transform eTransform;                // Transform of this entity
   /// ===================
   /// ENTER
   /// <summary>
   /// Enters this state
   /// </summary>
   /// ===================
   public override void Enter(FSM fsmComp)
   {
      base.Enter(fsmComp);
      // obtain this transform
      eTransform = this.transform;
   }
   /// ===================
   /// EXECUTES
   /// <summary>
   /// Executes this state
   /// </summary>
   /// ===================
   public override void Execute()
 	{
      base.Execute();   
      // set player as destination
      agent.SetDestination(target.position);
      // calculate path
      NavMesh.CalculatePath(eTransform.position, target.position, -1, path);
      // Go to walking state
      state.setState(EntityStateData.eEntityState.WALKING);
      // Check distance between this and player
      float distance = (target.position - eTransform.position).magnitude;
      if(distance <= minDistance)
      {
         agent.Stop();
         // target has been reached
         fsmComponent.ChangeState(targetReachedState);
      }
      else if(distance >= RangeOfSight)
      {
         // target is out of sight
         fsmComponent.ChangeState(targetOutOfSight);
      }
   }
}
