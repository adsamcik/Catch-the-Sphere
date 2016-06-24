using UnityEngine;
using System.Collections;

public class Standard : MonoBehaviour {
    public float speed = 10;
    public float sqrspeed;
    public new Rigidbody rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        sqrspeed = speed * speed;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(rigidbody.velocity.sqrMagnitude < sqrspeed) {
            rigidbody.AddForce(rigidbody.velocity.normalized, ForceMode.Impulse);
        }
	}
}
