using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoIntro : MonoBehaviour {

	public bool skipIntroduction = false;

	// Use this for initialization
	void Start () {
		if (skipIntroduction) {
			Global.animationEnded = true;
			gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
