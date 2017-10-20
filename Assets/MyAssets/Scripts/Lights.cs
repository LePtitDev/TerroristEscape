using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {

	private GameObject Controller;
	public float distance = 10.0f;

	private bool isLightEnable = true;

	// Use this for initialization
	void Start () {
		Controller = Camera.main.gameObject;

		if (Controller == null)
			Debug.Log ("No Camera Found");
	}
	
	// Update is called once per frame
	void Update () {
		Light l = GetComponent<Light> ();

		if (Controller != null)
			l.enabled = isLightEnable &&
				(Controller.transform.position - transform.position).magnitude <= distance &&
				(transform.position.y - Controller.transform.position.y) > 0.0f &&
				(transform.position.y - Controller.transform.position.y) < 3.4f;
		else
			l.enabled = isLightEnable;
	}

	public void SetLight(bool l){
		isLightEnable = l;
	}
}
