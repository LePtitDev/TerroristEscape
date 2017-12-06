using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pair {
	public GameObject activable;
	public bool needOpen = false;
}

public class PlayerPositionRoom : MonoBehaviour {

    public Vector3 Position;

	public Pair[] activablesObjectsToTurnOff;

	private float fitness = 0.0f;
	private float fitness_temp = 0.0f;

	private float value_fitness_playerInRoom = 5.0f;
	private float value_fitness_playerRunning = 10.0f;
	private float value_fitness_notTurnedOff = 2.0f;

	private bool playerInRoom = false;


	private float[] tab_fitnessTemp;
	private float[] tab_weights;
	private float raison = 0.5f;

	// Use this for initialization
	void Start ()
    {
        int cm = 0;
        Position = transform.position;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Position")
            {
                Position = transform.GetChild(i).transform.position;
                Destroy(transform.GetChild(i).gameObject);
                cm++;
                break;
            }
        }
        tab_fitnessTemp = new float[transform.childCount - cm];
        tab_weights = new float[transform.childCount - cm];
    }
	
	// Update is called once per frame
	void Update () {
		//if (playerInRoom)
		//	Debug.Log ( "Player in " + gameObject.name + " at " + transform.parent.gameObject.name + " " + getFitnessTotal());

		bool playerRunning = (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton8));
		fitness = ((playerInRoom) ? value_fitness_playerInRoom : 0) + ((playerRunning) ? value_fitness_playerRunning : 0);

		for (int i = 0; i < activablesObjectsToTurnOff.Length; i++) {
			fitness += ((activablesObjectsToTurnOff [i].activable.GetComponent<Activable> ().isClosed () != activablesObjectsToTurnOff [i].needOpen) ? value_fitness_notTurnedOff : 0);
		}

		fitness_temp = 0.0f;

		for (int i = 0; i < transform.childCount; i++) {
            PlayerPositionHiddingPlace pp = transform.GetChild(i).gameObject.GetComponent<PlayerPositionHiddingPlace>();
            tab_fitnessTemp[i] = pp.getFitnessTotal();
            fitness_temp += pp.getFitnessTemp();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject == Global.controller)
			playerInRoom = true;
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject == Global.controller)
			playerInRoom = true;
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject == Global.controller)
			playerInRoom = false;
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
		for (int i = 0; i < tab_fitnessTemp.Length; i++) {
			tab_weights [i] = -1;
		}

		float w = 1.0f;
		for (int i = 0; i < tab_fitnessTemp.Length; i++) {
			int indexmax = 0;
			for (int j = 0; j < tab_fitnessTemp.Length; j++) {
				if (tab_weights [j] == -1 && tab_fitnessTemp [indexmax] < tab_fitnessTemp [j] || tab_weights[indexmax] != -1) {
					indexmax = j;
				}
			}
			if (tab_weights [indexmax] == -1) {
				tab_weights [indexmax] = w;
				w *= raison;
			}
		}
	}

	public GameObject getMaxFitnessHiddingPlace(){
		calculateweights ();
		for (int i = 0; i < tab_fitnessTemp.Length; i++) {
			if (tab_weights [i] == 1.0f)
				return transform.GetChild (i).gameObject;
		}
		return null;
	}

	/**
	 * 
	 * Warning! Can be null if the room doesn't contain any hiddingPlace
	 * 
	 * */
	public GameObject getNextHiddingPlace(){
		calculateweights ();
		float som_weight = 1.0f * (1.0f - Mathf.Pow(raison,transform.childCount)) / (1.0f - raison);
		float r = Random.value * som_weight;

		for (int i = 0; i < tab_fitnessTemp.Length; i++) {
			if (tab_weights [i] > r) {
				return transform.GetChild (i).gameObject;
			} else {
				r -= tab_weights [i];
			}
		}
		return null;
	}
}
