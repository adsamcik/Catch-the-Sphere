using UnityEngine;
using System.Collections;

public class InvisibleHit : MonoBehaviour {

	
	void Update () {
	
	}

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Sphere") StartCoroutine("Show");
    }

}
