using UnityEngine;
using System.Collections;

public class MoveState : State {

    // Use this for initialization
    public MoveState(Pilot owner):base(owner)
    {

    }

    // Update is called once per frame
    public override void Update()
    {
		float totalWeight = (owner.avoidWeight + owner.moveWeight);
		Vector3 tempForce = Vector3.zero;
		Vector3 force = Vector3.zero;

		tempForce = (ObsAvoidance ()/totalWeight) * owner.avoidWeight;
		force += tempForce;

		if (owner.isLeader)
		{
			tempForce += Arrive(owner.targetPos);//.transform.position);
		}
		else
		{
			tempForce += OffsetPursue(owner.target, new Vector3(20*owner.offset, 0, -20));
		}
		
		tempForce += (ObsAvoidance ()/totalWeight) * owner.moveWeight;
		force += tempForce;
		Debug.Log (force);
		force = Vector3.ClampMagnitude(force, owner.maxForce);

		Vector3 acceleration = force / owner.transform.gameObject.GetComponent<Rigidbody>().mass;        
		Debug.Log (force);
		owner.moveForce += acceleration * Time.deltaTime;        
		owner.moveForce = Vector3.ClampMagnitude(owner.moveForce, owner.maxForce);
		if (owner.moveForce.magnitude > float.Epsilon)
		{
			owner.transform.forward = owner.moveForce.normalized;
			owner.transform.position += owner.moveForce;
		}

        /*float totalWeight = (owner.avoidWeight + owner.moveWeight);
        Vector3 force = Vector3.zero;
        bool cont = true;
		if (cont)
		{
			force = ObsAvoidance();
			cont = ForceApplier((force / totalWeight) * owner.avoidWeight);
		}
		if (cont) 
		{
			if (owner.isLeader)
			{
				force = Arrive();
			}
			else
			{
				force = OffPursue(-1*Vector3(1, 1, 1));
			}
			cont = ForceApplier((force / totalWeight) * owner.moveWeight);
		}*/
    }

	public Vector3 OffsetPursue(GameObject leader, Vector3 offset)
	{
		Vector3 relOffset = Vector3.Scale(offset, leader.transform.forward.normalized);
		Vector3 target = leader.transform.position + offset;
		Vector3 toTarget = owner.transform.position - target;
		float dist = toTarget.magnitude;
		float lookAhead = dist / owner.maxForce;
		Debug.Log (target);
		Vector3 offsetPursueTargetPos = target + (lookAhead * leader.GetComponent<Pilot>().moveForce.normalized);
		return Arrive(offsetPursueTargetPos);
	}

	private Vector3 Arrive(Vector3 targetPos)
	{
		Vector3 toTarget = targetPos - owner.transform.position;
		
		float slowingDistance = 15.0f;
		float distance = toTarget.magnitude;
		if (distance < 4f)
		{
			owner.moveForce = Vector3.zero;
			return Vector3.zero;
		} 
		float ramped = owner.maxForce * (distance / slowingDistance);
		
		float clamped = Mathf.Min(ramped, owner.maxForce);
		Vector3 desired = clamped * (toTarget / distance);

		return desired - owner.moveForce;
	}

    private Vector3 ObsAvoidance()
    {
        return new Vector3(0, 0, 0);
    }

    /*public bool ForceApplier(Vector3 force)
    {
        if (owner.currTotalForce - force.magnitude > 0)
        {
            owner.currTotalForce -= force.magnitude;
        }
        else
        {
            force = force / 20;
//            force = force * owner.maxMovement;
            owner.currTotalForce = 0;
			Debug.Log (owner.moveForce);
            owner.moveForce += force;
            return false;
        }
        return true;
    }*/

	public override void Enter()
	{
		
	}
	
	public override void Exit()
	{
		
	}
}
