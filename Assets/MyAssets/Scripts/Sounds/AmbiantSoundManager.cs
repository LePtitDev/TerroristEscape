using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiantSoundManager : MonoBehaviour {

	private GameObject terrorist = null;
	private GameObject player = null;

	public FMODUnity.StudioEventEmitter phoneRing;
	public FMODUnity.StudioEventEmitter MusicLoop;
	public FMODUnity.StudioEventEmitter CopsSounds;
	public FMODUnity.StudioEventEmitter HeartBeats;

	// Use this for initialization
	void Start () {
		MusicLoop.Play ();
		CopsSounds.Play ();
		HeartBeats.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (terrorist == null)
			terrorist = GameObject.Find ("Terrorist(Clone)");
		if (player == null) {
			player = GameObject.Find ("Server_FPS(Clone)");
			return;
		}

		// Parameters: InGame / Death / Intensity / Victory

		float param_InGame = player.GetComponent<MoveFPS> ().enabled ? 1f : 0f;
		float param_Death = Global.GameOver ? 1f : 0f;
		float param_Intensity = 0f;
		if( terrorist != null )
			param_Intensity = 1f - ((transform.position - terrorist.transform.position).magnitude / 20f);
		float param_Victory = (Global.victory) ? 1f : 0f;

		MusicLoop.SetParameter ("InGame", param_InGame);
		MusicLoop.SetParameter ("Death", param_Death);
		MusicLoop.SetParameter ("Intensity", param_Intensity);
		MusicLoop.SetParameter ("Victory", param_Victory);

		HeartBeats.SetParameter ("Intensity", param_Intensity);
		HeartBeats.SetParameter ("Death", Mathf.Min (1f, param_Death + param_Victory));

		float param_Timer = 1f - Global.timeLeft / Global.duration;
		if (Global.GameOver || Global.victory)
			param_Timer = 0f;

		if (!CopsSounds.IsPlaying ())
			CopsSounds.Play ();
		CopsSounds.SetParameter ("Timer", param_Timer);

		if (Global.phoneRing && !phoneRing.IsPlaying()) 
			phoneRing.Play ();
		if (!Global.phoneRing && phoneRing.IsPlaying()) 
			phoneRing.Stop ();
		
	}
}
