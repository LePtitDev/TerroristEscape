using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public GameObject[] forMaster;
	public GameObject[] forClient;

	public GameObject[] steps;
	public GameObject clientOnlyColliders;

	private Text text;
	private NetworkManager _network;

	private int maxStep = 0;
	private int currentStep = 0;

	private bool cursor = false;

	public GameObject prefabVictory;
	private GameObject victory = null;

	// Use this for initialization
	void Start () {
		_network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		text = GameObject.Find ("TextNetworkInformations").GetComponent<Text> ();

		maxStep = steps.Length - 1;
		currentStep = maxStep;

		Global.hideCursor = !cursor;
	}
	
	// Update is called once per frame
	void Update () {

		if (Global.victory && victory == null && prefabVictory!=null)
			victory = Instantiate (prefabVictory, Global.canvas.transform);

		cursor = !Global.hideCursor;

		bool isInRoom = PhotonNetwork.inRoom && _network.useNetwork;
		bool isClient = !PhotonNetwork.isMasterClient && isInRoom;
		bool isMaster = PhotonNetwork.isMasterClient && isInRoom || !_network.useNetwork;

		foreach(GameObject g in forMaster)
			g.SetActive (isMaster);
		foreach(GameObject g in forClient)
			g.SetActive (isClient);

		//forMaster.SetActive (isMaster);
		//forClient.SetActive (isClient);
		clientOnlyColliders.SetActive (isClient);

		text.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString () + ((PhotonNetwork.isMasterClient)?" (Master)":" (Client)");

		for (int i = 0; i < steps.Length; i++) {
			steps [i].SetActive (i <= currentStep);
		}

		if (PhotonNetwork.inRoom && !PhotonNetwork.isMasterClient)
			cursor = true;

		Cursor.visible = cursor;
		if (cursor)
			Cursor.lockState = CursorLockMode.None;
		else
			Cursor.lockState = CursorLockMode.Locked;

		if (Input.GetKeyDown (KeyCode.Escape)) {
			cursor = !cursor;
			Global.hideCursor = !cursor;
		}
	}

	public void OnButtonUp(){
		currentStep++;
		if (currentStep > maxStep)
			currentStep = maxStep;
	}

	public void OnButtonDown(){
		currentStep--;
		if (currentStep < 0)
			currentStep = 0;
	}

	public void OnPhone(){

		GameObject server = GameObject.Find ("Server(Clone)");

		if (server != null)
			server.GetComponent<Server>().Press ();
	}
}
