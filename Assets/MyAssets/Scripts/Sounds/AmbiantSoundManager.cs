using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiantSoundManager : MonoBehaviour {

	public FMODUnity.StudioEventEmitter ambiant;
	private GameObject terrorist = null;

	public FMODUnity.StudioEventEmitter looseMusic;
	public FMODUnity.StudioEventEmitter phoneRing;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (terrorist == null) {
			terrorist = GameObject.Find ("Terrorist(Clone)");
		}

		float intensity = 0.0f;

		if( terrorist != null ){
			intensity = 1f - ((transform.position - terrorist.transform.position).magnitude / 20f);
		}

		ambiant.SetParameter ("Intensity", intensity);
		ambiant.SetParameter ("Timer", 1f - Global.timeLeft / Global.duration);

		if (Global.GameOver && !looseMusic.IsPlaying()) {
			looseMusic.Play ();
		}

		if (Global.phoneRing && !phoneRing.IsPlaying()) {
			phoneRing.Play ();
		}
		if (!Global.phoneRing && phoneRing.IsPlaying()) {
			phoneRing.Stop ();
		}
	}
}
