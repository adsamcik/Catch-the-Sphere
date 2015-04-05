using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;


public class GameController : MonoBehaviour {
    //Instance - eliminates the requirement for lookups
    public static GameController instance;

    static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

    /*Need to be set in editor*/
    public Score score;
    public GameObject finalResults;
    public GameObject spawnInfo;
    public GameObject pauseMenu;

    /*Set automagically*/
    GameObject sphere;

    public float speed = 2;
    public int Active;
    public int destroyed;

    public static bool paused;

    /*Spheres with abilities*/
    public List<GameObject> AbilitySpheres = new List<GameObject>();

    void Awake() {
        instance = this;
    }

    void Start() {
        ChangeSeed();
        StartCoroutine("Spawn");
        Instantiate(Resources.Load("Cube"));
    }

    void Update() {
        spawnInfo.GetComponent<TextMesh>().text = destroyed + "/" + Active;
    }

    /// <summary>
    /// Unity GUI does not support static methods
    /// </summary>
    public void uGUIPause() {
        Pause();
    }

    public static void Pause() {
        paused = !paused;
        instance.pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    public void BackToMenu() {
        Application.LoadLevel(0);
    }

    IEnumerator Spawn() {
        if (QualitySettings.GetQualityLevel() < 2) sphere = Resources.Load("SphereLow") as GameObject;
        else if (QualitySettings.GetQualityLevel() == 2) sphere = Resources.Load("SphereMed") as GameObject;
        else sphere = Resources.Load("SphereHigh") as GameObject;

        while (true) {
            yield return new WaitForSeconds(speed);
            if (!paused) {
                if (Active < 20 && (Active - destroyed) < 6) {
                    Active++;
                    Vector2 Circle = Random.insideUnitCircle * 5;
                    if ((Active) % 7 == 0) Instantiate(Resources.Load(AbilitySpheres[Mathf.RoundToInt(Random.Range(0, AbilitySpheres.Count))].name), new Vector3(Circle.x, 6, Circle.y), new Quaternion());
                    else Instantiate(sphere, new Vector3(Circle.x, 6, Circle.y), new Quaternion());
                }
            }
        }
    }

    IEnumerator RestartIn() {
        yield return new WaitForSeconds(2);
        while (Input.touchCount == 0 || Input.GetMouseButtonDown(0)) yield return new WaitForFixedUpdate();
        Restart();
    }

    public void Results() {
        score.resultsactive = true;
        score.Summary();
        StartCoroutine("RestartIn");
    }

    void Restart() {
        ChangeSeed();
        destroyed = 0;
        Active = 0;
        speed = 2;
        finalResults.GetComponent<TextMesh>().text = "";
        score.NoScore();
    }

    public void AddScore(float scoresend) {
        score.AddScore(scoresend);
    }

    public void AddScoreNoModifier(float scoresend) {
        score.AddScoreNoModifier(scoresend);
    }

    void ChangeSeed() {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        Random.seed = System.BitConverter.ToInt32(data, 0);
    }
}

