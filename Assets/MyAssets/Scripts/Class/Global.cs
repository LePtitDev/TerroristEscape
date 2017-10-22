using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public GameObject characterController;
	public GameObject prefabSoundEffect;
	public static GameObject controller;
	public static GameObject soundEffect;

	// Use this for initialization
	void Start () {
		controller = characterController;
		soundEffect = prefabSoundEffect;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
