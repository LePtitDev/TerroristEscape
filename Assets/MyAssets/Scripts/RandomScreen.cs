using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScreen : MonoBehaviour {

	public int materialIndex;
	public Material[] materialsList;

	public Material BlackScreen;

	private Renderer rend;
	private Activable activable;
	private bool restart = false;
	private int index = 0;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();

		index = Random.Range (0, materialsList.Length);
		rend.materials [materialIndex].CopyPropertiesFromMaterial( materialsList [index]);

		activable = gameObject.GetComponent<Activable> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (activable != null) {
			if (activable.isOpened ()) {
				restart = true;
				rend.materials [materialIndex].CopyPropertiesFromMaterial (BlackScreen);

			} else if (restart) {
				restart = false;
				rend.materials [materialIndex].CopyPropertiesFromMaterial (materialsList [index]);
			}
		}
	}
}
