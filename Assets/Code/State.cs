using UnityEngine;
using System.Collections;

public abstract class State
{
    public FiniteStateMachine owner;
	// Use this for initialization
	public State (FiniteStateMachine owner)
    {
        this.owner = owner;
	}

    // Update is called once per frame
    public abstract void Update();
    public abstract void Enter();
    public abstract void Exit();
}
