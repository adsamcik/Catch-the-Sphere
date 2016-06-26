using UnityEngine;
using System.Collections;

public class MobileControlls : MonoBehaviour {
    RaycastHit hit;
    public LayerMask mask;

    void Update() {
        if (Input.GetButtonDown("Pause")) GameController.Pause();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).deltaTime < 1f)
#else
        if (Input.GetMouseButtonDown(0))
#endif
 {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(ray, 0.4f, out hit, mask)) {
                hit.transform.SendMessage("Touched", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

}

