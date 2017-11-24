using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	private NetworkManager _network;

	// Use this for initialization
	void Start () {
		_network = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();

		if (_network.useNetwork == false) {
			Instantiate (_network.prefabPlayer, new Vector3 (16.42f, 1.0f, -1.13f), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCreatedRoom(){
		PhotonNetwork.Instantiate (_network.prefabPlayer.name, new Vector3 (16.42f, 1.0f, -1.13f), Quaternion.identity, 0);
	}
}
