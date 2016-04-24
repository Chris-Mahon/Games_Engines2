using UnityEngine;
using System.Collections;

public class MotherBrain : FiniteStateMachine {
	public GameObject targetShip;
	public GameObject drone;
	public int remainingPeople;


	// Use this for initialization
	void Start () 
	{
		GameObject leader = Instantiate(drone, transform.position+new Vector3(0, -10, -0), transform.rotation) as GameObject;
		leader.GetComponent<Pilot>().Initialise(leader.transform.forward * 100);
		leader.name = "Ally Leader";
		GameObject follower = Instantiate(drone, transform.position+new Vector3(0, -10, 0), transform.rotation) as GameObject;
		follower.GetComponent<Pilot>().Initialise (leader, -1);
		follower.name = "Left Wing";
		GameObject follower2 = Instantiate(drone, transform.position+new Vector3(0, -10, 0), transform.rotation) as GameObject;
		follower2.GetComponent<Pilot>().Initialise (leader, 1);
		follower2.name = "Right Wing";
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
