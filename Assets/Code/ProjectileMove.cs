using UnityEngine;
using System.Collections;

public class ProjectileMove : MonoBehaviour {
    string sourceTag;
    // Use this for initialization
    void Start()
    {
    }

    void Initialise(string tag)
    {
        sourceTag = tag;
    }
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward.normalized*Time.deltaTime;
	}
}
