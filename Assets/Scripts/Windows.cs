using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : MonoBehaviour {

	public float speed = 1.0f;
	public GameObject movableGameObject;
	public float amplitude = 0.75f;
	private float position_up = 0.0f;

	private bool needAction = false;
	private bool actionEnded = true;
	private bool needOpen = true;

	private Vector3 previous_position;

	// Use this for initialization
	void Start () {
		previous_position = movableGameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (!actionEnded) {
			if (needOpen) {
				if (position_up >= amplitude) {
					actionEnded = true;
					position_up = amplitude;
				} else {
					position_up += Time.deltaTime * speed;
				}
			} else {
				if (position_up <= 0.0f) {
					actionEnded = true;
					position_up = 0.0f;
				} else {
					position_up -= Time.deltaTime * speed;
				}
			}
		}

		movableGameObject.transform.position = previous_position + new Vector3 (0,position_up,0);
	}

	public void Action(){
		if (actionEnded) {
			actionEnded = false;
			needOpen = (position_up == 0.0f);
		}
	}
}
