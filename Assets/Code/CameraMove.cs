using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public GameObject target = null;
    [Range(0.01f, 10)]
    public float speed = 5;

	// Use this for initialization
	void Start ()
    {
	
	}

    Vector3 toTarget;
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
        {
            GameObject[] drones = GameObject.FindGameObjectsWithTag("AllyDrone");
            for (int i = 0; i < drones.Length; i++)
            {
                if (drones[i].GetComponent<Pilot>().isLeader)
                {
                    target = drones[i];
                    transform.position = (target.transform.position - (target.transform.forward*50)) + new Vector3(0, 30, 0);
                    toTarget = transform.position - target.transform.position;
                    toTarget = target.transform.InverseTransformDirection(toTarget);
                }
            }
        }
        else
        {
            Vector3 worldTarget = target.transform.TransformPoint(toTarget);
            //Debug.Log("ToTarget: " +toTarget + " world: " + worldTarget + " Target Pos: "+ target.transform.position);
            transform.position = Vector3.Lerp(transform.position, worldTarget, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, Time.deltaTime * speed);
            //transform.LookAt(target.transform);

        }
	}
}
