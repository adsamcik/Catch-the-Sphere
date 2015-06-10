using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {
    Rigidbody r;

    void Awake() {
        r = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other) {
        float dir1, dir2;
        if (other.gameObject.CompareTag("Sphere")) {
            if (Random.Range(0, 4) > 2) dir1 = Random.Range(1000, 1250);
            else dir1 = Random.Range(-1250, -1000);

            if (Random.Range(0, 4) > 2) dir2 = Random.Range(1000, 1250);
            else dir2 = Random.Range(-1250, -1000);

            Vector3 Force = new Vector3(dir1, Random.Range(0, 400), dir2);
            if (r.velocity.x + r.velocity.z < 200) {
                r.AddForce(-(Force));
                other.rigidbody.AddForce(Force);
            }
            else {
                Vector3 CubeForce = new Vector3(r.velocity.x * Random.Range(3, 8), Random.Range(250, 1500), r.velocity.z * Random.Range(3, 8));
                r.AddForce(CubeForce);
                other.rigidbody.AddForce(Force + CubeForce);
            }
        }
    }
}
