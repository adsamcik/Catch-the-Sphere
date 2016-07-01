using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.UI;
using System.Linq;
using Abilities;

public class GameController : MonoBehaviour {
    public static Light sun { get { return instance._sun; } set { instance._sun = value; } }
    public Light _sun;
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
    public static int destroyed
    {
        get
        {
            return instance._destroyed;
        }
        set
        {
            instance._destroyed = value;
            instance.UpdateSphereCount();
            if (value >= 20) instance.Results();
        }
    }

    public static bool paused;

    public static Vector3 randomPositionInSphere
    {
        get { return instance.transform.position + Random.insideUnitSphere * instance.spawnRadius; }
    }

    /*Spheres with abilities*/
    public List<AbilityInfo> abilities = new List<AbilityInfo>();
    Standard standard;
    float totalSpawnValue;

    const float INCREASE_CHANCE_BY = 0.1f;
    const float BASE_CHANCE_TO_SPAWN_SPECIAL = 0.1f;
    float chanceToSpawnSpecial = BASE_CHANCE_TO_SPAWN_SPECIAL;

    float spawnRadius;

    public void Awake() {
        instance = this;
        paused = false;
        var interfaceType = typeof(Ability);
        var all = System.AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(x => x.GetTypes())
          .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
          .Select(x => System.Activator.CreateInstance(x));
        foreach (var ability in all) {
            if (standard == null && ability.GetType() == typeof(Standard))
                standard = (Standard)ability;
            else  if (abilities.FindIndex(x => x.ability.GetType() == ability.GetType()) == -1)
                abilities.Add(new AbilityInfo((Ability)ability, 1, true));
        }

        foreach (var ability in abilities)
            totalSpawnValue += ability.chanceToSpawn;
    }

    void Start() {
        spawnRadius = transform.localScale.x - 0.5f;
        ChangeSeed();
        StartCoroutine("Spawn");
        //Instantiate(Resources.Load("Cube"));
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
                    if (Random.value <= chanceToSpawnSpecial) {
                        GameObject g = (GameObject)Instantiate(sphere, randomPositionInSphere, new Quaternion());
                        Stats s = g.GetComponent<Stats>();
                        float abilityChance = 1f;
                        List<AbilityInfo> ab = new List<AbilityInfo>();
                        foreach (var ability in abilities)
                            if (ability.enabled == true)
                                ab.Add(ability);

                        while (ab.Count > 0) {
                            if (Random.value <= abilityChance) {
                                int rand = Random.Range(0, ab.Count);
                                s.AddAbility(ab[rand].ability);
                                ab.RemoveAt(rand);
                            }
                            else break;

                            abilityChance /= 4;

                        }
                        chanceToSpawnSpecial = BASE_CHANCE_TO_SPAWN_SPECIAL;
                    }
                    else {
                        Instantiate(sphere, randomPositionInSphere, new Quaternion());
                        chanceToSpawnSpecial += INCREASE_CHANCE_BY;
                    }
                    Debug.Log(chanceToSpawnSpecial);
                    //if ((spawned) % 7 == 0) Instantiate(Resources.Load(AbilitySpheres[Mathf.RoundToInt(Random.Range(0, AbilitySpheres.Count))].name), new Vector3(Circle.x, 6, Circle.y), new Quaternion());
                    //else Instantiate(sphere, new Vector3(Circle.x, 6, Circle.y), new Quaternion());
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

