using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public GameObject characterController;
	public GameObject prefabSoundEffect;
	public bool enableScreensLight = true;
	public float distanceMaxLight = 15.0f;
	public QualityLevel qualityLevel;

	public static GameObject controller;
	public static GameObject soundEffect;
	public static bool screensLight;
	public static float distanceLight;

	// Use this for initialization
	void Start () {
		controller = characterController;
		soundEffect = prefabSoundEffect;
		screensLight = enableScreensLight;
		distanceLight = distanceMaxLight;

		//UnityEditor.PlayerSettings.MTRendering = false;

	}
	
	// Update is called once per frame
	void Update () {
		QualitySettings.SetQualityLevel ( (int)qualityLevel );
	}
}
