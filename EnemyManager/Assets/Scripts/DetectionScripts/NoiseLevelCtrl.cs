using UnityEngine;
using System.Collections;

public class NoiseLevelCtrl : MonoBehaviour 
{
   public EntityStateData entityState;                   // Instance of Entity State

   public struct ActionNoiseLevel
   {
      public float idle;
      public float crouching;
      public float walking;
      public float running;
   }

   private ActionNoiseLevel noiseLevel;
   [HideInInspector]
   public float curNoiseLevel;
   [HideInInspector]
   public Color gizmoColor;

	void Start () 
   {
     
      curNoiseLevel = noiseLevel.idle;
      gizmoColor = Color.cyan;

      noiseLevel.idle = 0f;
      noiseLevel.crouching = 0f;
      noiseLevel.walking = 1f;
      noiseLevel.running = 5f;
	}
	
	void Update () 
   {
      SetNoiseLevel();
	}

   public void SetNoiseLevel()
   {
      if (entityState.IsOnIdle())
      {
         curNoiseLevel = noiseLevel.idle;
      }
      else if (entityState.IsWalking())
      {
         curNoiseLevel = noiseLevel.walking;
      }
      else if (entityState.IsCrouching())
      {
         curNoiseLevel = noiseLevel.crouching;
      }
      else if (entityState.IsRunning())
      {
         curNoiseLevel = noiseLevel.running;
      }
   }
}
