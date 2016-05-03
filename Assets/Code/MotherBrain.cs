using UnityEngine;
using System.Collections;
using System;

public class MotherBrain : FiniteStateMachine
{
	public GameObject targetShip;
	public GameObject drone;

    [Range(0, 100)]
    public int remainingPeople;


	// Use this for initialization
	public override void Start ()
    {
        isAlive = true;
        home = gameObject;

        myBoid = GetComponent<Boid>();
        StartCoroutine(SpawnNewShips());
	}

    // Update is called once per frame
    public override void Update()
    {
        if (health < 0)
        {
            StateChange(new DeadState(this));
        }
        GameObject[] drones = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < drones.Length; i++)
        {
            if (drones[i].name != "Projectile(Clone)")
            {
                if (Vector3.Distance(transform.position, drones[i].transform.position) < 20 && drones[i].GetComponent<Boid>().myFSM.State == "Returning")
                {
                    Debug.Log(drones[i].GetComponent<Boid>().fuel);
                    Destroy(drones[i]);
                    remainingPeople++;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != tag)
        {
            this.health--;
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

    IEnumerator SpawnNewShips()
    {
        while (isAlive)
        {


            if (remainingPeople > 2)
            {
                Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 5;
                GameObject leader = Instantiate(drone, transform.position + new Vector3(0, -10, 0) + randomOffset, transform.rotation) as GameObject;
                leader.GetComponent<Pilot>().Initialise(targetShip, gameObject);
                leader.name = "Leader";
                remainingPeople--;
                if (remainingPeople > 0)
                {
                    GameObject follower = Instantiate(drone, transform.position + new Vector3(10, -10, -10) + randomOffset, leader.transform.rotation) as GameObject;
                    follower.GetComponent<Pilot>().Initialise(leader, this.gameObject, -1);
                    follower.name = "Left Wing";
                    remainingPeople--;
                }

                if (remainingPeople > 0)
                {
                    GameObject follower2 = Instantiate(drone, transform.position + new Vector3(10, -10, 10) + randomOffset, leader.transform.rotation) as GameObject;
                    follower2.GetComponent<Pilot>().Initialise(leader, this.gameObject, 1);
                    follower2.name = "Right Wing";
                    remainingPeople--;
                }
            }
            else if (remainingPeople > 0)
            {
                GameObject leader = Instantiate(drone, transform.position + new Vector3(0, -10, 0) + UnityEngine.Random.insideUnitSphere*5, transform.rotation) as GameObject;
                leader.GetComponent<Pilot>().Initialise(targetShip, gameObject);
                leader.name = "Leader";
                remainingPeople--;
            }
            yield return new WaitForSeconds(15);
        }
    }
}
