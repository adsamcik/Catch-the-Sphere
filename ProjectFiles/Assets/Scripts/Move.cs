using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    RaycastHit hit;

    Vector3 velocity;
    bool paused;
    public bool frozen { get; private set; }
    float punish;

    Rigidbody r;

    void Start() {
        r = GetComponent<Rigidbody>();
        r.drag = Random.Range(0, 2);
        StartCoroutine("IsInside");
    }

    IEnumerator IsInside() {
        while (true) {
            if (Mathf.Abs(transform.position.x) > 8 || Mathf.Abs(transform.position.z) > 8 || transform.position.y < -1) { Instantiate(gameObject, new Vector3(0, 6, 0), new Quaternion()); Destroy(gameObject); }
            yield return new WaitForSeconds(1);
        }
    }

    void FixedUpdate() {
        if (r != null && r.velocity.sqrMagnitude > 5000)
            r.velocity = Vector3.zero;
    }

    public void Touched() {
        StopCoroutine("IsInside");
        GameController.AddScore(10 * (Mathf.Abs(frozen ? velocity.x : r.velocity.x) + Mathf.Abs(frozen ? velocity.z : r.velocity.z) + Mathf.Abs((frozen ? velocity.y : r.velocity.y) / 2f)));
        GameController.destroyed++;
        GetComponent<SphereCollider>().enabled = false;

        Abilities a = GetComponentInChildren<Abilities>();
        if (a != null) a.Activate();
        else StartCoroutine("Puff");
    }

    IEnumerator Puff() {
        for (float i = 0.00f; i < 0.1f; i += Time.deltaTime) {
            transform.localScale -= new Vector3(Time.deltaTime * 10, Time.deltaTime * 10, Time.deltaTime * 10);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    public void Freeze() {
        Rigidbody r = GetComponent<Rigidbody>();
        if (r.isKinematic)
            r.velocity = velocity;
        else {
            velocity = r.velocity;
            r.velocity = Vector3.zero;
        }

        r.isKinematic = !r.isKinematic;
    }

}
