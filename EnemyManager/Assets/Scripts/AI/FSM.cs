using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour 
{
   public BaseState currentState;
   private EntityStateData state;
   private NavMeshAgent agent;
	// Use this for initialization
	void Start ()
   {
	  // obtain state
	  state = this.GetComponent<EntityStateData>();
	  agent = this.GetComponent<NavMeshAgent>();
      // Enter current state
      if (currentState)         
         currentState.Enter(this);
	}
	
	// Update is called once per frame
	void Update () 
   {      
      // Execute current state
      if (state.getState() != EntityStateData.eEntityState.DEAD && currentState)
         currentState.Execute();
	  else
		 agent.enabled = false;
	}

   public void ChangeState(BaseState newState)
   {
      // Exit current state
      if (currentState)
         currentState.Exit();
      // Assign new state
      currentState = newState;
      // Enter new state
      currentState.Enter(this);
   }
}
