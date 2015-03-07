using UnityEngine;
using System.Collections;

public class Up : MonoBehaviour {
    Vector3 ForceUp = new Vector3(0,250f,0);
	// Use this for initialization
    void OnTriggerStay(Collider other) {
        if(other.rigidbody) other.rigidbody.AddForce(ForceUp);
    }
}
