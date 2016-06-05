using UnityEngine;
using System.Collections;

public interface Ability {
    void Initialize(GameObject g);
    void OnFieldEnter(GameObject g);
    void OnFieldExit(GameObject g);
    int Pop();
    void FixedUpdate(Rigidbody rigidbody);
    IEnumerator PopAnimation();
}
