﻿using UnityEngine;
using System.Collections;
using System;

public class MotherBrain : FiniteStateMachine
{
	public GameObject targetShip;
	public GameObject drone;
	public int remainingPeople;


	// Use this for initialization
	public override void Start ()
    {
        isAlive = true;

        GameObject leader = Instantiate(drone, transform.position+new Vector3(0, -10, 0), transform.rotation) as GameObject;
		leader.GetComponent<Pilot>().Initialise(targetShip, gameObject);
		leader.name = "Ally Leader";
		GameObject follower = Instantiate(drone, transform.position+new Vector3(10, -10, -10), leader.transform.rotation) as GameObject;
		follower.GetComponent<Pilot>().Initialise (leader, this.gameObject, -1);
		follower.name = "Left Wing";
		GameObject follower2 = Instantiate(drone, transform.position + new Vector3(10, -10, 10), leader.transform.rotation) as GameObject;
		follower2.GetComponent<Pilot>().Initialise (leader, this.gameObject, 1);
		follower2.name = "Right Wing";
	}
	
	// Update is called once per frame
	public override void Update () 
	{
	
	}
}
