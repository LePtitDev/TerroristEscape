﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class TerroristSpotter : MonoBehaviour {

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

    // Indique si le joueur est visible par le terroriste
    private bool _spotted = true;

    /// <summary>
    /// Indique si le joueur est visible par le terroriste
    /// </summary>
    public bool Spotted { get { return _spotted; } }

    /// <summary>
    /// Affiche le raycast
    /// </summary>
    public bool DisplayRaycast;

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

    void Start() {
        if (GameObject.Find("NetworkManager").GetComponent<NetworkManager>().useNetwork)
        {
            enabled = false;
            return;
        }
        navmesh = GetComponent<NavMeshAgent>();
        target_step = null;
        target_room = null;
        target_hidding = null;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        Vector3 rayStart = RaycastStart.transform.position;
        RaycastHit[] hits;
        Ray ray = new Ray(rayStart, Player.transform.position - rayStart);
        bool lastSpot = _spotted;
        _spotted = false;
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
                        _spotted = true;
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
        if (lastSpot && !_spotted)
        {
            if (OnLost != null)
                OnLost.Invoke(gameObject);
            //Debug.Log("Lost!");
        }
        if (DisplayRaycast)
            DrawLine(rayStart, Player.transform.position, Spotted ? Color.green : Color.red, Time.deltaTime);
        if (lastPosition != transform.position)
        {
            lastPosition = transform.position;
            return;
        }
        PlayerPositionScene playerposition = GameObject.FindObjectOfType<PlayerPositionScene>();
        GameObject step = playerposition.getNextStep();
        if (step == null)
            return;
        GameObject room = step.GetComponent<PlayerPositionStep>().getNextRoom();
        if (room == null)
        {
            if (step != target_step)
            {
                target_step = step;
                navmesh.SetDestination(step.GetComponent<PlayerPositionStep>().Position);
                Debug.Log("Go to step " + step.name);
            }
            return;
        }
        GameObject hidding = room.GetComponent<PlayerPositionRoom>().getNextHiddingPlace();
        if (hidding == null)
        {
            if (room != target_room)
            {
                target_room = room;
                navmesh.SetDestination(room.GetComponent<PlayerPositionRoom>().Position);
                Debug.Log("Go to room " + room.name);
            }
            return;
        }
        if (hidding != target_hidding)
        {
            target_hidding = hidding;
            navmesh.SetDestination(hidding.GetComponent<PlayerPositionHiddingPlace>().Position);
            Debug.Log("Go to hidding place " + hidding.name);
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

    // Material for group displaying
    private static Material lineMaterial = null;

    // lineMaterial accessor
    public static Material LineMaterial
    {
        get
        {
            CreateLineMaterial();
            return lineMaterial;
        }
    }

    // Draw the patch group
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f, float width = 0.1f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = LineMaterial;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    // Create the matrial
    private static void CreateLineMaterial()
    {
        if (lineMaterial == null)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
}
