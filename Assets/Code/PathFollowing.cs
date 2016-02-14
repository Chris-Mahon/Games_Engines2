using UnityEngine;
using System.Collections.Generic;

public class PathFollowing : MonoBehaviour
{
    public float radius = 5;
    public int waypointCount = 10;
    int curent = 0;
    public List<Vector3> waypoints = new List<Vector3>();
    public Vector3 AllyMothership;
    public Vector3 EnemyMothership = new Vector3(0, 0, 0);
    public Vector3 basis;
    public float dist = 0;
    public float rotOffset = 0;

    void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i], waypoints[(i + 1) % waypointCount]);
        }
        Gizmos.DrawLine(transform.position, basis);
        Gizmos.DrawLine(EnemyMothership, basis);
    }

    // Use this for initialization
    void Start()
    {
        AllyMothership = transform.position;
        transform.LookAt(EnemyMothership);
        float thetaInc = (Mathf.PI * 2.0f) / waypointCount;
        dist = Vector3.Distance(transform.position, EnemyMothership);
        radius = dist/2;
        basis = (EnemyMothership-transform.position)/2;
        basis += transform.position - (new Vector3(1, 1, 1));
        for (int i = 0; i < waypointCount; i++)
        {
            float theta = i * thetaInc;

            Vector3 pos = new Vector3();
            pos.x = Mathf.Sin(theta) * radius;
            pos.z = Mathf.Cos(theta) * radius;
            pos.y = 0;
            // Alternatively, use a quaternion
            //Quaternion q = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, Vector3.up);
            //Vector3 pos = q * basis;
            waypoints.Add(pos+basis);
        }
    }
    float speed = 4.0f;
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, waypoints[curent]);
        if (dist < 0.5f)
        {
            curent = (curent + 1) % waypoints.Count;
        }
        transform.LookAt(waypoints[curent]);
        
        transform.position = Vector3.MoveTowards(transform.position, waypoints[curent], speed * Time.deltaTime);

        // Or use translate
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
}