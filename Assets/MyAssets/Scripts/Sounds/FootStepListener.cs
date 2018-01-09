using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepListener : MonoBehaviour {

	public float distanceMax = 20f;
	private GameObject terrorist = null;
	private float distance = 0;

	private bool _terroristInRaycast = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (terrorist == null)
			terrorist = GameObject.Find ("Terrorist(Clone)");

		if (terrorist != null) {
			RaycastHit[] hits;
			hits = Physics.RaycastAll(transform.position, terrorist.transform.position-transform.position, distanceMax);
			_terroristInRaycast = false;
			if (hits.Length > 0 && hits [0].collider.gameObject.name == "Terrorist(Clone)")
				_terroristInRaycast = true;

			distance = (transform.position - terrorist.transform.position).magnitude / distanceMax;

			if (_terroristInRaycast)
				distance *= 2;

		} else {
			distance = distanceMax + 1;
		}
	}

	public float getVolume(){
		return (distance <= 1f) ? 1f - distance : 0;
	}
}
