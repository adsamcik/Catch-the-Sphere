using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class TutorialText : MonoBehaviour {
    bool writing;
    bool show;
    int scoreget, combomodifier;
    public int Active, destroyed;

    GameObject cube;
    GameObject sphere;

    List<GameObject> instances = new List<GameObject>();

    Text textObject;

    void Awake() {
        textObject = GetComponent<Text>();
        Application.targetFrameRate = 60;
        StartCoroutine("Write", "This tutorial will guide you through basic mechanics of Catch the Sphere");
        StartCoroutine("Guide");

        sphere = (GameObject)Resources.Load("SphereMed");
        cube = (GameObject)Resources.Load("Cube");
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
        StartCoroutine("Write", "In normal mode, you have cube that randomly pushes spheres around");
        StartCoroutine("Part0");
        while (writing) yield return new WaitForFixedUpdate();

        yield return new WaitForSeconds(2.5f);
        DestroyAll();
        StartCoroutine("Write", "Spheres are your main objective. The faster they are, the more points you get.");

        while (writing) yield return new WaitForFixedUpdate();

        yield return new WaitForSeconds(5f);

        StartCoroutine("Write", "Let's try it.");
        StartCoroutine("Part1");
        while (show) yield return new WaitForFixedUpdate();
        StartCoroutine("Write", "That one was slow\ndon't you think?");
        while (writing) {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Part2");
        while (show) yield return new WaitForFixedUpdate();

        StartCoroutine("Write", "You would get " + scoreget + " score points for this one and your modifier would be " + combomodifier);

        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(4f);

        StartCoroutine("Write", "Modifier multiplies the score you get from next sphere and it's value is not shown to you");

        while (writing) yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(3.5f);

        StartCoroutine("Write", "There are several types  of special spheres (this one explodes)");
        StartCoroutine("Part3");
        while (show) {
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine("Write", "Special spheres have different colors and effects. Tap when ready to start the real game!");
        while (writing) {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);

        while (Input.touchCount == 0 || !Input.GetMouseButtonDown(0)) {
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Normal");
    }

    IEnumerator Part0() {
        show = true;
        instances.Add(Instantiate(cube));
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
        GameObject Sphere = Instantiate(Resources.Load("Exploding Sphere")) as GameObject;

        GameObject Sphere1 = Instantiate(sphere, new Vector3(1, 6, 0), new Quaternion()) as GameObject;
        GameObject Sphere2 = Instantiate(sphere, new Vector3(0, 6, 1), new Quaternion()) as GameObject;
        GameObject Sphere3 = Instantiate(sphere, new Vector3(1, 6, 1), new Quaternion()) as GameObject;

        Destroy(Sphere1.GetComponent<Move>());
        Sphere1.SetActive(true);
        Destroy(Sphere2.GetComponent<Move>());
        Sphere2.SetActive(true);
        Destroy(Sphere3.GetComponent<Move>());
        Sphere3.SetActive(true);

        yield return new WaitForSeconds(1f);
        while (Sphere != null) yield return new WaitForFixedUpdate();
        show = false;
    }

    void DestroyAll() {
        for (int i = 0; i < instances.Count; i++) {
            Destroy(instances[i]);
        }

        instances.Clear();
    }
}
