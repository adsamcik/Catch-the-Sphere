using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {
    Rigidbody r;
    public int size;

    void Awake() {
        r = GetComponent<Rigidbody>();
    }

    public void AddSphere() {
        size++;
        float s = size * 0.1f;
        transform.localScale = new Vector3(s, s, s);
    }
}
