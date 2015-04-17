using UnityEngine;
using System.Collections;

public class EntityStateData : MonoBehaviour {

   public enum eEntityState
   {
      IDLE = 0,
      WALKING = 1,
      RUNNING = 2, 
      CROUCHING = 3,
      DEAD = 4,
   }

   public eEntityState data;  
   
   public void setState(eEntityState newState)
   {
      data = newState;
   }

   public eEntityState getState()
   {
      return data;
   }

   public void GoToDeadState()
   {
      data = eEntityState.DEAD;
   }

   public bool IsCrouching()
   {
      return data == eEntityState.CROUCHING;
   }

   public bool IsOnIdle()
   {
      return data == eEntityState.IDLE;
   }

   public bool IsWalking()
   {
      return data == eEntityState.WALKING;
   }
   public bool IsRunning()
   {
      return data == eEntityState.RUNNING;
   }
}
