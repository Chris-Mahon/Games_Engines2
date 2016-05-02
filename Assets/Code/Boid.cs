using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {
    public FiniteStateMachine myFSM;
    public float fuel;
    public Vector3 moveForce = Vector3.zero;
    public Vector3 targetPos;
    public GameObject target;
    public bool isMoving, isAvoiding, isRunning, isWandering;

    // Use this for initialization
    void Start ()
    {
        if (GetComponent<Pilot>() != null)
        {
            myFSM = this.GetComponent<Pilot>();
        }
        else if (GetComponent<MotherBrain>())
        {
            myFSM = this.GetComponent<MotherBrain>();
        }
        else
        {
            Debug.Log("Wat");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 tempForce = Vector3.zero;
        Vector3 force = Vector3.zero;
        float totalWeight = CalculateWeights();

        if (isMoving)
        {
            force += (Arrive(targetPos)/totalWeight)*myFSM.moveWeight;
        }
        if (isAvoiding)
        {
            force += (ObsAvoidance()/totalWeight)*myFSM.avoidWeight;
        }
        if (isRunning)
        {
            force += (Arrive(target.transform.position)/totalWeight)*myFSM.fleeWeight;
        }
        if (isWandering)
        {
            force += Vector3.zero;
        }
        Debug.Log("force: " + moveForce + " from Boid");

        force = Vector3.ClampMagnitude(force, myFSM.speed);

        Vector3 acceleration = force / transform.gameObject.GetComponent<Rigidbody>().mass;
        moveForce += acceleration * Time.deltaTime;
        moveForce = Vector3.ClampMagnitude(moveForce, myFSM.speed);
        if (moveForce.magnitude > float.Epsilon)
        {
            transform.position += moveForce;
            transform.forward += moveForce*myFSM.speed;
        }

    }

    private Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

        float slowingDistance = 40.0f;
        float distance = toTarget.magnitude;
        if (distance < 2f)
        {
            moveForce = Vector3.zero;
            return Vector3.zero;
        }
        float ramped = myFSM.speed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, myFSM.speed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - moveForce;
    }

    public Vector3 OffsetPursue(GameObject leader, Vector3 offset)
    {

        Vector3 target = Vector3.zero;
        target = leader.transform.TransformPoint(offset);
        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;
        float lookAhead = dist / myFSM.speed;

        Vector3 offsetPursueTargetPos = target + (lookAhead * leader.GetComponent<Boid>().moveForce);
        return Arrive(offsetPursueTargetPos);
    }

    private Vector3 ObsAvoidance()
    {
        return new Vector3(0, 0, 0);
    }

    float CalculateWeights()
    {
        float totalWeight = 0;
        if (isMoving)
        {
            totalWeight += myFSM.moveWeight;
        }
        if (isAvoiding)
        {
            totalWeight += myFSM.avoidWeight;
        }
        if (isRunning)
        {
            totalWeight += myFSM.fleeWeight;
        }
        if (isWandering)
        {
            totalWeight += myFSM.wanderWeight;
        }

        return totalWeight;
    }
}
