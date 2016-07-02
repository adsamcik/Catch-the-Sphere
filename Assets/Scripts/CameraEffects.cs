using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour {

    // How long the object should shake for.
    float length;

    // Amplitude of the shake. A larger value shakes the camera harder.
    float strength = 0.7f;

    Vector3 originalPos;

    void OnEnable() {
        originalPos = transform.localPosition;
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
            transform.localPosition = originalPos + Random.insideUnitSphere * strength;
            length -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        length = 0f;
        transform.localPosition = originalPos;
    }

}