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

	// Use this for initialization
	void Start () {
		activable = gameObject.GetComponent<Activable> ();
		timeActual = timeBeforeNextAction;

		if (activable == null)
			Debug.Log ("ChangeCamera script need Activable script!");
	}
	
	// Update is called once per frame
	void Update () {
		if ((activable.isOpened () || activable.isClosing ()) && timeActual >= timeBeforeNextAction) {

			if (nobodyInside) {
				camera_actionner = activable.getLastActionner ().GetComponent<Camera> ();
				playerController = activable.getLastActionner ().transform.parent.gameObject;
			}

			camera_actionner.enabled = !camera_actionner.enabled;
			m_camera.enabled = !m_camera.enabled;

			playerController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = camera_actionner.enabled;
			playerController.GetComponent<CharacterController> ().enabled = camera_actionner.enabled;

			nobodyInside = !nobodyInside;
			timeActual = 0.0f;
		}

		if (timeActual < timeBeforeNextAction)
			timeActual += Time.deltaTime;
	}

	public bool isPlayerInside(){
		return !nobodyInside;
	}
}
