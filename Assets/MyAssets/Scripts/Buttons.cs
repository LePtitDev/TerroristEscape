using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

	public bool isEnable = true;
	private bool prev_state = true;

	public FMODUnity.StudioEventEmitter soundOn;
	public FMODUnity.StudioEventEmitter soundOff;

	private Activable activable;

	// Use this for initialization
	void Start () {
		activable = gameObject.GetComponent<Activable> ();

		if (activable == null)
			Debug.Log ("Need Activable Script!");

		isEnable = activable.isOpened ();
		prev_state = isEnable;
	}
	
	// Update is called once per frame
	void Update () {

		isEnable = activable.isOpened ();

		if (isEnable != prev_state) {
			EnableLight (gameObject, isEnable);
			EnableLightMesh (gameObject, isEnable);
			prev_state = isEnable;
		}

		if (activable.isClosing () && soundOff!=null && !soundOff.IsPlaying() ) {
			soundOff.Play ();
		}
		if (activable.isOpening () && soundOn!=null  && !soundOn.IsPlaying() ) {
			soundOn.Play ();
		}
	}

	void EnableLight(GameObject go, bool e){
		if (go.GetComponent<Lights> () != null) {
			go.GetComponent<Lights> ().SetLight (activable.isOpened ());
		} else {
			for (int i = 0; i < go.transform.childCount; i++) {
				EnableLight (go.transform.GetChild (i).gameObject, activable.isOpened ());
			}
		}
	}

	void EnableLightMesh(GameObject go, bool e){
		if (go.GetComponent<LightsMesh> () != null) {
			go.GetComponent<LightsMesh> ().SetLight (activable.isOpened ());
		} else {
			for (int i = 0; i < go.transform.childCount; i++) {
				EnableLightMesh (go.transform.GetChild (i).gameObject, activable.isOpened ());
			}
		}
	}
}
