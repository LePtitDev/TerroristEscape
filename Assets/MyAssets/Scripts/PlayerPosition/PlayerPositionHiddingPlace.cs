using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionHiddingPlace : MonoBehaviour {

    public Vector3 Position;

	public GameObject hiddingPlace;

	private float fitness_temp = 0.0f;
	private GameObject soundParticleSystem = null;

	private float time_fitness_temp_sound = 2.0f;
	private float value_fitness_temp_sound = 5.0f;

	private Locker locker;
	private Activable activable;

	// Use this for initialization
	void Start ()
    {
        Position = transform.position;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Position")
            {
                Position = transform.GetChild(i).transform.position;
                Destroy(transform.GetChild(i).gameObject);
                break;
            }
        }
        locker = hiddingPlace.GetComponent<Locker> ();
		activable = hiddingPlace.GetComponent<Activable> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (activable.isClosing () || activable.isOpening()){
			fitness_temp = time_fitness_temp_sound;
			if (soundParticleSystem == null) {
				soundParticleSystem = Instantiate (Global.soundEffect, hiddingPlace.transform.position, Quaternion.identity);
			}
		}

		if (fitness_temp > 0) {
			fitness_temp -= Time.deltaTime;
		} else if (fitness_temp < 0) {
			fitness_temp = 0;
		}

		if(soundParticleSystem != null && fitness_temp == 0)
			Destroy (soundParticleSystem);
	}

	public float getFitnessTemp(){
		return ((fitness_temp > 0)?value_fitness_temp_sound:0);
	}

	public float getFitnessTotal(){
		return getFitnessTemp ();
	}

}
