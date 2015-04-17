using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

// Script for FOV visualization
public class FOV_Debug : MonoBehaviour
{
   // target to attack
   private GameObject target;

   private GameObject detectionOrb;
   private Material detectionOrbMat;

   private NoiseLevelCtrl noiseDetectionCtrl;
   public EntityStateData targetState;
   private FOV_Ctrl fovCtrl;

   // raycast different bone on player based on his current action
   private Vector3 raycastPos = Vector3.zero;

	void Start ()
   {
      // obtain targetState  
      if (!targetState)
         Debug.LogError("No EntitySataeData has been assigned to " + gameObject.name);      

      noiseDetectionCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<NoiseLevelCtrl>();

      fovCtrl = gameObject.GetComponent<FOV_Ctrl>();

      if (fovCtrl.target != null)
      {
         target = fovCtrl.target;
      }

      // Detection Orb init ---------------------------------------------------------
      // displays detection indicator gizmo above player's head when scene is playing
      detectionOrb = target.transform.FindChild("DetectionIndicator").gameObject;
      detectionOrbMat = Resources.Load("Materials/Undetected", typeof(Material)) as Material;
      detectionOrb.renderer.material = detectionOrbMat;
      detectionOrb.SetActive(fovCtrl.showGizmos);
	}

   private void OnDrawGizmos()
   {
      if (fovCtrl.showGizmos)
      {
         Gizmos.color = noiseDetectionCtrl.gizmoColor;
         Gizmos.DrawWireSphere(target.transform.position, noiseDetectionCtrl.curNoiseLevel);
      }
   }
	
	void Update () 
   {
      if (fovCtrl.showGizmos)
      {
         // direction from enemy to the player
         Vector3 dirToPlayer = target.transform.position - transform.position;

         if (targetState.IsCrouching())
         {
            Vector3 upDir = new Vector3(0, 1.1f, 0);
            // raycast player's head bone
            raycastPos = transform.position + upDir;
         }
         else
         {
            // raycast player's torso bone
            raycastPos = transform.position + transform.up;
         }
         Debug.DrawRay(raycastPos, dirToPlayer, Color.green);
      }

      // display detection orb indicator
      DrawGizmoInGame();
	}

   public void DrawGizmoInGame()
   {
      detectionOrb.SetActive(fovCtrl.showGizmos);

      if (fovCtrl.curPlayerStatus == FOV_Ctrl.PlayerStatus.Detected)
      {
         detectionOrbMat = Resources.Load("Materials/Detected", typeof(Material)) as Material;
      }
      else if (fovCtrl.curPlayerStatus == FOV_Ctrl.PlayerStatus.Hiding)
      {
         detectionOrbMat = Resources.Load("Materials/PartialDetection", typeof(Material)) as Material;
      }
      else
      {
         detectionOrbMat = Resources.Load("Materials/Undetected", typeof(Material)) as Material;
      }

      detectionOrb.renderer.material = detectionOrbMat;
   }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FOV_Ctrl))]
public class DrawSolidDisc : Editor
{
   void OnSceneGUI()
   {
      FOV_Ctrl myTarget = (FOV_Ctrl)target;

      if (myTarget.showGizmos)
      {
         // character's range
         Handles.color = new Color(1, 0, 0, 0.2f);
         Handles.DrawSolidDisc(myTarget.transform.position, Vector3.up, myTarget.detectionRadius);

         // direction of sight
         if (myTarget.curPlayerStatus == FOV_Ctrl.PlayerStatus.Detected)
         {
            Handles.color = new Color(1, 0.6f, 0, 0.3f);
         }
         else if (myTarget.curPlayerStatus == FOV_Ctrl.PlayerStatus.Hiding)
         {
            Handles.color = new Color(1, 1, 0, 0.3f);
         }
         else
         {
            Handles.color = new Color(0, 1, 0, 0.3f);
         }

         // right
         Handles.DrawSolidArc(
            myTarget.transform.position,
            myTarget.transform.up,
            myTarget.transform.forward,
            myTarget.viewAngle / 2f,
            myTarget.detectionRadius);
         // left
         Handles.DrawSolidArc(
            myTarget.transform.position,
            myTarget.transform.up,
            myTarget.transform.forward,
            -myTarget.viewAngle / 2f,
            myTarget.detectionRadius);

         // character's forward
         Handles.color = new Color(0, 1, 1, 1f);
         myTarget.detectionRadius = Handles.ScaleValueHandle(myTarget.detectionRadius,
             myTarget.transform.position + myTarget.transform.forward * myTarget.detectionRadius,
             myTarget.transform.rotation,
             3f,
             Handles.ConeCap,
             1f);
      }
   }
}
#endif