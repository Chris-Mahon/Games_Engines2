using UnityEngine;
using System.Collections;

public class Pilot : FiniteStateMachine
{
    public bool isLeader;
    public bool isAlive;
    public bool cannonReady;

    public float maxForce;
    public Vector3 moveForce = Vector3.zero;

    private int fuel;
	public GameObject home;
	public Vector3 targetPos;
    public GameObject bullet;
    public GameObject explosion;
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

	}

    public void Initialise(GameObject leader, GameObject mother, int offset)
    {
        isLeader = false;
        isAlive = true;
        fuel = 100;
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
        fuel = 100;
		this.target = target;
        home = mother;
		StateChange(new MoveState (this));
        transform.forward += targetPos;
    }

	public void Initialise()
	{
		isLeader = true;
		isAlive = true;
		fuel = 100;
        //maxMovement = 20;
        StateChange(new MoveState(this));
    }

    void Update ()
    {
        if (cannonReady && isAlive)
        {
            GameObject bull = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            bull.GetComponent<ProjectileMove>().Initialise(tag);
            Physics.IgnoreCollision(bull.GetComponent<Collider>(), GetComponent<Collider>());
            Debug.Log(bull.transform.forward - transform.forward);
            cannonReady = false;
        }
        if (isLeader)
        {
            if (State == "TargetSeeking")
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 80)
                {

                    State = "Engaging";
                    transform.forward = -transform.right;
                    targetPos = transform.position + transform.forward * 50;
                    StateChange(new CombatState(this));
                }
            }
        }
        else
        {
            if (target.GetComponent<Pilot>().State == "Engaging")
            {
                StateChange(new CombatState(this));
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
		transform.forward += moveForce*speed;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != tag)
        {
            this.health = 0;
            Debug.Log("Oww");
        }
    }

    void OnDrawGizmos()
    {
        if (State == "TargetSeeking")
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
        if (State == "Engaging")
        {
            Gizmos.DrawLine(transform.position, targetPos);

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
