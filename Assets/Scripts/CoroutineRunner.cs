using UnityEngine;
using System.Collections;

public class CoroutineRunner : MonoBehaviour {
    public static CoroutineRunner instance;

    void Start() {
        instance = this;
    }
}
