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
        
    }
    
    public override void Enter()
    {
        //owner.speed = owner.maxForce/2;
        owner.State = "Engaging";
        owner.myBoid.isMoving = true;
        owner.myBoid.isAvoiding = true;
        owner.speed = owner.maxForce;

        if (owner.isLeader)
        {
            owner.myBoid.myTarget = null;
            owner.myBoid.targetPos = (owner.transform.position - (owner.transform.right * 100));
        }
    }

    public override void Exit()
    {

        owner.myBoid.isMoving = false;
        owner.myBoid.isAvoiding = false;
    }

    IEnumerator Fire()
    {
        while (owner.State == "Engaging")
        {
            GameObject bull = GameObject.Instantiate(owner.bullet, owner.transform.position, owner.transform.rotation * new Quaternion(0, 1, 0, -90)) as GameObject;
            bull.GetComponent<ProjectileMove>().Initialise(owner.tag);
            bull.GetComponent<Rigidbody>().AddForce(owner.myBoid.moveForce);
            Physics.IgnoreCollision(bull.GetComponent<Collider>(), owner.GetComponent<Collider>());
            yield return new WaitForSeconds(5);
        }
    }
}
