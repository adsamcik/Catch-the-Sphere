using UnityEngine;
using System.Collections;
public class MenuControlls : MonoBehaviour {

    GameObject go;
    float finger;
    RaycastHit hit = new RaycastHit();
    Touch touch;

    public Vector3 HomePos;
    public Vector3 AboutPos;
    public Vector3 SettingsPos;
    Vector3 SpherePosition = new Vector3(-10, 1, 1.7f);
    Vector3 SphereScale = new Vector3(2.2f, 2.2f, 2.2f);

    int gk;

    public TextMesh GraphicsQuality;

    public GameObject MobileController;
    public GameController GameController;

    public TextMesh FinalResults;

    GameObject LowSphere;
    GameObject MedSphere;
    GameObject HighSphere;
    GameObject ActiveSphere;

    void Start() {
        if (Application.loadedLevel == 0) {
            LowSphere = Instantiate(Resources.Load("SphereLow"), SpherePosition, new Quaternion()) as GameObject;
            MedSphere = Instantiate(Resources.Load("SphereMed"), SpherePosition, new Quaternion()) as GameObject;
            HighSphere = Instantiate(Resources.Load("SphereHigh"), SpherePosition, new Quaternion()) as GameObject;

            LowSphere.SetActive(false);
            MedSphere.SetActive(false);
            HighSphere.SetActive(false);

            Destroy(LowSphere.GetComponent<Move>());
            Destroy(LowSphere.GetComponent<Rigidbody>());
            Destroy(LowSphere.GetComponent<SphereCollider>());
            LowSphere.transform.localScale = SphereScale;

            Destroy(MedSphere.GetComponent<Move>());
            Destroy(MedSphere.GetComponent<Rigidbody>());
            Destroy(MedSphere.GetComponent<SphereCollider>());
            MedSphere.transform.localScale = SphereScale;

            Destroy(HighSphere.GetComponent<Move>());
            Destroy(HighSphere.GetComponent<Rigidbody>());
            Destroy(HighSphere.GetComponent<SphereCollider>());
            HighSphere.transform.localScale = SphereScale;
        }

    }

    void OnEnable() {
        if (Application.loadedLevel == 2) {
            MobileController.SetActive(false);
            FinalResults.gameObject.SetActive(false);
            StartCoroutine("ChangeState", new Vector3(Camera.main.transform.position.x, 16, Camera.main.transform.position.z));
        }
        else if (Application.loadedLevel == 1) {
            MobileController.SetActive(false);
            StartCoroutine("ChangeState", new Vector3(Camera.main.transform.position.x, 16, Camera.main.transform.position.z));
        }
    }

    IEnumerator ChangeState(Vector3 finpos) {
        float frac = 0;
        float i = 8;
        while (transform.position != finpos) {
            Camera.main.transform.position = Vector3.Lerp(transform.position, finpos, frac);
            frac += Time.deltaTime * i;
            yield return new WaitForFixedUpdate();
            if (i > 1) i -= Time.deltaTime * 6;
        }
    }

    IEnumerator Disable() { yield return null; }

    IEnumerator Continue() {
        Camera.main.transform.position = new Vector3(0, 12, 0);
        MobileController.SetActive(true);
        if (Application.loadedLevel == 2) { GameController.Pause(); FinalResults.gameObject.SetActive(true); }
        else GameObject.Find("Text").GetComponent<Text>().Pause();

        gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator EndGame() {
        Application.LoadLevel(0);
        yield return null;
    }

    IEnumerator About() {
        float i = 0;
        while (transform.position != AboutPos) {
            transform.position = Vector3.Lerp(transform.position, AboutPos, i);
            i += Time.deltaTime * 2;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Home() {
        float i = 0;
        while (transform.position != HomePos) {
            transform.position = Vector3.Lerp(transform.position, HomePos, i);
            i += Time.deltaTime * 2;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Settings() {
        gk = QualitySettings.GetQualityLevel();
        if (gk == 0) { GraphicsQuality.text = "No shadows"; LowSphere.SetActive(true); }
        else if (gk == 1) { GraphicsQuality.text = "Good"; LowSphere.SetActive(true); }
        else if (gk == 2) { GraphicsQuality.text = "Better"; MedSphere.SetActive(true); }
        else if (gk == 3) { GraphicsQuality.text = "Excellent"; HighSphere.SetActive(true); }

        float i = 0;

        while (transform.position != SettingsPos) {
            transform.position = Vector3.Lerp(transform.position, SettingsPos, i);
            i += Time.deltaTime * 2;
            yield return new WaitForFixedUpdate();
        }


    }

    IEnumerator Normal() {
        Application.LoadLevel(hit.transform.name);
        yield return null;
    }
    IEnumerator Tutorial() {
        Application.LoadLevel(hit.transform.name);
        yield return null;
    }

    IEnumerator Quality() {
        if (gk == 0) { gk = 1; GraphicsQuality.text = "Good"; }
        else if (gk == 1) { gk = 2; GraphicsQuality.text = "Better"; LowSphere.SetActive(false); MedSphere.SetActive(true); }
        else if (gk == 2) { gk = 3; GraphicsQuality.text = "Excellent"; MedSphere.SetActive(false); HighSphere.SetActive(true); }
        else if (gk == 3) { gk = 1; GraphicsQuality.text = "Good"; HighSphere.SetActive(false); LowSphere.SetActive(true); }

        QualitySettings.SetQualityLevel(gk);
        yield return null;
    }

    void Update() {
        if (MobileController) { if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine("Continue"); } }
        if (transform.position == SettingsPos || transform.position == AboutPos) { if (Input.GetKeyDown(KeyCode.Escape)) StartCoroutine("Home"); }
        else if (transform.position == HomePos) if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).deltaTime < 1f) {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.SphereCast(ray, 0.4f, out hit)) {
                StartCoroutine(hit.transform.name, hit);
            }
        }
    }


    // PC controls
    //    void Update()
    //    {
    //        if (MobileController) if (Input.GetKeyDown(KeyCode.Escape)) StartCoroutine("Continue");
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //            if (Physics.SphereCast(ray, 0.4f, out hit))
    //            {
    //                StartCoroutine(hit.transform.name, hit);
    //            }
    //        }
    //    }

}

