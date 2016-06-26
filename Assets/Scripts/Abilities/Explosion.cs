using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : Ability {
    const float maxDist = 25;

    List<GameObject> inRange;
    GameObject gameObject;
    public void FixedUpdate(Rigidbody rigidbody) {
        
    }

    public void Initialize(GameObject g) {
        gameObject = g;
        SphereCollider sc = gameObject.AddComponent<SphereCollider>();
        sc.radius = maxDist;
    }

    public void OnFieldEnter(GameObject g) {
        inRange.Add(g);
    }

    public void OnFieldExit(GameObject g) {
        inRange.Add(g);
    }

    public int Pop() {
        int val = 0;
        foreach (var item in inRange)
            val += Mathf.RoundToInt(maxDist - Vector3.Distance(item.transform.position, gameObject.transform.position));
        return val;
    }

    public IEnumerator PopAnimation(System.Action func) {
        yield return new WaitForEndOfFrame();
    }
}
