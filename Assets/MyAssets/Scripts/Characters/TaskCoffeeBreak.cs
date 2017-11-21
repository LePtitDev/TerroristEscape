using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskCoffeeBreak : MonoBehaviour
{

    public bool FlagHaveCoffee = false;

    public GameObject SpeakingTarget = null;

    bool flagInRoom = false;

    bool flagTakeCoffee = false;

    GameObject coffee_room;

    GameObject selected_coffeemachine;

    float coffee_take_timeout;

    NavMeshAgent agent;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        GameObject tmp = null;
        foreach (Transform t in GameObject.Find("Office_Checkpoints").GetComponentsInChildren<Transform>())
        {
            if (t.gameObject.name == "CoffeeRooms")
            {
                tmp = t.gameObject;
                break;
            }
        }
        List<Transform> rooms = new List<Transform>();
        foreach (Transform t in tmp.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject.name == "Room")
                rooms.Add(t);
        }
        coffee_room = rooms[0].gameObject;
        NavMeshPath path = new NavMeshPath();
        float dist = 0f;
        agent.CalculatePath(coffee_room.transform.position, path);
        foreach (Vector3 v in path.corners)
            dist += v.magnitude;
        for (int i = 1; i < rooms.Count; i++)
        {
            Transform t = rooms[i];
            NavMeshPath ptmp = new NavMeshPath();
            float dtmp = 0f;
            agent.CalculatePath(t.position, ptmp);
            foreach (Vector3 v in ptmp.corners)
                dtmp += v.magnitude;
            if (dist > dtmp)
            {
                path = ptmp;
                dist = dtmp;
                coffee_room = t.gameObject;
            }
        }
        agent.SetPath(path);
    }
	
	// Update is called once per frame
	void Update () {
        if (!flagInRoom && coffee_room.GetComponent<BoxCollider>().bounds.Contains(transform.position))
        {
           // Debug.Log("Je suis à la Cafet'");
            flagInRoom = true;
            SelectCoffeeMachine();
            agent.SetDestination(selected_coffeemachine.transform.position);
        }
        if (!flagTakeCoffee && selected_coffeemachine != null && (selected_coffeemachine.transform.position - transform.position).magnitude < 0.5f)
        {
            //Debug.Log("Je prends mon café");
            agent.isStopped = true;
            flagTakeCoffee = true;
            TakeCoffee();
        }
        if (!FlagHaveCoffee && flagTakeCoffee && Time.time > coffee_take_timeout)
        {
          //  Debug.Log("Je bois mon café");
            FlagHaveCoffee = true;
        }
	}

    void SelectCoffeeMachine()
    {
        List<Transform> coffee_machines = new List<Transform>();
        foreach (Transform t in coffee_room.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject.name == "CoffeeMachine")
                coffee_machines.Add(t);
        }
        selected_coffeemachine = coffee_machines[0].gameObject;
        foreach (Transform t in coffee_machines)
        {
            if ((selected_coffeemachine.transform.position - transform.position).magnitude > (t.position - transform.position).magnitude)
                selected_coffeemachine = t.gameObject;
        }
    }

    void TakeCoffee()
    {
        coffee_take_timeout = Time.time + 3f;
    }

    void SpeakDuringCoffee()
    {

    }
}
