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

	public GameObject _canvas;
	public GameObject _prefabScreenGameOver;

	public static GameObject canvas;
	public static GameObject prefabScreenGameOver;
	public static bool GameOver = false;
	public static bool victory = false;
	public static bool rescueCalled = false;

	public static float timeLeft;
	public static float duration;

	public static bool hideCursor = true;
	public static bool animationEnded = false;
	public static bool phoneRing = false;

	// Use this for initialization
	void Start () {
		controller = characterController;
		soundEffect = prefabSoundEffect;
		screensLight = enableScreensLight;
		distanceLight = distanceMaxLight;

		canvas = _canvas;
		prefabScreenGameOver = _prefabScreenGameOver;

		//UnityEditor.PlayerSettings.MTRendering = false;

	}
	
	// Update is called once per frame
	void Update () {
		QualitySettings.SetQualityLevel ( (int)qualityLevel );
	}
}
