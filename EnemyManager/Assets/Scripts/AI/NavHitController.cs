using UnityEngine;
using System.Collections;

public class NavHitController : Mixin 
{
   public NavMeshAgent agent;
   public EntityStateData state;
   private NavMeshPath path;
   private bool drawPath = false;
   /// ====================
   /// UPDATE
   /// <summary>
   /// Use this for initialization
   /// </summary>
   /// ====================
	void Start () 
   {
      path = new NavMeshPath();
	}

   /// ====================
   /// UPDATE
   /// <summary>
	/// Update called once per frame
	/// </summary>
   /// ====================
	void Update () 
   {
	   // Update NavPath
      UpdateNavPath();
	}
   /// ====================
   /// UPDATE NAVPATH
   /// <summary>
   /// Update NavPath
   /// </summary>
   /// ====================
   private void UpdateNavPath()
   {
      //==============
      // OBTAIN INPUT FROM MOUSE
      //==============
      if (Input.GetMouseButton(0))
      {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;

         if (Physics.Raycast(ray, out hit, 100))
         {
            agent.SetDestination(hit.point);
            NavMesh.CalculatePath(transform.position, hit.point, -1, path);
            // Go to walking state
            state.setState(EntityStateData.eEntityState.WALKING);
            drawPath = true;
         }
      }
      //===============
      // CHECK STATUS
      //===============
      if (agent.transform.position == agent.destination)
         GoBackToIdle();
      //===============
      // Check if path should be drawn
      //===============
      if (drawPath)      
         DrawPath();      
   }
   /// ====================
   /// DRAW PATH
   /// <summary>
   /// Draws NavMeshPath
   /// </summary>
   /// ====================
   private void DrawPath()
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
   /// ==================
   /// GO BACK TO IDLE
   /// <summary>
   /// Draws NavMeshPath
   /// </summary>
   /// ==================
   public void GoBackToIdle()
   {
      state.setState(EntityStateData.eEntityState.IDLE);
   }
}
