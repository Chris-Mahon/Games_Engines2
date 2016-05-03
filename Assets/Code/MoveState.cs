using UnityEngine;
using System.Collections;

public class MoveState : State
{
    public Boid myBoid;
    // Use this for initialization
    public MoveState(Pilot owner):base(owner)
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!owner.target.GetComponent <Boid>().myFSM.isAlive)
        {
            owner.myBoid.targetPos = owner.home.transform.position;
        }
        else
        {
            owner.myBoid.targetPos = owner.target.transform.position;
        }
    }

	public override void Enter()
	{
        owner.GetComponent<Pilot>().speed = owner.GetComponent<Pilot>().maxForce;
        owner.State = "TargetSeeking";
        owner.StartCoroutine(Fire());
        owner.myBoid.isMoving = true;
        owner.myBoid.isAvoiding = true;
        owner.myBoid.myTarget = owner.target;
    }
	
	public override void Exit()
    {
        owner.myBoid.isMoving = false;
        owner.myBoid.isAvoiding = false;

    }

    IEnumerator Fire()
    {
        while (owner.State == "TargetSeeking")
        {
            Debug.Log("pew");
            GameObject shootingTarget = null;
            GameObject[] targets = GameObject.FindGameObjectsWithTag(owner.enemyTag.ToString());
            foreach (GameObject testedTarget in targets)
            {
                if (Vector3.Distance(testedTarget.transform.position, owner.transform.position)<30)
                {
                    shootingTarget = testedTarget;
                }
            }
            if (shootingTarget != null)
            {
                GameObject bull = GameObject.Instantiate(owner.bullet, owner.transform.position + owner.transform.forward, owner.transform.rotation) as GameObject;
                bull.GetComponent<ProjectileMove>().Initialise(owner.tag);
                bull.transform.LookAt(shootingTarget.transform.position);
                Physics.IgnoreCollision(bull.GetComponent<Collider>(), owner.GetComponent<Collider>());
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

}
