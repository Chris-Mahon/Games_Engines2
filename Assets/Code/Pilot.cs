using UnityEngine;
using System.Collections;

public class Pilot : FiniteStateMachine
{
    public bool isLeader;
    public bool cannonReady;

    public float maxForce;
    public GameObject home;
	public Vector3 targetPos;
    public GameObject bullet;
    public GameObject explosion;
    public int health = 10;
    public float maxFuel;
    public Boid myBoid;
    [Header("Behaviour")]
	public GameObject target; 
	public int offset = 0;

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
		StateChange(new MoveState (this));
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
        if (isLeader)
        {
            if (State == "TargetSeeking")
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 80)
                {

                    State = "Engaging";
                    targetPos = transform.position - (transform.right * 50);
                    StateChange(new CombatState(this));
                }
            }
        }
        else
        {
            if (target.GetComponent<Boid>().myFSM.State == "Engaging")
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
