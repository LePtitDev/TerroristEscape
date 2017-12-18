using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State{
	None, Keyboard, CallIn, CallOut 
}

public class PhoneScript : MonoBehaviour {

	public State state;

	public GameObject phone_base;
	public GameObject phone_keyboard;
	public GameObject phone_callIn;
	public GameObject phone_callOut;

	public GameObject phone_text_keyboard;
	public GameObject phone_text_calling;

	public string number = "";
	private bool phone = false;

	private NetworkManager _network;

	// Use this for initialization
	void Start () {
		state = State.None;
		_network = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.T))
			Global.phoneRing = true;

		if (GameObject.Find ("Server(Clone)") && GameObject.Find ("Server(Clone)").GetComponent<Server> ().phone) {
			GameObject.Find ("Server(Clone)").GetComponent<Server> ().phone = false;
			Global.phoneRing = true;
		}

		if (Global.phoneRing && !phone) {
			state = State.CallIn;
			phone = !phone;
			Global.hideCursor = !phone;
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			phone = !phone;
			Global.hideCursor = !phone;
		}

		if (phone && state == State.None) {
			state = State.Keyboard;
		}
		if (!phone && state != State.None) {
			state = State.None;
		}

		bool canActivePhone = !_network.useNetwork || (_network.useNetwork && PhotonNetwork.isMasterClient);

		phone_base.SetActive (state != State.None && canActivePhone);
		phone_keyboard.SetActive (state == State.Keyboard && canActivePhone);
		phone_callIn.SetActive (state == State.CallIn && canActivePhone);
		phone_callOut.SetActive (state == State.CallOut && canActivePhone);

		phone_text_keyboard.GetComponent<Text> ().text = number;
		phone_text_calling.GetComponent<Text> ().text = number;
	}

	public void Press0(){
		if (number.Length < 10)
			number = number + "0";
	}

	public void Press1(){
		if (number.Length < 10)
			number = number + "1";
	}

	public void Press2(){
		if (number.Length < 10)
			number = number + "2";
	}

	public void Press3(){
		if (number.Length < 10)
			number = number + "3";
	}

	public void Press4(){
		if (number.Length < 10)
			number = number + "4";
	}

	public void Press5(){
		if (number.Length < 10)
			number = number + "5";
	}

	public void Press6(){
		if (number.Length < 10)
			number = number + "6";
	}

	public void Press7(){
		if (number.Length < 10)
			number = number + "7";
	}

	public void Press8(){
		if (number.Length < 10)
			number = number + "8";
	}

	public void Press9(){
		if (number.Length < 10)
			number = number + "9";
	}

	public void PressDelete(){
		if (number.Length > 0)
			number = number.Substring (0, number.Length - 1);
	}

	public void PressCall(){
		if (number.Length > 0)
		{
			state = State.CallOut;
			if (number == "15" ||
			    number == "17" ||
			    number == "18" ||
			    number == "112")
				GameObject.FindObjectOfType<RescueTimer>().StartTimer();
		}
	}

	public void PressReject(){
		state = State.Keyboard;
		Global.phoneRing = false;
	}
}
