using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

	public GameObject prefabPlayer;

	public bool useNetwork;

	// Use this for initialization
	void Start () {
		if(useNetwork)
			PhotonNetwork.ConnectUsingSettings ("v0.1");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Status: " + PhotonNetwork.connectionStateDetailed.ToString ());
	}

	public void OnJoinedLobby(){
		// Rejoindre une "Room" aléatoire
		PhotonNetwork.JoinRandomRoom ();
	}

	// Si aucune "Room" n'existe
	public void OnPhotonRandomJoinFailed(object[] codeAndMsg){
		// Créer une nouvelle "Room"
		PhotonNetwork.CreateRoom(null, new RoomOptions(){ MaxPlayers = 20}, null);
	}

	// Si on rejoins une "Room"
	public void OnJoinedRoom(){

	}
}
