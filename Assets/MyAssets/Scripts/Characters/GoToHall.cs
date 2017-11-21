using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToHall : MonoBehaviour {
	[SerializeField]
	Transform destination;
	NavMeshAgent navMeshAgent;


	// Use this for initialization
	void Start () {

		navMeshAgent = this.GetComponent<NavMeshAgent>();

		if (navMeshAgent == null) {
			Debug.LogError ("the navmesh agent component is not attached to " + gameObject.name);

		} else {
			SetDestination ();
		}
		
	}


	private void SetDestination ()
		{
			if (destination != null)
			{
				Vector3 targetVector = destination.transform.position;
				navMeshAgent.SetDestination(targetVector);
			}
		}
	
	// Update is called once per frame
	void Update () {
		
	}
}
