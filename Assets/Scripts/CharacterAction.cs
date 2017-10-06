using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour {

	public float length = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.E)) {

			Vector3 fwd = transform.TransformDirection (Vector3.forward);

			RaycastHit hit;
			Physics.Raycast (transform.position, fwd, out hit, length);

			if (hit.collider != null && hit.collider.gameObject.CompareTag ("Tag_Door")) {
				Doors d = hit.collider.gameObject.GetComponent<Doors> ();
				if (d != null)
					d.Action (gameObject);
				else {
					if (hit.collider.gameObject.transform.childCount > 0) {
						d = hit.collider.gameObject.transform.GetChild (0).gameObject.GetComponent<Doors> ();
						if (d != null) {
							d.Action (gameObject);
						}
					}
				}
			}

		}

	}
}
