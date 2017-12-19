using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCamera : MonoBehaviour
{

    private GameObject lobbyCamera;
    private NetworkManager _network;

    // Use this for initialization
    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        lobbyCamera = GameObject.Find("LobbyCamera");
    }

    // Update is called once per frame
    void Update()
    {
		if (PhotonNetwork.inRoom || !_network.useNetwork) {
			bool isClient = _network.useNetwork && PhotonNetwork.inRoom && !PhotonNetwork.isMasterClient;

			if (isClient) {
				lobbyCamera.SetActive (false);
			}
			gameObject.transform.GetChild (0).gameObject.SetActive (isClient);
			AudioListener al = gameObject.GetComponent<AudioListener> ();
			if (al)
				al.enabled = isClient;
			Vuforia.VuforiaBehaviour vb = gameObject.GetComponent<Vuforia.VuforiaBehaviour> ();
			if (vb)
				vb.enabled = isClient;
			Vuforia.DefaultInitializationErrorHandler dieh = gameObject.GetComponent<Vuforia.DefaultInitializationErrorHandler> ();
			if (dieh)
				dieh.enabled = isClient;
		}
    }
}