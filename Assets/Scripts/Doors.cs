using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

	public float angle_max_right = -120.0f;
	public float angle_max_left	 = 120.0f;

	public bool openRight = true;
	public bool openLeft = true;
	public bool isLocked = false;

	public float angular_speed = 90.0f;
	public float openingTimeRatio = 0.5f;

	private bool actionEnded = true;
	private bool needAction = false;
	private bool needOpen = false;
	private bool needClose = false;
	private float angle = 0.0f;
	private bool left_true_right_false = false;

	private Vector3 angleBase;

	private float timePoignee = 1.0f;
	private float durationPoignee = 1.0f;
	private float anglePoignee = 0.0f;

	private GameObject character;

	// Use this for initialization
	void Start () {
		angleBase = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

		if( !openRight && !openLeft ){
			isLocked = true;
		}

		if (needClose) {
			if (left_true_right_false) { // door on left size (positive angle)
				angle -= angular_speed * Time.deltaTime;
				if (angle < 0) {
					angle = 0;
					needClose = false;
					actionEnded = true;
				}
			} else { // door on right size (negative angle)
				angle += angular_speed * Time.deltaTime;
				if (angle > 0) {
					angle = 0;
					needClose = false;
					actionEnded = true;
				}
			}
		}

		if (needOpen) {
			if (left_true_right_false) { // door on left size (positive angle)
				angle += angular_speed * Time.deltaTime;
				if (angle >= angle_max_left) {
					angle = angle_max_left;
					needOpen = false;
					actionEnded = true;
				}
			} else { // door on right size (negative angle)
				angle -= angular_speed * Time.deltaTime;
				if (angle < angle_max_right) {
					angle = angle_max_right;
					needOpen = false;
					actionEnded = true;
				}
			}
		}

		if (timePoignee < durationPoignee)
			timePoignee += Time.deltaTime;
		else
			timePoignee = durationPoignee;
		
		if (timePoignee >= durationPoignee * openingTimeRatio && timePoignee < durationPoignee && needAction) {
			DoorAction (character);
		}

		if (timePoignee < durationPoignee) {

			float u = ((isLocked)?2.0f:1.0f)*2.0f*Mathf.PI * timePoignee / durationPoignee;

			float a = 45.0f * (Mathf.Cos( u ) - 1.0f ) / 2.0f;

			transform.GetChild (0).localEulerAngles = new Vector3 (0, -a, 0);
		}
			
		transform.localEulerAngles = new Vector3 (angleBase.x,angleBase.y,angleBase.z+angle);
	}

	public void Action(GameObject go){
		if (timePoignee >= durationPoignee) {
			timePoignee = 0.0f;
			character = go;
			needAction = true;
		}
	}

	public void DoorAction(GameObject go){
		if (actionEnded) {
			actionEnded = false;
			needAction = false;

			if (angle != 0.0f) {
				needClose = true;
				needOpen = false;
			} else if(!isLocked){
				if (openLeft && openRight) {

					needOpen = true;
					needClose = false;

					Vector3 direction = transform.position - go.transform.position;
					float dir_angle_horizontal = Mathf.Atan2 (direction.z, direction.x) * 180.0f / Mathf.PI;

					dir_angle_horizontal += transform.eulerAngles.y;

					while (dir_angle_horizontal > 180)
						dir_angle_horizontal -= 360;
					while (dir_angle_horizontal < -180)
						dir_angle_horizontal += 360;

					left_true_right_false = dir_angle_horizontal > 0.0f;

				} else if (openLeft) {
					needOpen = true;
					needClose = false;
					left_true_right_false = true;
				} else if (openRight) {
					needOpen = true;
					needClose = false;
					left_true_right_false = false;
				}
			}
		}
	}

	public bool isClosed(){
		return (angle == 0.0f);
	}

	public bool isOpenned(){
		return (angle == angle_max_left || angle == angle_max_right);
	}
}
