using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {

    GameObject Collider;

    Vector3 Velocity;
    bool frozen;

    float accelerometerUpdateInterval = 1.0f / 60.0f;

    // The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 1.5f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    private Vector3 acceleration;
    private Vector3 deltaAcceleration;

    void Start() { 
        StartCoroutine("IsInside");
        Collider = GameObject.Find("SphereCollider");
        Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(),Collider.collider);
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds; 
    }

    void Update() {
        acceleration = Input.acceleration;

        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);

        deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            rigidbody.AddForce(100*deltaAcceleration);
        }
    }
    IEnumerator IsInside()
    {
        while (true)
        {
            if (transform.position.y < -10) { Instantiate(Resources.Load("Cube")); Destroy(gameObject); }
            yield return new WaitForSeconds(1);
        }

    }

    void OnCollisionEnter(Collision other)
    {
        float dir1, dir2;
        if (other.gameObject.tag == "Sphere")
        {
            if (Random.Range(0, 4) > 2) dir1 = Random.Range(1000, 1250);
            else dir1 = Random.Range(-1250, -1000);

            if (Random.Range(0, 4) > 2) dir2 = Random.Range(1000, 1250);
            else dir2 = Random.Range(-1250, -1000);

			Vector3 Force = new Vector3(dir1, Random.Range(0, 400), dir2);
			if (rigidbody.velocity.x + rigidbody.velocity.z < 200) {
				rigidbody.AddForce(-(Force));
				other.rigidbody.AddForce(Force);
			}
			else {
				Vector3 CubeForce = new Vector3(rigidbody.velocity.x * Random.Range(3, 8), Random.Range(250, 1500), rigidbody.velocity.z * Random.Range(3, 8));
				rigidbody.AddForce(CubeForce);
				other.rigidbody.AddForce(Force + CubeForce);
			}
        }
    }

    public void Freeze()
    {
        if (frozen == true)
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = Velocity;
            frozen = false;
        }
        else
        {
            Velocity = rigidbody.velocity;
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            frozen = true;
        }
    }
}
