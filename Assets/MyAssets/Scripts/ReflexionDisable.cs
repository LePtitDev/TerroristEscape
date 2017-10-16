using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflexionDisable : MonoBehaviour {

	public float FPS_Min = 60.0f;
	public float time_Min = 3.0f;

	private float time = 0;
	private float time_low = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (time < time_Min) {
			time += Time.deltaTime;
		}

		float fps = 1.0f / (Time.deltaTime);

		if (fps < FPS_Min && time >= 3.0f) {

			time_low += Time.deltaTime;

			if (time_low > 1.0f) {
				Destroy (gameObject);
			}
		} else {
			time_low = 0.0f;
		}

	}
}
