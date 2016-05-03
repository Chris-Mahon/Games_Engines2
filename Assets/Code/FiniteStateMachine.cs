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
    [Range(0.01f, 30)]
    public int health;
    public State currState;
    public string State = "";
    public bool isLeader = false;
    public int offset = 0;
    public GameObject home;
    public Boid myBoid;
    [Range(0.01f, 10)]
    public float maxForce;
    public GameObject target;
    public GameObject bullet;
    public GameObject explosion;

    // Use this for initialization
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();
}
