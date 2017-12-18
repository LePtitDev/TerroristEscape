using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class TerroristSpotter : PNJ_Controller {

    /// <summary>
    /// Position de départ du raycast par rapport à l'objet
    /// </summary>
    public GameObject RaycastStart;

    /// <summary>
    /// Angle de vue (en degrés)
    /// </summary>
    public float ViewAngle;

    /// <summary>
    /// Gameobject du joueur
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// Evenement déclenché lorsque le joueur est repéré
    /// </summary>
    [SerializeField]
    public UnityEvent<GameObject> OnSpotted;

    /// <summary>
    /// Evenement déclenché lorsque le joueur n'est plus visible
    /// </summary>
    [SerializeField]
    public UnityEvent<GameObject> OnLost;

    /// <summary>
    /// Nav mesh agent controller
    /// </summary>
    private NavMeshAgent navmesh;

    /// <summary>
    /// Next step to go
    /// </summary>
    private GameObject target_step;

    /// <summary>
    /// Next room to go
    /// </summary>
    private GameObject target_room;

    /// <summary>
    /// Next hidding place to go
    /// </summary>
    private GameObject target_hidding;

    protected override void Start() {
        base.Start();
        /*if (GameObject.Find("NetworkManager").GetComponent<NetworkManager>().useNetwork)
        {
            enabled = false;
            return;
        }*/
		
        //if (Player == null)
        //    Player = GameObject.Find("LobbyCamera");
        navmesh = GetComponent<NavMeshAgent>();
		_target = null;
        target_step = null;
        target_room = null;
        target_hidding = null;
    }

    // Update is called once per frame
    protected override void Update () {

		if (Player == null)
			Player = GameObject.Find ("TerroristTarget");

		if (_target != null) {
			foreach (Transform t in GetComponentsInChildren<Transform>()) {
				if (t.name == "Target") {
					t.position = _target.Value;
					break;
				}
			}
		}
		if (Player != null) {
			base.Update ();
		}
    }

    // Indicate if ignore collider
    private bool IgnoreCollider(Collider collider)
    {
        return collider.tag != "Tag_Wall" &&
               collider.tag != "Tag_Floor" &&
               collider.tag != "Tag_Door" &&
               collider.tag != "Tag_Locker" &&
               collider.tag != "Player";
    }

    // Terrorist target
    private Vector3? _target = null;

    // Player gameObject
    private GameObject _player = null;

    /// <summary>
    /// Indicate if player is visible
    /// </summary>
    public bool Spotted { get { return _player != null; } }
    
    ///////////////
    /// ACTIONS ///
    ///////////////
	
    /// <summary>
    /// No action
    /// </summary>
    [ActionMethod]
    public void Idle() {}

    /// <summary>
    /// Move to target
    /// </summary>
    [ActionMethod]
    public void MoveToPlayer()
    {
		//navmesh.isStopped = true;
        if (_player != null)
            navmesh.SetDestination(_player.transform.position);
    }
	
    ////////////////
    /// PERCEPTS ///
    ////////////////

    /// <summary>
    /// Indicate if see the player
    /// </summary>
	[PerceptMethod]
	[ActionLink("Idle", 0f)]
    [ActionLink("MoveToPlayer", 2f)]
    public bool SeePlayer()
    {
        Vector3 rayStart = RaycastStart.transform.position;
        RaycastHit[] hits;
        Ray ray = new Ray(rayStart, Player.transform.position - rayStart);
        bool lastSpot = Spotted;
        _player = null;
        if (Vector3.Angle(transform.forward, Player.transform.position - rayStart) <= ViewAngle && (hits = Physics.RaycastAll(ray)).Length > 0)
        {
            List<RaycastHit> tmp = new List<RaycastHit>(hits);
            tmp.Sort((a, b) => a.distance < b.distance ? -1 : 1);
            foreach (RaycastHit r in tmp)
            {
                if (!IgnoreCollider(r.collider))
                {
					if (r.collider.tag == "Player" && r.collider.gameObject.transform.localScale.x > 0.1f && PhotonNetwork.isMasterClient)
                    {
                        _player = r.collider.gameObject;
						_target = _player.transform.position;
                        if (!lastSpot)
                        {
                            if (OnSpotted != null)
                                OnSpotted.Invoke(gameObject);
                            //Debug.Log("Spotted!");

							if (!Global.GameOver) {
								Instantiate (Global.prefabScreenGameOver, Global.canvas.transform);
								Global.GameOver = true;
							}

                        }
                    }
                    break;
                }
            }
        }
        if (lastSpot && !Spotted)
        {
            if (OnLost != null)
                OnLost.Invoke(gameObject);
            //Debug.Log("Lost!");
        }
        return Spotted;
    }

	/// <summary>
	/// Indicate if not see the player
	/// </summary>
	[PerceptMethod]
	[ActionLink("MoveToPlayer", 0f)]
	public bool NoSeePlayer() {
		return !Spotted;
	}

    /// <summary>
    /// Change room
    /// </summary>
    /// <returns></returns>
    [PerceptMethod]
    [ActionLink("Idle", 2f)]
    public bool ChangeRoom()
	{
		//Debug.Log (_target);
	/*
		Debug.Log (_target);
		if (_target != null && (_target.Value - transform.position).magnitude > 1f)
			return false;
        PlayerPositionScene playerposition = GameObject.FindObjectOfType<PlayerPositionScene>();
        GameObject step = playerposition.getNextStep();
		if (step == null) {
			Debug.Log ( "No step" );

			_target = null;
			return false;
		}
        GameObject room = step.GetComponent<PlayerPositionStep>().getNextRoom();
        if (room == null)
		{

			Debug.Log ( "NextStep = " + step.name + "(" + step.GetComponent<PlayerPositionStep>().getFitnessTotal() + ") / No room)" );
            if (step != target_step)
            {
                target_step = step;
				_target = new Vector3 (0,1,0);//step.GetComponent<PlayerPositionStep>().Position;
                navmesh.SetDestination(_target.Value);
                //Debug.Log("Go to step " + step.name);
            }
            return true;
        }
        GameObject hidding = room.GetComponent<PlayerPositionRoom>().getNextHiddingPlace();
        if (hidding == null)
		{

			Debug.Log ( "NextRoom = " + step.name + "(" + step.GetComponent<PlayerPositionStep>().getFitnessTotal() + ") /" + room.name + "(" + room.GetComponent<PlayerPositionRoom>().getFitnessTotal() + ")" );
            if (room != target_room)
            {
                target_room = room;
                _target = room.GetComponent<PlayerPositionRoom>().Position;
                navmesh.SetDestination(_target.Value);
                //Debug.Log("Go to room " + room.name);
            }
            return true;
        }

        if (hidding != target_hidding)
        {
            target_hidding = hidding;
            _target = hidding.GetComponent<PlayerPositionHiddingPlace>().Position;
            navmesh.SetDestination(_target.Value);
            //Debug.Log("Go to hidding place " + hidding.name);
		}
		Debug.Log ( "NextRoom = " + step.name + "(" + step.GetComponent<PlayerPositionStep>().getFitnessTotal() + ") /" + room.name + "(" + room.GetComponent<PlayerPositionRoom>().getFitnessTotal() + ")" );
        return true;*/
		
		if (_target != null && (_target.Value - transform.position).magnitude > 1f)
			return false;

		GameObject step = GameObject.FindObjectOfType<PlayerPositionScene> ().getNextStep ();
		GameObject room = step.GetComponent<PlayerPositionStep> ().getNextRoom ();

		_target = room.GetComponent<PlayerPositionRoom>().Position;
		navmesh.SetDestination(_target.Value);

		//Debug.Log ( "NextRoom = " + step.name + "/" + room.name + " " +  _target.Value );

		return true;
    }

    /// <summary>
    /// Indicate if hasn't found a target
    /// </summary>
    [PerceptMethod]
    [ActionLink("Idle", 1.5f)]
    public bool HasNotTarget()
    {
        return _target == null;
    }
    
}
