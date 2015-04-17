using UnityEngine;
using System.Collections;

public abstract class Mixin : MonoBehaviour {

	public string name;
	protected GameObject recipient;

	public void SetRecipient(GameObject r)
	{
		recipient = r;
	}

	public GameObject GetRecipient()
	{
		return recipient;
	}
}
