using UnityEngine;
using System.Collections;

public class BillboardPlane : MonoBehaviour {

    void Start() {
        transform.rotation = Camera.main.transform.rotation;
    }

}
