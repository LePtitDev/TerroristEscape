using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour {

	public Camera m_camera;

	private Camera camera_actionner = null;
	private Activable activable;
	private bool isActionEnded = true;
	private float timeActual = 0.0f;
	private float timeBeforeNextAction = 2.0f;
	private bool nobodyInside = true;

	private GameObject playerController = null;
	private GameObject _camera;

	private Vector3 previousPosition;

	// Use this for initialization
	void Start () {
		activable = gameObject.GetComponent<Activable> ();
		timeActual = timeBeforeNextAction;

		if (activable == null)
			Debug.Log ("ChangeCamera script need Activable script!");

		_camera = GameObject.Find ("LobbyCamera");
	}
	
	// Update is called once per frame
	void Update () {

		if(playerController == null)
			playerController = GameObject.Find ("Server_FPS(Clone)");

		if (timeActual < timeBeforeNextAction)
			timeActual += Time.deltaTime;
		
		if ((activable.isOpened () || activable.isClosing ()) && timeActual >= timeBeforeNextAction && playerController != null) {


			if (nobodyInside) {
				_camera.transform.position = m_camera.transform.position;
				_camera.transform.rotation = m_camera.transform.rotation;

				playerController.GetComponent<MoveFPS> ().enabled = false;

				previousPosition = playerController.transform.position;
				playerController.transform.position = m_camera.transform.position - Vector3.up * 0.5f;
			} else {
				playerController.transform.position = previousPosition;
				playerController.GetComponent<MoveFPS> ().enabled = true;
			}

			nobodyInside = !nobodyInside;
			timeActual = 0.0f;
		}
	}

	public bool isPlayerInside(){
		return !nobodyInside;
	}
}
