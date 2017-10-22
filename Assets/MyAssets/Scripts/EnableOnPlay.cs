using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnPlay : MonoBehaviour {

	public GameObject[] gameobjects;

	// Use this for initialization
	void Start () {

		foreach (GameObject go in gameobjects) {
			go.SetActive (true);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
