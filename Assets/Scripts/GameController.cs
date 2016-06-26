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
    public Text finalResults;
    public Text spawnInfo;
    public GameObject pauseMenu;

    /*Set automagically*/
    GameObject sphere;

    public float speed = 2;

    int _spawned;
    public int spawned { get { return _spawned; } set { _spawned = value; UpdateSphereCount(); } }

    int _destroyed;
    public static int destroyed {
        get {
            return instance._destroyed;
        }
        set {
            instance._destroyed = value;
            instance.UpdateSphereCount();
            if (value >= 20) instance.Results();
        }
    }

    public static bool paused;

    /*Spheres with abilities*/
    public List<GameObject> AbilitySpheres = new List<GameObject>();

    void Awake() {
        instance = this;
        paused = false;
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
        sphere = Resources.Load("SphereMed") as GameObject;

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
        Score.resultsActive = true;
        Score.Summary();
        StartCoroutine("RestartIn");
    }

    void Restart() {
        ChangeSeed();
        destroyed = 0;
        spawned = 0;
        speed = 2;
        finalResults.text = "";
        Score.NoScore();
    }

    public static void AddScore(float scoresend) {
        Score.AddScore(scoresend);
    }

    public static void AddScoreNoModifier(float scoresend) {
        Score.AddScoreNoModifier(scoresend);
    }

    void ChangeSeed() {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        Random.InitState(System.BitConverter.ToInt32(data, 0));
    }
}

