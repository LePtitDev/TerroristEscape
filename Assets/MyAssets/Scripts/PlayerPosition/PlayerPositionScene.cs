﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionScene : MonoBehaviour {
    
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
		for (int i = 0; i < tab_fitness.Length; i++) {
            PlayerPositionStep ps = transform.GetChild(i).gameObject.GetComponent<PlayerPositionStep>();
            if (ps != null)
                tab_fitness [i] = ps.getFitnessTotal ();
		}
		//*
		GameObject step = getNextStep ();
		GameObject room = step.GetComponent<PlayerPositionStep> ().getNextRoom ();
		//GameObject hidd = room.GetComponent<PlayerPositionRoom> ().getNextHiddingPlace ();
		//Debug.Log ( ((hidd!=null)?hidd.name:"null") + " in the room " + room.name + " at step " + step.name );
		//Debug.Log ( "Room " + room.name +"(" + room.GetComponent<PlayerPositionRoom> ().getFitnessTotal() + ")" + " at step " + step.name + " (" + step.GetComponent<PlayerPositionStep> ().getFitnessTotal() + ")" );//*/
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

	public GameObject getMaxFitnessStep(){
		calculateweights ();
		for (int i = 0; i < tab_fitness.Length; i++) {
			if (tab_weights [i] == 1.0f)
				return transform.GetChild (i).gameObject;
		}
		return null;
	}


	/**
	 * 
	 * Warning! Can be null if the scene doesn't contain any step
	 * 
	 * */
	public GameObject getNextStep(){
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
