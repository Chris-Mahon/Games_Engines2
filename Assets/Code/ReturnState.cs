using UnityEngine;
using System.Collections;

public class ReturnState : State
{
    public Boid myBoid;
    // Use this for initialization
    public ReturnState(Pilot owner) : base(owner)
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        owner.myBoid.targetPos = owner.home.transform.position;
    }

    public override void Enter()
    {
        owner.speed = owner.maxForce;
        owner.State = "Returning";
        owner.myBoid.isRunning = true;
        owner.myBoid.isAvoiding = true;
        owner.myBoid.myTarget = owner.home;
    }

    public override void Exit()
    {
        owner.myBoid.isRunning = false;
        owner.myBoid.isAvoiding = false;

    }
}
