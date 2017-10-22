using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneClass : MonoBehaviour {
	[Header("Scene")]

	public StepClass[] steps;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < steps.Length; i++) {
			steps [i].Start ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < steps.Length; i++) {
			steps [i].Update ();
		}
	}
}
