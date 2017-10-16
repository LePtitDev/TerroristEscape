using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsMesh : MonoBehaviour {

	public int indexMaterial = 0;
	public Material lightOn;
	public Material lightOff;
	public bool lightenable = true;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lightenable) {
			rend.materials [indexMaterial].CopyPropertiesFromMaterial (lightOn);
		} else {
			rend.materials [indexMaterial].CopyPropertiesFromMaterial (lightOff);
		}
	}

	public void SetLight(bool l){
		lightenable = l;
	}
}
