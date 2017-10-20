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

		if (!gameObject.GetComponent<Camera> ().enabled)
			return;

		if (Input.GetKeyDown (KeyCode.E)) {

			Vector3 fwd = transform.TransformDirection (Vector3.forward);

			RaycastHit[] hits = Physics.RaycastAll (transform.position, fwd, length);

			for (int i = 0; i < hits.Length; i++) {
				RaycastHit hit = hits[i];
				
				// Si on rencontre une porte
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

				// Si on rencontre un objet activable
				if (hit.collider != null) {
					GameObject go = hit.collider.gameObject;

					Activable a = go.GetComponent<Activable> ();
					if (!a && go.transform.parent != null) {
						go = go.transform.parent.gameObject;
						a = go.GetComponent<Activable> ();
					}

					if (a) {
						a.Action (gameObject);
					}
				}
			}
		}
	}
}
