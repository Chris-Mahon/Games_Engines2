using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public GameObject target = null;
	// Use this for initialization
	void Start ()
    {
	
	}
	
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
                }
            }
        }
        else
        {
            transform.position = target.transform.position + Vector3.Scale(target.transform.forward, new Vector3(0, 5, -15));
        }
	}
}
