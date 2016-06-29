using UnityEngine;
using System.Collections;

public class Standard : Ability {
    const int MAX_SPEED = 50;
    const int MIN_SPEED = 10;
    const float POP_ANIMATION_LENGTH = 0.5f;

    Rigidbody rigidbody;
    Transform transform;
    float sqrspeed;

    void FixedUpdate() {
        //if (rigidbody.velocity.sqrMagnitude < sqrspeed)
            //rigidbody.AddForce(rigidbody.velocity.normalized, ForceMode.Impulse);
    }

    public void Initialize(GameObject g) {
        rigidbody = g.GetComponent<Rigidbody>();
        transform = g.transform;
        float speed = Random.Range(MIN_SPEED, MAX_SPEED);
        sqrspeed = speed * speed;
    }

    public void OnFieldEnter(GameObject g) {

    }

    public void OnFieldExit(GameObject g) {

    }

    public int Pop() {
        return Mathf.RoundToInt(sqrspeed);
    }

    public void FixedUpdate(Rigidbody rigidbody) {

    }

    public IEnumerator PopAnimation(System.Action func) {
        float mod = transform.localScale.x / POP_ANIMATION_LENGTH;
        while (transform.localScale.x > 0) {
            float val = Time.deltaTime * mod;
            transform.localScale -= new Vector3(val, val, val);
            yield return new WaitForEndOfFrame();
        }
        func();
    }

    public Ability Clone() {
        return new Standard();
    }
}
