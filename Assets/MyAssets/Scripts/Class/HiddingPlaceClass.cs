using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HiddingPlaceClass {

	public string name = "Cachette";
	public GameObject target;

	private float distance = 1.0f;
	private float fitness_temp_sound = 0.1f;
	private float fitness_temp_phone = 50.0f;
	private float fitness_temp_checked = -20.0f;

	private float fitness = 0.0f;
	private float fitness_temp = 0.0f;
	private Activable activable;
	private bool added = false;
	private GameObject instanceOfSoundEffect = null;

	// Use this for initialization
	public void Start () {
		activable = target.GetComponent<Activable> ();

		if (activable == null)
			Debug.Log ("Need Activable");
	}
	
	// Update is called once per frame
	public void Update () {

		if (activable != null) {
			if (!added && (activable.isClosing () || activable.isOpening ())) {	// Si la porte du placard bouge
				fitness_temp += fitness_temp_sound;
				added = true;

				if (instanceOfSoundEffect == null) {
					instanceOfSoundEffect = UnityEngine.Object.Instantiate(Global.soundEffect,target.transform.position,Quaternion.identity);
				}

			} else {
				added = false;
			}
		}

		if (fitness_temp > 0)
			fitness_temp -= Time.deltaTime;
		if (fitness_temp <= 0) {
			fitness_temp = 0;
			if (instanceOfSoundEffect != null) {
				UnityEngine.Object.Destroy (instanceOfSoundEffect);
			}
		}
	}

	public float GetFitness(){
		return fitness;
	}

	public float GetFitnessTemp(){
		return fitness_temp;
	}

	public float GetFitnessTotal(){
		return fitness + fitness_temp;
	}
}
