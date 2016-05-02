using UnityEngine;
using System.Collections;

public abstract class FiniteStateMachine : MonoBehaviour
{
    public int avoidWeight;
    public int moveWeight;
    public int fleeWeight;
    public int wanderWeight;
    public float speed;
    public bool isAlive;
    public State currState;
    public string State = "";

    // Use this for initialization
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();
}
