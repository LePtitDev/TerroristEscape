using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFPS : MonoBehaviour {

	public float movementSpeed = 2.0f;
	public float horizontalMouseSpeed = 2.0f;
	public float vertiaclMouseSpeed = 2.0f;

	public GameObject cameraPosition;
	public GameObject cameraTarget;

	private NetworkManager _network;
	private PhotonView _view;

	private float pitch = 0;
	private float yaw = 0;
	private Vector3 localPositionTarget;
	private GameObject _camera;

	// Use this for initialization
	void Start () {
		_network = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();
		_view = GetComponent<PhotonView> ();

		localPositionTarget = cameraTarget.transform.localPosition;

		_camera = GameObject.Find ("LobbyCamera");
	}
	
	// Update is called once per frame
	void Update () {

		float move_h = Input.GetAxis ("Horizontal");
		float move_v = Input.GetAxis ("Vertical");

		//transform.position += new Vector3 (move_h, 0, move_v) * Time.deltaTime * movementSpeed;
		transform.Translate(new Vector3 (move_h, 0, move_v) * Time.deltaTime * movementSpeed);

		float mouse_x = Input.GetAxisRaw ("Mouse X") * Time.deltaTime * horizontalMouseSpeed;
		float mouse_y = Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * vertiaclMouseSpeed;

		pitch += mouse_x * 180.0f / Mathf.PI;
		yaw += mouse_y * 180.0f / Mathf.PI;

		if (yaw >= 89)
			yaw = 89;
		if (yaw <= -89)
			yaw = -89;

		float x = Mathf.Cos (yaw * Mathf.PI / 180.0f);
		float y = Mathf.Sin (yaw * Mathf.PI / 180.0f);

		transform.localEulerAngles = new Vector3 (0, pitch, 0);

		cameraTarget.transform.localPosition = localPositionTarget + new Vector3 (0,y,x);

		_camera.transform.position = cameraPosition.transform.position;
		_camera.transform.LookAt (cameraTarget.transform.position);
	}
}
