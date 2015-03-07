using UnityEngine;
using System.Collections;

public class MobileControlls : MonoBehaviour
{

    GameObject go;
    float finger;
    RaycastHit hit = new RaycastHit();
    public LayerMask mask;
    Touch touch;

    public GameController GameController;

#if UNITY_ANDROID || UNITY_IOS
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) GameController.Pause();

        if (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).deltaTime < 1f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.SphereCast(ray, 0.4f, out hit))
            {
                hit.transform.gameObject.GetComponent<Move>().Touched();
            }
        }
    }
}

#else
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) GameController.Pause();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(ray, 0.4f, out hit, mask))
            {
                hit.transform.gameObject.GetComponent<Move>().Touched();
            }
        }
    }

}
#endif
