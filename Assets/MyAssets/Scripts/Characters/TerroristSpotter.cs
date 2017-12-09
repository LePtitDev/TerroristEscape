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

    /// <summary>
    /// Position in last update
    /// </summary>
    private Vector3 lastPosition;

    protected override void Start() {
        base.Start();
        if (GameObject.Find("NetworkManager").GetComponent<NetworkManager>().useNetwork)
        {
            enabled = false;
            return;
        }
        if (Player == null)
            Player = GameObject.Find("LobbyCamera");
        navmesh = GetComponent<NavMeshAgent>();
        target_step = null;
        target_room = null;
        target_hidding = null;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update () {
        
        base.Update();
        lastPosition = transform.position;
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
                    if (r.collider.tag == "Player")
                    {
                        _player = r.collider.gameObject;
                        if (!lastSpot)
                        {
                            if (OnSpotted != null)
                                OnSpotted.Invoke(gameObject);
                            //Debug.Log("Spotted!");
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
    /// Change room
    /// </summary>
    /// <returns></returns>
    [PerceptMethod]
    [ActionLink("Idle", 0f)]
    public bool ChangeRoom()
    {
        if (lastPosition != transform.position)
        {
            lastPosition = transform.position;
            return false;
        }
        PlayerPositionScene playerposition = GameObject.FindObjectOfType<PlayerPositionScene>();
        GameObject step = playerposition.getNextStep();
        if (step == null)
            return false;
        GameObject room = step.GetComponent<PlayerPositionStep>().getNextRoom();
        if (room == null)
        {
            if (step != target_step)
            {
                target_step = step;
                _target = step.GetComponent<PlayerPositionStep>().Position;
                navmesh.SetDestination(_target.Value);
                //Debug.Log("Go to step " + step.name);
            }
            return true;
        }
        GameObject hidding = room.GetComponent<PlayerPositionRoom>().getNextHiddingPlace();
        if (hidding == null)
        {
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
