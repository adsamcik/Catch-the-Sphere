using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MobileControlls : MonoBehaviour {

    GameObject go;
    float finger;
    RaycastHit hit = new RaycastHit();
    public LayerMask mask;
    Touch touch;

    public GameController GameController;

    void Update() {
        if (CrossPlatformInputManager.GetButtonDown("Pause")) GameController.Pause();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).deltaTime < 1f)
#else
        if (Input.GetMouseButtonDown(0))
#endif
 {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(ray, 0.4f, out hit, mask)) {
                hit.transform.gameObject.GetComponent<Move>().Touched();
            }
        }
    }

}

