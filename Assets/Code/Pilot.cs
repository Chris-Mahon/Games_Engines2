using UnityEngine;
using System.Collections;

public class Pilot : FiniteStateMachine
{
    public bool isLeader;
    public bool isAlive;
    public float maxForce;
    public Vector3 moveForce = Vector3.zero;

    private int fuel;
	private Vector3 home;
	public Vector3 targetPos;

    [HideInInspector]
    public int health = 10;
    public State currState;
	public string State = "";
	public float speed;
    [Header("Behaviour")]
    public int avoidWeight;
    public int moveWeight;
	public GameObject target; 
	public int offset = 0;

	// Use this for initialization
	void Start ()
    {
        //isInitialised = false;
        home = transform.position;
	}

    public void Initialise(GameObject leader, int offset)
    {
        isLeader = false;
        isAlive = true;
        fuel = 100;
		target = leader;
		this.offset = offset;
		transform.position += ((new Vector3 (10, 0, 0) * offset) + new Vector3(0, 0, -10));
		currState = new MoveState (this);
        currState.Enter();
    }

    public void Initialise(GameObject target)
    {
        isLeader = true;
        isAlive = true;
        fuel = 100;
		this.target = target;
		currState = new MoveState (this);
        transform.forward += targetPos;
        currState.Enter();
    }

	public void Initialise()
	{
		isLeader = true;
		isAlive = true;
		fuel = 100;
		//maxMovement = 20;
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
			//currTotalForce
            currState.Update();
        }
		transform.forward += moveForce*speed;
		//transform.position += moveForce;
	}
}
