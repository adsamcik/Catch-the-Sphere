using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    const float MAX_SPEED = 20;
    const float MAX_SPEED_SQR = MAX_SPEED * MAX_SPEED;

    Rigidbody r;
    Stats s;
    Vector3 velocity;

    void Start() {
        StartCoroutine("IsInside");
        r = GetComponent<Rigidbody>();
        s = GetComponent<Stats>();
    }

    IEnumerator IsInside() {
        while (true) {
            if (Mathf.Abs(transform.position.x) > 8 || Mathf.Abs(transform.position.z) > 8 || transform.position.y < -1) {
                transform.position = new Vector3(0, 6, 0);
                r.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void FixedUpdate() {
        if (r.velocity.sqrMagnitude > MAX_SPEED_SQR)
            r.velocity *= 0.99f;

        s.AbilityUpdate(r);
    }

    public void ToggleFreeze() {
        if (r.isKinematic)
            r.velocity = velocity;
        else {
            velocity = r.velocity;
            r.velocity = Vector3.zero;
        }

        r.isKinematic = !r.isKinematic;
    }

}
