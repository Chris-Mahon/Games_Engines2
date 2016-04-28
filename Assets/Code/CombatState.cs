using UnityEngine;
using System.Collections;

public class CombatState : State
{

    // Use this for initialization
    public CombatState(Pilot owner) : base(owner)
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        float totalWeight = (owner.avoidWeight + owner.moveWeight);
        Vector3 tempForce = Vector3.zero;
        Vector3 force = Vector3.zero;

        tempForce = (ObsAvoidance() / totalWeight) * owner.avoidWeight;
        force += tempForce;

        if (owner.isLeader)
        {
            tempForce += Arrive(owner.targetPos);
        }
        else
        {
            if (owner.target.GetComponent<Pilot>().isAlive)
            {
                tempForce += OffsetPursue(owner.target, new Vector3(10 * owner.offset, 0, -5));
            }
            else
            {
                tempForce += Arrive(owner.home.transform.position);
            }
        }

        tempForce += (ObsAvoidance() / totalWeight) * owner.moveWeight;
        force += tempForce;
        force = Vector3.ClampMagnitude(force, owner.speed);

        Vector3 acceleration = force / owner.transform.gameObject.GetComponent<Rigidbody>().mass;
        owner.moveForce += acceleration * Time.deltaTime;
        owner.moveForce = Vector3.ClampMagnitude(owner.moveForce, owner.speed);
        if (owner.moveForce.magnitude > float.Epsilon)
        {
            owner.transform.position += owner.moveForce;
        }

    }

    public Vector3 OffsetPursue(GameObject leader, Vector3 offset)
    {
        Vector3 target = Vector3.zero;
        target = leader.transform.TransformPoint(offset);
        Vector3 toTarget = target - owner.transform.position;
        float dist = toTarget.magnitude;
        float lookAhead = dist / owner.speed;

        Vector3 offsetPursueTargetPos = target + (lookAhead * leader.GetComponent<Pilot>().moveForce);
        return Arrive(offsetPursueTargetPos);
    }

    private Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - owner.transform.position;

        float slowingDistance = 40.0f;
        float distance = toTarget.magnitude;
        if (distance < 2f)
        {
            owner.moveForce = Vector3.zero;
            return Vector3.zero;
        }
        float ramped = owner.speed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, owner.speed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - owner.moveForce;
    }

    private Vector3 ObsAvoidance()
    {
        return new Vector3(0, 0, 0);
    }

    public override void Enter()
    {
        owner.speed = owner.maxForce/2;
        owner.State = "Engaging";
    }

    public override void Exit()
    {

    }

    IEnumerator Fire()
    {
        while (true)
        {
            owner.cannonReady = true;
        }
        WaitForSeconds(10);
        yield return null;
    }
}
