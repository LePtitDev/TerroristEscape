using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScreen : MonoBehaviour {

	public int materialIndex;
	public Material[] materialsList;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();

		rend.materials [materialIndex].CopyPropertiesFromMaterial( materialsList [Random.Range (0, materialsList.Length)]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
