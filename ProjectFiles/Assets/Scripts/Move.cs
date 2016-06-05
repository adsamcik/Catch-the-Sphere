using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour {
    Rigidbody r;
    Vector3 velocity;
    Ability ability;

    void Start() {
        StartCoroutine("IsInside");
        r = GetComponent<Rigidbody>();
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
        ability.FixedUpdate(r);
    }

    public void Touched() {
        StopCoroutine("IsInside");
        GetComponent<SphereCollider>().enabled = false;
        enabled = false;

        GameController.AddScore(ability.Pop());
        ability.PopAnimation();

        GameController.destroyed++;
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
