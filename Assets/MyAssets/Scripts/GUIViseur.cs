using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIViseur : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		GUI.Label( new Rect(Screen.width / 2.0f , Screen.height / 2.0f , 200,200),"+");
	}
}
