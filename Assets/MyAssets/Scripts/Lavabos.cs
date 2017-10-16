using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavabos : MonoBehaviour {

	public float speed = 10.0f;
	public GameObject movableChild;
	public ParticleSystem waterParticuleEmitter;

	public float amplitude = 90.0f;

	private float angle = 0.0f;
	private bool needAction = false;
	private bool actionEnded = true;
	private bool needOpen = true;

	private Vector3 previous_position;

	// Use this for initialization
	void Start () {
		previous_position = new Vector3 (0,
			movableChild.transform.localEulerAngles.y,
			movableChild.transform.localEulerAngles.z+(Random.value*45.0f)-22.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!actionEnded) {
			if (needOpen) {
				if (angle >= amplitude) {
					actionEnded = true;
					angle = amplitude;
				} else {
					angle += Time.deltaTime * speed;
					if (!waterParticuleEmitter.isPlaying)
						waterParticuleEmitter.Play ();
				}
			} else {
				if (angle <= 0.0f) {
					actionEnded = true;
					angle = 0.0f;
				} else {
						angle -= Time.deltaTime * speed;
						if (!waterParticuleEmitter.isStopped)
							waterParticuleEmitter.Stop ();
				}
			}
		}
		movableChild.transform.localEulerAngles = previous_position + new Vector3 (-angle,0,0);

		if (angle == 0.0f) {
			if (!waterParticuleEmitter.isStopped) {
				waterParticuleEmitter.Stop ();
			}
		} else if (angle == amplitude){
			if (!waterParticuleEmitter.isPlaying) {
				waterParticuleEmitter.Play ();
			}
		}
	}

	public void Action(){
		if (actionEnded) {
			actionEnded = false;
			needOpen = (angle == 0.0f);
		}
	}
}
