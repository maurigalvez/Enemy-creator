using UnityEngine;
using System.Collections;

public class BaseState : MonoBehaviour 
{
   protected NavMeshAgent agent;                // NavMeshAgent
   protected EntityStateData state;             // State
   protected FSM fsmComponent;                  // Reference to fsm Component
   protected NavMeshPath path;                  // NavMeshPath
   public bool drawPathToDestination;           // True if PathToDestination should be drawn
   /// ====================
   /// START
   /// ====================
   public virtual void Enter(FSM fsmComp)
   {
      // Set FSM
      if(!fsmComponent)
         fsmComponent = fsmComp;
     
      Init();
   }
   /// ====================
   /// INIT
   /// <summary>
   /// Initialize this State
   /// </summary>
   /// ====================
   public virtual void Init()
   {
      // obtain navMeshAgent
      if (!agent)
         agent = GetComponent<NavMeshAgent>();
      // obtain playerStateData
      if (!state)
         state = GetComponent<EntityStateData>();
      // Create new NavMeshPath   
      path = new NavMeshPath();
   }
   /// ====================
   /// EXECUTE
   /// <summary>
   /// Executes this path
   /// </summary>
   /// ====================
   public virtual void Execute() 
   {
      // Check if path should be drawn
      if (drawPathToDestination)
         DrawPath();         
   }
   /// ====================
   /// EXIT
   /// <summary>
   /// Exits this state
   /// </summary>
   /// ====================
   public virtual void Exit() { }

   /// ====================
   /// DRAW PATH
   /// <summary>
   /// Draws NavMeshPath
   /// </summary>
   /// ====================
   public void DrawPath()
   {
      Color _drawColor;
      if (path.status == NavMeshPathStatus.PathComplete)
      {
         _drawColor = Color.white;

      }
      else if (path.status == NavMeshPathStatus.PathInvalid)
      {
         _drawColor = Color.red;
      }
      else
      {
         _drawColor = Color.yellow;
      }
      for (int i = 0, j = 1; j < path.corners.Length; i++, j++)
      {
         Debug.DrawLine(path.corners[i], path.corners[j], _drawColor);
      }
   }

}
