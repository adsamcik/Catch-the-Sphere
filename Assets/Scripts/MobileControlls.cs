using UnityEngine;
using System.Collections;

public class MobileControlls : MonoBehaviour {
    RaycastHit hit;
    public LayerMask mask;
    public float radius = 0.5f;

    void Update() {
        if (Input.GetButtonDown("Pause")) GameController.Pause();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).deltaTime < 1f)
#else
        if (Input.GetMouseButtonDown(0))
#endif
 {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 2, true);
            if (Physics.SphereCast(ray, radius, out hit, 100, mask, QueryTriggerInteraction.Ignore))
                GameController.Pop(hit.transform.GetComponent<SphereStats>().Pop());
        }
    }

}

