using UnityEngine;
using System.Collections;

public class AttackState : BaseState
{
   public Transform target;                     // Target to be attacked
   public float minDistance;                    // Minimum Distance from player
   public BaseState targetOutBounds;            // State to change to when distance from player > minDistance
   private Animator animator;                    // Animator
   public string attackAnimationName;            // Property of animator
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
      // Obtain transform
      if (!eTransform)
         eTransform = this.transform;
      if (!animator)
         animator = GetComponent<Animator>();
   }
   /// ===================
   /// EXECUTE
   /// <summary>
   /// Executes this state
   /// </summary>
   /// ===================
   public override void Execute()
   {
      base.Execute();

      // Look at target
      eTransform.LookAt(target);
      if (animator)
         animator.SetTrigger(attackAnimationName);
      // Obtain distance between entity and target
      float distance = (target.position - eTransform.position).magnitude;
      // check if player is further than minDistance
      if (distance > minDistance)
      {
         // Change to targeOutBounds state
         fsmComponent.ChangeState(targetOutBounds);
      }
   }
}
