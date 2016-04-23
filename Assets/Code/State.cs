using UnityEngine;
using System.Collections;

public abstract class State{
    public Pilot owner;
	// Use this for initialization
	public State (Pilot owner)
    {
        this.owner = owner;
	}

    // Update is called once per frame
    public abstract void Update();
    public abstract void Enter();
    public abstract void Exit();
}
