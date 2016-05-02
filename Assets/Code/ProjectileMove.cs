using UnityEngine;
using System.Collections;

public class ProjectileMove : MonoBehaviour {
    public Pilot owner;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(ClearJunk());
    }

    public void Initialise(string source)
    {
        tag = source;
    }
	// Update is called once per frame
	void Update ()
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.right * 100, ForceMode.Acceleration);
        //transform.position += transform.forward*Time.deltaTime*100;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (tag != collision.gameObject.tag)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator ClearJunk()
    {
        yield return new WaitForSeconds(15);
        Destroy(this.gameObject);
    }
}
