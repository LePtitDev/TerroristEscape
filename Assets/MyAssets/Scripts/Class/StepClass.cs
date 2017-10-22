using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StepClass {

	public string name = "Etage";
	public Vector3 position;
	public RoomClass[] rooms;

	// Use this for initialization
	public void Start () {
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i].Start ();
		}
	}
	
	// Update is called once per frame
	public void Update () {
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i].Update ();
		}
	}
}
