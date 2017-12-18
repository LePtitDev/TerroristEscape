using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	private NetworkManager _network;

	public GameObject terroristSpawn;
	public GameObject serverScript;

	private bool startTerroristTimer = false;
	private float timeBeforeTerroristSpawn = 5.0f;

	// Use this for initialization
	void Start () {
		_network = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();

		if (_network.useNetwork == false) {
			Instantiate (_network.prefabPlayer, new Vector3 (16.42f, 1.0f, -1.13f), Quaternion.identity);
			//Instantiate (serverScript, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			startTerroristTimer = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (startTerroristTimer && Global.animationEnded) {
			if (timeBeforeTerroristSpawn > 0) {
				timeBeforeTerroristSpawn -= Time.deltaTime;
			} else {
				if (_network.useNetwork == false) {
					Instantiate (_network.prefabTerrorist, terroristSpawn.transform.position, Quaternion.identity);
				} else {
					PhotonNetwork.Instantiate (_network.prefabTerrorist.name, terroristSpawn.transform.position, Quaternion.identity, 0);
				}
				startTerroristTimer = false;
			}
		}
	}

	void OnCreatedRoom(){
		PhotonNetwork.Instantiate (_network.prefabPlayer.name, new Vector3 (16.42f, 1.0f, -1.13f), Quaternion.identity, 0);
		//PhotonNetwork.Instantiate (serverScript.name, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
		startTerroristTimer = true;
	}
}
