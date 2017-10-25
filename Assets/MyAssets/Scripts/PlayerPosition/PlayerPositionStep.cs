using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionStep : MonoBehaviour {

	private float fitness = 0.0f;
	private float fitness_temp = 0.0f;

	private float[] tab_fitness;
	private float[] tab_weights;
	private float raison = 0.5f;

	// Use this for initialization
	void Start () {
		tab_fitness = new float[transform.childCount];
		tab_weights = new float[transform.childCount];
	}
	
	// Update is called once per frame
	void Update () {

		fitness = 0.0f;
		fitness_temp = 0.0f;

		for (int i = 0; i < transform.childCount; i++) {
			tab_fitness [i] = transform.GetChild (i).gameObject.GetComponent<PlayerPositionRoom> ().getFitnessTotal ();

			fitness += transform.GetChild (i).gameObject.GetComponent<PlayerPositionRoom> ().getFitness ();
			fitness_temp += transform.GetChild (i).gameObject.GetComponent<PlayerPositionRoom> ().getFitnessTemp ();
		}

	}

	public float getFitness(){
		return fitness;
	}

	public float getFitnessTemp(){
		return fitness_temp;
	}

	public float getFitnessTotal(){
		return fitness_temp + fitness;
	}

	void calculateweights(){
		for (int i = 0; i < tab_fitness.Length; i++) {
			tab_weights [i] = -1;
		}

		float w = 1.0f;
		for (int i = 0; i < tab_fitness.Length; i++) {
			int indexmax = 0;
			for (int j = 0; j < tab_fitness.Length; j++) {
				if (tab_weights [j] == -1 && tab_fitness [indexmax] < tab_fitness [j] || tab_weights[indexmax] != -1) {
					indexmax = j;
				}
			}
			if (tab_weights [indexmax] == -1) {
				tab_weights [indexmax] = w;
				w *= raison;
			}
		}
	}

	public GameObject getMaxFitnessRoom(){
		calculateweights ();
		for (int i = 0; i < tab_fitness.Length; i++) {
			if (tab_weights [i] == 1.0f)
				return transform.GetChild (i).gameObject;
		}
		return null;
	}

	/**
	 * 
	 * Warning! Can be null if the step doesn't contain any room
	 * 
	 * */
	public GameObject getNextRoom(){
		calculateweights ();
		float som_weight = 1.0f * (1.0f - Mathf.Pow(raison,transform.childCount)) / (1.0f - raison);
		float r = Random.value * som_weight;

		for (int i = 0; i < tab_fitness.Length; i++) {
			if (tab_weights [i] > r) {
				return transform.GetChild (i).gameObject;
			} else {
				r -= tab_weights [i];
			}
		}
		return null;
	}
}
