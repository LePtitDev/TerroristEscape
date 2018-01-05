using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepEmitter : MonoBehaviour {

	public FootStepListener listener;
	public FMODUnity.StudioEventEmitter footstep;

	// Use this for initialization
	void Start () {
		footstep.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		footstep.SetParameter("VolumeFootStep", (listener.getVolume()>0)?1f-Mathf.Min(1f,listener.getVolume()):1f );
	}
}
