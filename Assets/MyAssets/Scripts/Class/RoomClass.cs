using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomClass {

	public string name = "Piece";
	public Vector3 position;
	public HiddingPlaceClass[] hiddingPlaces;

	// Use this for initialization
	public void Start () {
		for (int i = 0; i < hiddingPlaces.Length; i++) {
			hiddingPlaces [i].Start ();
		}
	}
	
	// Update is called once per frame
	public void Update () {
		for (int i = 0; i < hiddingPlaces.Length; i++) {
			hiddingPlaces [i].Update ();
		}
	}
}
