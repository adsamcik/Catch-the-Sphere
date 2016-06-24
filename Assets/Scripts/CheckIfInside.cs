using UnityEngine;
using System.Collections;

public class CheckIfInside : MonoBehaviour {
    public GameController GameController;

    void OnTriggerEnter(Collider other) {
        Destroy(other); GameController.spawned--;
        Debug.Log("Collision");
    }
    void OnTriggerStay(Collider other) {
        Destroy(other);
        GameController.spawned--;
    }
}
