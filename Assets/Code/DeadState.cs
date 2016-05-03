using UnityEngine;
using System.Collections;

public class DeadState : State {
    // Use this for initialization
    public DeadState(FiniteStateMachine owner):base(owner)
    {

    }

    // Update is called once per frame
    public override void Update () {
	
	}

    public override void Enter()
    {
        owner.isAlive = false;
        owner.State = "dead";
    }

    public override void Exit()
    {

    }
}
