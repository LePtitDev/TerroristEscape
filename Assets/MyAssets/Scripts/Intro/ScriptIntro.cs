using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptIntro : MonoBehaviour {

	public GameObject Player;
	// Use this for initialization
	void Start () {
		Player.SetActive (false);
		
	}
	
	// Update is called once per frame
	public void EndIntro () {
		Player.SetActive (true);
		GetComponent<Camera> ().depth = -1;
		this.gameObject.SetActive (false);
	}
}
