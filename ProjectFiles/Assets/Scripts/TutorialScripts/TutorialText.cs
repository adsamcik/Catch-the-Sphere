using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TutorialText : MonoBehaviour {
    bool writing;
    bool show;
    int scoreget, combomodifier;
    public int Active, destroyed;

    GameObject Cube;
    GameObject sphere;

    public GameObject PauseMenu;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 1.5f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    private Vector3 acceleration;
    private Vector3 deltaAcceleration;

	Text textObject;

    void Start() {
		textObject = GetComponent<Text>();
        Application.targetFrameRate = 60;
        StartCoroutine("Write", "This tutorial will guide you\nthrough basic mechanics of\nCatch the Sphere");
        StartCoroutine("Guide");

        sphere = Instantiate(Resources.Load("SphereMed")) as GameObject;
        sphere.SetActive(false);

        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
    }

    public void AddScore(float addscore) {
        int scoretemp = Mathf.FloorToInt(addscore);
        scoreget += scoretemp;
        combomodifier = Mathf.FloorToInt(scoretemp / 25);
    }

    IEnumerator Write(string input) {
        writing = true;
        textObject.text = "";
        int length = 1;
        while (true) {
            if (input.Length < 1) break;

            if (input.Length > 1)
                if (input.Substring(0, 2) == "/n") length = 2;
                else length = 1;
            textObject.text += input.Substring(0, length);
            input = input.Substring(length);
            yield return new WaitForFixedUpdate();
        }
        writing = false;
    }

    IEnumerator Guide() {
        while (writing) {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine("Write", "In normal mode, you have cube\nthat randomly pushes\nspheres around");
        StartCoroutine("Part0");
        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(2f);

        StartCoroutine("Write", "If your cube is slow or stucked\n just shake your device.\nTry it now.");
        while (writing) yield return new WaitForFixedUpdate();

        while (deltaAcceleration.sqrMagnitude < shakeDetectionThreshold) {
            acceleration = Input.acceleration;
            lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
            deltaAcceleration = acceleration - lowPassValue;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(2.5f);
        StartCoroutine("Write", "Spheres are your main objective.\nThe faster they are,\nthe more points you get.");

        while (writing) yield return new WaitForFixedUpdate();

        yield return new WaitForSeconds(5f);
        StartCoroutine("Write", "Let's try it.");
        Destroy(Cube);
        StartCoroutine("Part1");
        while (show) yield return new WaitForFixedUpdate();
        StartCoroutine("Write", "That one was slow\ndon't you think?");
        while (writing) {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Part2");
        while (show) yield return new WaitForFixedUpdate();

        StartCoroutine("Write", "You would get " + scoreget + " score points\nfor this one and your modifier\nwould be " + combomodifier);

        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(4f);

        StartCoroutine("Write", "Modifier multiplies the score\nyou get from next sphere.\nModifier is not shown to you");

        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(3.5f);

        StartCoroutine("Write", "However I'll tell you it's\nsphere score divided by 25\nrounded down");

        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(3f);

        StartCoroutine("Write", "It's reseted every 2 seconds\nand if you hit multiple spheres\n modifier is the average of those");

        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(4f);

        StartCoroutine("Write", "There are several types\n of special spheres\n(this one explodes)");
        StartCoroutine("Part3");
        while (show) {
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine("Write", "Special spheres have\ndifferent colors and effects.\nTap when ready to\nstart the real game!");
        while (writing) {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);

        while (Input.touchCount == 0 || Input.GetMouseButtonDown(0)) {
            yield return new WaitForFixedUpdate();
        }


        Application.LoadLevel("Normal");
    }

    IEnumerator Part0() {
        show = true;
        Cube = Instantiate(Resources.Load("Cube")) as GameObject;
        show = false;
        yield return null;
    }

    IEnumerator Part1() {
        show = true;
        GameObject Sphere = Instantiate(sphere, new Vector3(0, 6, 0), new Quaternion()) as GameObject;
        Sphere.SetActive(true);
        Destroy(Sphere.GetComponent<Move>());
        Sphere.AddComponent<TutSphereController>();

        Sphere.GetComponent<TutSphereController>().phase = 1;
        yield return new WaitForSeconds(1f);
        while (Sphere) yield return new WaitForFixedUpdate();
        show = false;
    }

    IEnumerator Part2() {
        show = true;
        GameObject Sphere = Instantiate(sphere, new Vector3(0, 6, 0), new Quaternion()) as GameObject;
        Sphere.SetActive(true);
        Destroy(Sphere.GetComponent<Move>());
        Sphere.AddComponent<TutSphereController>();

        Sphere.GetComponent<TutSphereController>().phase = 2;
        Sphere.GetComponent<Rigidbody>().AddForce(750, 500, 750);
        yield return new WaitForSeconds(1f);
        while (Sphere) yield return new WaitForFixedUpdate();
        show = false;
    }

    IEnumerator Part3() {
        show = true;
        GameObject Sphere = Instantiate(Resources.Load("ExplodingTut")) as GameObject;

        GameObject Sphere1 = Instantiate(sphere, new Vector3(1, 6, 0), new Quaternion()) as GameObject;
        GameObject Sphere2 = Instantiate(sphere, new Vector3(0, 6, 1), new Quaternion()) as GameObject;
        GameObject Sphere3 = Instantiate(sphere, new Vector3(1, 6, 1), new Quaternion()) as GameObject;

        Destroy(Sphere1.GetComponent<Move>());
        Sphere1.SetActive(true);
        Destroy(Sphere2.GetComponent<Move>());
        Sphere2.SetActive(true);
        Destroy(Sphere3.GetComponent<Move>());
        Sphere3.SetActive(true);

        Sphere.GetComponent<TutSphereController>().phase = 3;
        yield return new WaitForSeconds(1f);
        while (Sphere) yield return new WaitForFixedUpdate();
        show = false;
    }
}
