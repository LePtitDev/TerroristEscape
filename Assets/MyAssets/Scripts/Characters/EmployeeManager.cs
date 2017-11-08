using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour {

    MonoBehaviour currentTask;

	// Use this for initialization
	void Start () {
        currentTask = gameObject.AddComponent<TaskCoffeeBreak>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
