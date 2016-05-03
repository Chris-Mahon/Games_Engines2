using UnityEngine;
using System.Collections;

public class Pilot : FiniteStateMachine
{
    public bool cannonReady;
    public bool debugMode;


	public Vector3 targetPos;
    
    public float maxFuel;

	// Use this for initialization
	public override void Start ()
    {
        myBoid = GetComponent<Boid>();
    }

    public void Initialise(GameObject leader, GameObject mother, int offset)
    {
        isLeader = false;
        isAlive = true;
        myBoid = GetComponent<Boid>();
        myBoid.fuel = maxFuel;
		target = leader;
        home = mother;
		this.offset = offset;
		transform.position += ((new Vector3 (10, 0, 0) * offset) + new Vector3(0, 0, -10));
        StateChange(new MoveState(this));
    }

    public void Initialise(GameObject target, GameObject mother)
    {
        isLeader = true;
        isAlive = true;
        myBoid = GetComponent<Boid>();
        myBoid.fuel = maxFuel;
        this.target = target;
        home = mother;
		StateChange(new MoveState(this));
        transform.forward += targetPos;
    }

	public void Initialise()
	{
		isLeader = true;
		isAlive = true;
        myBoid = GetComponent<Boid>();
        myBoid.fuel = maxFuel;
        StateChange(new MoveState(this));
    }

    public override void Update ()
    {
        if (myBoid.fuel < 800 && State != "dead")
        {
            StateChange(new ReturnState(this));
        }
        if (myBoid.fuel < 0 && State != "dead")
        {
            StateChange(new DeadState(this));
        }
        if (isLeader)
        {
            if (State == "TargetSeeking")
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 200)
                {
                    State = "Engaging";
                    targetPos = transform.position - (transform.right * 100);
                    StateChange(new CombatState(this));
                    myBoid.moveForce -= myBoid.moveForce / 4;
                }
            }
            else if (State == "Engaging")
            {
                if (Vector3.Distance(transform.position, myBoid.targetPos) < 30*maxForce)
                {
                    State = "Returning";
                    targetPos = transform.position - (transform.right * 100);
                    StateChange(new ReturnState(this));
                    myBoid.moveForce -= myBoid.moveForce / 4;
                }
            }
        }
        else
        {
            if (target != null)
            {
                if (target.GetComponent<Boid>().myFSM.State == "Engaging")
                {
                    StateChange(new CombatState(this));
                }
            }
            else
            {
                StateChange(new ReturnState(this));
            }
        }
        if (health < 1)
        {
            if (State != "dead")
            {
                GameObject thing = Instantiate(explosion, transform.position - new Vector3(0, 25, 0), Quaternion.identity) as GameObject;
                thing.transform.parent = transform;
                StateChange(new DeadState(this));
                StartCoroutine(JunkRemoval());
            }
        }
        if (currState != null)
		{
            currState.Update();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != tag)
        {
            this.health = 0;
        }
    }



    void StateChange(State newState)
    {
        if (currState != null)
        {
            currState.Exit();
        }
        currState = newState;
        if (currState != null)
        {
            currState.Enter();
        }
    }

    IEnumerator JunkRemoval()
    {
        yield return new WaitForSeconds(60);
        Destroy(transform.gameObject);
    }
}
