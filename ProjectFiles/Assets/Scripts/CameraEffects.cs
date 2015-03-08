using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour {
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    float length;

    // Amplitude of the shake. A larger value shakes the camera harder.
    float strength = 0.7f;

    Vector3 originalPos;

    void Awake() {
        if (camTransform == null) {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable() {
        originalPos = camTransform.localPosition;
    }

    public void ShakeCamera(float _length, float _strength) {
        length = _length;
        strength = _strength;
        StartCoroutine("Shake");
    }

    public void ShakeCamera(float _length) {
        length = _length;
        StartCoroutine("Shake");
    }

    IEnumerator Shake() {
        while (length > 0) {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * strength;
            length -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        length = 0f;
        camTransform.localPosition = originalPos;
    }

}