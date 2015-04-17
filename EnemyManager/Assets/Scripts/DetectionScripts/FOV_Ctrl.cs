using UnityEngine;
using UnityEditor;
using System.Collections;

// Player detection based on character's field of view
public class FOV_Ctrl : MonoBehaviour
{

   // target for enemy attack
   public GameObject target;
   public bool showGizmos = true;

   // params for FOV area arc
   public float detectionRadius = 5f;
   public float viewAngle = 60f;

   private NoiseLevelCtrl noiseDetectionCtrl;
   private EntityStateData entityState;

   // raycast different bone on player based on his current action
   private Vector3 raycastPos = Vector3.zero;
   private Transform eTransform;                                     // Instance of transform of this instance
   private Transform tTransform;                                     // Instance pof transform of target
   public enum PlayerStatus 
   {
      Detected,
      Undetected,
      Hiding  // player in FOV area of enemy but hidden behind obstacle
   }
   [HideInInspector]
   public PlayerStatus curPlayerStatus;


   public void Awake()
   {
      if (!target)
         return;
      // obtain entity state data
      entityState = target.GetComponent<EntityStateData>();
      // Obtain entity state   
      if (!entityState)
         Debug.LogError("No EntitySataeData has been assigned to " + target.name);      

      // obtain transform of this entity
      eTransform = this.transform;

      noiseDetectionCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<NoiseLevelCtrl>();
      
      curPlayerStatus = PlayerStatus.Undetected;

      if (target == null)
      {
         if (EditorUtility.DisplayDialog("FOV_Ctrl: Detection Target not assigned!",
         "Assign the object with tag 'Player' by default?", "Assign", "Cancel"))
         {
            target = GameObject.FindGameObjectWithTag("Player");
         }
         else
         {
            // target not assigned
         }
      }
      else
         // assign target Transform
         tTransform = target.transform;
   }

   public void Update()
   {
      if (!target)
         return;
      if (Input.GetKeyDown(KeyCode.E))
      {
         showGizmos = !showGizmos;
      }

      // determine how player will be raycasted based on his curState
      if (entityState.IsCrouching())
      {
         Vector3 upDir = new Vector3(0, 1.1f, 0);
         // raycast player's head bone
         raycastPos = eTransform.position + upDir;
      }
      else
      {
         // raycast player's torso bone
         raycastPos = eTransform.position + eTransform.up;
      }

      // direction from enemy to the player
      Vector3 dirToPlayer = tTransform.position - eTransform.position;
      float distanceToPlayerSQR = dirToPlayer.sqrMagnitude;
      // angle between enemy forward and player position
      float angleDeg = Vector3.Angle(dirToPlayer, eTransform.forward);
      //Debug.Log("AngleDeg BTW: " + angleDeg);

      // if player makes noise ->
      if (noiseDetectionCtrl.curNoiseLevel > 0f)
      {
         // ------------------- NOISE DETECTION --------------------------
         DetectNoise();
      }
      else
      {
         // detect on sight if there is no noise ->
         // ------------------- SIGHT DETECTION CONE --------------------------
         RaycastHit hit;
         // angle between enemy forward and player location is less than half of FOV angle
         bool withinAngle = (angleDeg < viewAngle * 0.5f);
          // player inside the detectionRadius
         bool withinRadius = (distanceToPlayerSQR < detectionRadius * detectionRadius);
         if (withinAngle && withinRadius)
         {
            // raycast the player from enemy position
            if (Physics.Raycast(raycastPos, dirToPlayer, out hit))
            {
               // hit the player
               if (hit.collider.gameObject == target)
               {
                  curPlayerStatus = PlayerStatus.Detected;
                  //Debug.Log("Raycast hit: " + hit.transform.gameObject.name);
               }
               else
               {
                  // player behind obstacle
                  curPlayerStatus = PlayerStatus.Hiding;
               }
            }
            else
            {
               // player behind obstacle
               curPlayerStatus = PlayerStatus.Hiding;
            }
         }
         else
         {
            curPlayerStatus = PlayerStatus.Undetected;
         }
      }
   }

   public void DetectNoise()
   {
      if (!target)
         return;
      // if enemy's detection circle and player's noise circle intersect -> enemy detects player
      if (CheckCircleIntersect(eTransform.position, detectionRadius, tTransform.position, noiseDetectionCtrl.curNoiseLevel))
      {
         curPlayerStatus = PlayerStatus.Detected;
      }
   }

   public bool CheckCircleIntersect (Vector3 c1_pos, float c1_r, Vector3 c2_pos, float c2_r)
   { 
      // two circles intersect if the distance is smaller than the sum of their detectionRadiuses
      return (Vector3.Distance(c1_pos, c2_pos) <= (c1_r + c2_r)); 
   }
}
