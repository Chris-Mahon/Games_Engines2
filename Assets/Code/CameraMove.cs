using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public GameObject target = null;
    [Range(0.01f, 10)]
    public float speed = 5;

   public Targets toFind;
    // Use this for initialization
    public enum Targets
    {
        AllyDrone, EnemyDrone
    }

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
            if (drones[0] == null)
            {
                return;
            }
            for (int i = 0; i < drones.Length; i++)
            {
                if (drones[i].GetComponent<Pilot>()!=null)
                {
                    if (drones[i].GetComponent<Pilot>().isLeader)
                    {
                        target = drones[i];
                        transform.position = (target.transform.position - (target.transform.forward * 50)) + new Vector3(0, 30, 0);
                        toTarget = transform.position - target.transform.position;
                        toTarget = target.transform.InverseTransformDirection(toTarget);
                    }
                }
            }
        }
        else
        {
            Vector3 worldTarget = target.transform.TransformPoint(toTarget);
            transform.position = Vector3.Lerp(transform.position, worldTarget, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, Time.deltaTime * speed);

        }
	}
}
