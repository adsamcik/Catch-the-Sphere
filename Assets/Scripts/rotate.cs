using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	void Update () {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
	}
}
