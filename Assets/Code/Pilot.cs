using UnityEngine;
using System.Collections;

public class Pilot : FiniteStateMachine
{
    private bool isInitialised;
    public bool isLeader;
    public bool isAlive;
    public float maxMovement;
    public Vector3 moveForce = Vector3.zero;

    private int fuel;
    private Vector3 home;

    [HideInInspector]
    public int health;
    public State currState;
	public string State = "";
	public float speed;
    [Header("Behaviour")]
    public int avoidWeight;
    public int moveWeight;

	// Use this for initialization
	void Start ()
    {
        isInitialised = false;
        home = transform.position;
		Initialise ();
	}

    void Initialise(GameObject leader, int offset)
    {
        isLeader = false;
        isAlive = true;
        fuel = 100;
        maxMovement = 20;
		currState = new MoveState (this);
    }

    void Initialise(GameObject target)
    {
        isLeader = true;
        isAlive = true;
        fuel = 100;
		maxMovement = 20;
		currState = new MoveState (this);
    }

	void Initialise()
	{
		isLeader = true;
		isAlive = true;
		fuel = 100;
		maxMovement = 20;
		currState = new MoveState (this);
	}

    void Update ()
    {
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
		transform.forward += moveForce;
		transform.position += moveForce*speed;
	}
}
