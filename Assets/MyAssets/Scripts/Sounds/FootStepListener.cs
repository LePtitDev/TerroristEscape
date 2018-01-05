using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepListener : MonoBehaviour {

	public float distanceMax = 20f;
	private GameObject terrorist = null;
	private float distance = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (terrorist == null)
			terrorist = GameObject.Find ("Terrorist(Clone)");

		if (terrorist != null) {
			distance = (transform.position - terrorist.transform.position).magnitude / distanceMax;
		} else {
			distance = distanceMax + 1;
		}
	}

	public float getVolume(){
		return (distance <= 1f) ? 1f - distance : 0;
	}
}
