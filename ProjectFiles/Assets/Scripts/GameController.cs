using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
    //Instance - eliminates the requirement for lookups
    public static GameController instance;

    static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

    /*Need to be set in editor*/
    public Score score;
    public Text finalResults;
    public Text spawnInfo;
    public GameObject pauseMenu;

    /*Set automagically*/
    GameObject sphere;

    public float speed = 2;

    int _spawned;
    public int spawned { get { return _spawned; } set { _spawned = value; UpdateSphereCount(); } }

    int _destroyed;
    public int destroyed { get { return _destroyed; } set { _destroyed = value; UpdateSphereCount(); } }

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

    public void UpdateSphereCount() {
        spawnInfo.text = destroyed + "/" + spawned;
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
        RenderSettings.fogDensity = paused ? 0.12f : 0;
    }

    IEnumerator Spawn() {
        if (QualitySettings.GetQualityLevel() < 2) sphere = Resources.Load("SphereLow") as GameObject;
        else if (QualitySettings.GetQualityLevel() == 2) sphere = Resources.Load("SphereMed") as GameObject;
        else sphere = Resources.Load("SphereHigh") as GameObject;

        while (true) {
            yield return new WaitForSeconds(speed);
            if (!paused) {
                if (spawned < 20 && (spawned - destroyed) < 6) {
                    spawned++;
                    Vector2 Circle = Random.insideUnitCircle * 5;
                    if ((spawned) % 7 == 0) Instantiate(Resources.Load(AbilitySpheres[Mathf.RoundToInt(Random.Range(0, AbilitySpheres.Count))].name), new Vector3(Circle.x, 6, Circle.y), new Quaternion());
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
        score.resultsActive = true;
        score.Summary();
        StartCoroutine("RestartIn");
    }

    void Restart() {
        ChangeSeed();
        destroyed = 0;
        spawned = 0;
        speed = 2;
        finalResults.text = "";
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

