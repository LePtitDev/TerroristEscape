using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableReflexionOnStart : MonoBehaviour {

	public bool enableReflexion = true;

	// Use this for initialization
	void Start () {

		DisableReflexion( gameObject );

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DisableReflexion(GameObject go){

		for (int i = 0; i < go.transform.childCount; i++) {
			DisableReflexion (go.transform.GetChild (i).gameObject);
		}

		ReflectionProbe rp = go.GetComponent<ReflectionProbe> ();

		if (rp != null) {
			rp.enabled = enableReflexion;	
		}
	}
}
