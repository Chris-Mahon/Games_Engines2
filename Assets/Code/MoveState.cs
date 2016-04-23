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
        Vector3 force = Vector3.zero;
        bool cont = true;
        force = ObsAvoidance();
        cont = ForceApplier((force / totalWeight) * owner.avoidWeight);
        if (owner.isLeader)
        {
            force = Seek();
        }
        else
        {
            force = OffPursue();
        }
        cont = ForceApplier((force / totalWeight) * owner.moveWeight);

    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    private Vector3 OffPursue()
    {
        return Vector3.zero;
    }

    private Vector3 Seek()
    {
        return Vector3.zero;
    }

    private Vector3 ObsAvoidance()
    {
        return Vector3.zero;
    }

    public bool ForceApplier(Vector3 force)
    {
        owner.transform.position += force;
        if (owner.maxMovement - force.magnitude > 0)
        {
            owner.maxMovement -= force.magnitude;

        }
        else
        {
            force = force / 20;
            force = force * owner.maxMovement;
            owner.maxMovement = 0;
            owner.moveForce += force;
            return false;
        }
        return true;
    }
}
