using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensLight : MonoBehaviour {

	private float cpt = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (cpt == 0) {
			cpt++;
		}
		else if (cpt == 1) {
			Light l = gameObject.GetComponent<Light> ();
			if (l != null) {
				l.enabled = Global.screensLight;
			}
			cpt++;
		}
	}
}
