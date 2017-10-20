using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour {

	// Animation
	public GameObject movableObject;
	public float amplitudeMax = 0.0f;
	public Vector3 direction;
	public float movementSpeed = 1.0f;
	public bool movementIsRotation = false;
	public bool startClose = true;
	public bool autoClose = false;

	// Particules
	public ParticleSystem particuleSystem;
	public bool activeWhenOpened;

	public bool debug = false;

	//Sons
	/*
	public ParticleSystem particuleSystem;
	public bool activeWhenOpened;*/

	private float amplitudeActual = 0.0f;
	private bool needOpen = true;
	private bool isActionEnded = true;
	private Vector3 initialPosition;
	private Vector3 initialLocalEulerAngles;
	private float amplitudeMaxAbs = 1.0f;
	private float amplitudeMaxSigne = 1.0f;
	private bool isOpen = false;
	private bool isClose = false;

	private GameObject lastActionner;

	void Start () {
		if (movableObject) {
			initialPosition = movableObject.transform.position;
			initialLocalEulerAngles = movableObject.transform.localEulerAngles;
		}

		amplitudeMaxAbs = Mathf.Abs (amplitudeMax);
		amplitudeMaxSigne = ((amplitudeMax > 0) ? 1.0f : -1.0f);

		if (!startClose) {
			amplitudeActual = amplitudeMaxAbs;
			needOpen = false;

			isOpen = true;
			isClose = false;
		} else {
			isOpen = false;
			isClose = true;
		}

		if (activeWhenOpened) {
			if(particuleSystem && isOpen && !particuleSystem.isPlaying)
				particuleSystem.Play ();
			if(particuleSystem && isClose && !particuleSystem.isStopped)
				particuleSystem.Stop ();
		} else {
			if(particuleSystem && isClose && !particuleSystem.isPlaying)
				particuleSystem.Play ();
			if(particuleSystem && isOpen && !particuleSystem.isStopped)
				particuleSystem.Stop ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(movableObject == null && autoClose && isOpen) {
			isOpen = !isOpen;
			isClose = !isClose;
		}
			

		if (!isActionEnded) {
			if (needOpen) {
				if (amplitudeActual >= amplitudeMaxAbs) {
					isActionEnded = true;
					amplitudeActual = amplitudeMaxAbs;
					isOpen = true;
					isClose = false;
					if (autoClose)
						Action (lastActionner);

				} else {
					amplitudeActual += Time.deltaTime * movementSpeed;
				}
			} else {
				if (amplitudeActual <= 0.0f) {
					isActionEnded = true;
					amplitudeActual = 0.0f;
					isClose = true;
					isOpen = false;
				} else {
					amplitudeActual -= Time.deltaTime * movementSpeed;
				}
			}
		}

		if (movableObject) {
			if (movementIsRotation)
				movableObject.transform.localEulerAngles = initialLocalEulerAngles + amplitudeMaxSigne * new Vector3 (direction.x * amplitudeActual, direction.y * amplitudeActual, direction.z * amplitudeActual);
			else
				movableObject.transform.position = initialPosition + amplitudeMaxSigne * new Vector3 (direction.x * amplitudeActual, direction.y * amplitudeActual, direction.z * amplitudeActual);
		}

		if (activeWhenOpened) {
			if (particuleSystem && !isClose && needOpen && !particuleSystem.isPlaying)
				particuleSystem.Play ();
			if (particuleSystem && !isOpen && !needOpen && !particuleSystem.isStopped)
				particuleSystem.Stop ();
		} else {
			if (particuleSystem && !isOpen && !needOpen && !particuleSystem.isPlaying)
				particuleSystem.Play ();
			if (particuleSystem && !isClose && needOpen && !particuleSystem.isStopped)
				particuleSystem.Stop ();
		}
	}

	public void Action(GameObject other) {
		lastActionner = other;

		if (debug)
			Debug.Log ("Action " + gameObject.name);

		if (isActionEnded) {

			if (movableObject == null) {
				isOpen = !isOpen;
				isClose = !isClose;
			} else {
				isActionEnded = false;
				needOpen = (amplitudeActual == 0.0f);
			}
		}
	}

	public bool isOpened(){
		return isOpen;
	}

	public bool isClosed(){
		return isClose;
	}

	public bool isOpening(){
		return needOpen && isOpen == false && isClose == false;
	}

	public bool isClosing(){
		return !needOpen && isOpen == false && isClose == false;
	}

	public GameObject getLastActionner(){
		return lastActionner;
	}
}
