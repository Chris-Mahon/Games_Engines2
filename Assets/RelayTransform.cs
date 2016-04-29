using UnityEngine;
using System.Collections;

public class RelayTransform : MonoBehaviour {
    public Vector3 forward;
	// Use this for initialization
	void Start () {
        forward = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = forward;
        Debug.Log("Location: " + transform.position + "Rotation: " +transform.rotation + "Forward: "+ transform.forward);
	}
}
