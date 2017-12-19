using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RescueTimer : MonoBehaviour
{
	
	// Timer panel
	private GameObject _panel;

	// Timer text
	private Text _textTimer;

	// Indicate if timer is started
	private bool _started;

	// Timer started
	private float _begin;

	/// <summary>
	/// Timer duration
	/// </summary>
	public float Duration;
	
	/// <summary>
	/// Event called when timer is finished
	/// </summary>
	[SerializeField]
	public UnityEvent OnRescue;

	/// <summary>
	/// Time release
	/// </summary>
	public float TimeRelease
	{
		get
		{
			if (!_started)
				return Duration;
			if (_begin + Duration <= Time.time)
				return 0f;
			return _begin + Duration - Time.time;
		}
	}

	// Use this for initialization
	private void Start () {
		foreach (Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == "RescueTimer")
			{
				_panel = t.gameObject;
				break;
			}
		}
		foreach (Text t in _panel.GetComponentsInChildren<Text>())
		{
			if (t.name == "Timer")
			{
				_textTimer = t;
				break;
			}
		}
		_panel.SetActive(false);
	}
	
	// Update is called once per frame
	private void Update () {

		Global.timeLeft = 0;
		Global.duration = Duration;

		if(!_started)
			Global.timeLeft = Duration;

		if (!_started)
			return;
		if (_begin + Duration <= Time.time)
		{
			_textTimer.text = "00:00:00";
			if (OnRescue != null)
				OnRescue.Invoke();
			return;
		}
		float release = _begin + Duration - Time.time;
		int min = ((int) release / 60),
			sec = ((int) release % 60),
			mil = ((int) (release * 100f) % 100);
		_textTimer.text = (min < 10 ? "0" + min.ToString() : min.ToString()) + ":" +
		                  (sec < 10 ? "0" + sec.ToString() : sec.ToString()) + ":" +
			(mil < 10 ? "0" + mil.ToString() : mil.ToString());

		Global.timeLeft = release;
		Global.rescueCalled = true;
	}

	/// <summary>
	/// Begin the rescue timer
	/// </summary>
	public void StartTimer()
	{
		_started = true;
		_begin = Time.time;
		_panel.SetActive(true);
	}
}
