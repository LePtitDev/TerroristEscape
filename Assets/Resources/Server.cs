using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour {

	public bool phone;

	private PhotonView view;

	// Use this for initialization
	void Start () {
		view = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (phone) {
			view.RPC ("SyncPhone", PhotonTargets.All);
			phone = false;
		}
	}
	/*
	[PunRPC]
	public void Test(){
		if (phone) {
			view.RPC ("SyncPhone", PhotonTargets.All);
			phone = false;
		}
	}*/

	public void Press(){
		phone = true;
	}

	[PunRPC]
	public void SyncPhone(){
		Global.phoneRing = true;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext (phone);
		}
		else
		{
			phone = (bool)stream.ReceiveNext ();
		}
	}
}
