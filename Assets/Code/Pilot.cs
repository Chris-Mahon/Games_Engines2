using UnityEngine;
using System.Collections;

public class Pilot : FiniteStateMachine
{
    private bool isInitialised;
    public bool isLeader;
    public bool isAlive;
    public float maxMovement;
    public Vector3 moveForce;

    private int fuel;
    private Vector3 home;

    [HideInInspector]
    public int health;
    public State currState;

    [Header("Behaviour")]
    public int avoidWeight;
    public int moveWeight;

	// Use this for initialization
	void Start ()
    {
        isInitialised = false;
        home = transform.position;
	}

    void Initialise(GameObject leader, int offset)
    {
        isLeader = false;
        isAlive = true;
        fuel = 100;
        maxMovement = 20;

    }

    void Initialise(GameObject target)
    {
        isLeader = true;
        isAlive = true;
        fuel = 100;
        maxMovement = 20;
    }

    void Update ()
    {
        if (isAlive)
        {
            moveForce = Vector3.zero;
            maxMovement = 20;
        }
        if (health < 0)
        {
            if (currState != null)
            {
                currState.Exit();
            }
            currState = new DeadState(this);
            if (currState != null)
            {
                currState.Enter();
            }
        }
        if (currState != null)
        {
            currState.Update();
        }
        transform.position += moveForce;
	}
}
