using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptIntro : MonoBehaviour {

	private NetworkManager _network;

	private GameObject Player;
	private GameObject _camera;
	public GameObject cameraPosition;
	public GameObject cameraTarget;

	private bool started = false;
	// Use this for initialization
	void Start () {
		//Player.SetActive (false);
		_network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		_camera = GameObject.Find ("LobbyCamera");

		GetComponent<Animator> ().enabled = false;
	}

	void Update(){

		if( Player == null )
			Player = GameObject.Find("Server_FPS(Clone)");

		if (Player != null && (_network.useNetwork && PhotonNetwork.isMasterClient || !_network.useNetwork))
			StartIntro ();

		if (started) {
			_camera.transform.position = cameraPosition.transform.position;
			_camera.transform.LookAt (cameraTarget.transform.position);
		}
	}

	// Update is called once per frame
	public void StartIntro () {
		Player.GetComponent<MoveFPS> ().enabled = false;
		GetComponent<Animator> ().enabled = true;
		started = true;
	}

	// Update is called once per frame
	public void EndIntro () {
		this.gameObject.SetActive (false);
		Player.GetComponent<MoveFPS> ().enabled = true;
		started = false;

	}
}
