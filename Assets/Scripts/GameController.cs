using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.UI;
using System.Linq;
using Abilities;
using System.IO;

public class GameController : MonoBehaviour {
    public const string ABILITY_FILE = "abilities";

    public static Color ambientLight { get { return _aLight; } }
    static Color _aLight;

    public static Color sunLight { get { return _sLight; } }
    static Color _sLight;

    public static Light sun { get { return instance._sun; } set { instance._sun = value; } }
    public Light _sun;
    //Instance - eliminates the requirement for lookups
    public static GameController instance;


    public static Ability[] abilityList {
        get {
            if (_abilityList == null)
                _abilityList = System.Reflection.Assembly.GetAssembly(typeof(Ability)).GetTypes()
          .Where(x => typeof(Ability).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && x != typeof(Standard))
          .Select(x => (Ability)System.Activator.CreateInstance(x)).ToArray();
            return _abilityList;
        }
    }
    static Ability[] _abilityList;

    static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

    public GameObject pauseMenu;

    /*Set automagically*/
    GameObject sphere;

    GameStats _score;
    public static GameStats score { get { return instance._score; } }

    public float speed = 2;

    int _active;
    public int active { get { return _active; } set { _active = value; } }

    public int spawned { get { return score.spawned; } }

    int _destroyed;
    public static int destroyed { get { return instance._destroyed; } set { instance._destroyed = value; instance._score.SpawnedSphere(); } }

    public static bool paused;

    public static Vector3 randomPositionInSphere { get { return instance.transform.position + Random.insideUnitSphere * instance.spawnRadius; } }

    /*Spheres with abilities*/
    public List<AbilityInfo> abilities;
    bool initialized = false;

    public Standard standard = new Standard();
    float totalSpawnValue;

    const float INCREASE_CHANCE_BY = 0.1f;
    const float BASE_CHANCE_TO_SPAWN_SPECIAL = 0.1f;
    float chanceToSpawnSpecial = BASE_CHANCE_TO_SPAWN_SPECIAL;

    float spawnRadius;

    void Awake() {
        instance = this;
        paused = false;
    }

    AbilityInfo[] LoadAbilities() {
        return JsonUtility.FromJson<AbilityInfoList>(Resources.Load<TextAsset>(ABILITY_FILE).text).array;
    }

    public void Initialize() {
        if (initialized)
            return;
        else
            initialized = true;
        abilities = new List<AbilityInfo>();

        var list = LoadAbilities();

        if (list != null) {
            if (!Application.isPlaying)
                abilities.AddRange(list);
            else
                abilities.AddRange(list.Where(x => x.enabled == true).ToArray());
        } else if (!Application.isPlaying) {
            foreach (var a in abilityList)
                abilities.Add(new AbilityInfo(a, 1, true));
        } else {
            throw new System.Exception("ABILITY DATA ARE NULL");
        }

        _score = new GameStats(this, transform.root.Find("/canvas"));
    }

    void Start() {
        Initialize();
        foreach (var ability in abilities)
            totalSpawnValue += ability.chanceToSpawn;

        spawnRadius = transform.localScale.x - 0.5f;
        ChangeSeed();
        StartCoroutine("Spawn");

        _aLight = RenderSettings.ambientLight;
        _sLight = sun.color;
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
        RenderSettings.fogDensity = paused ? 0.16f : 0;
    }

    Ability GetRandomAbility(List<AbilityInfo> al, ref float spawnValue) {
        var rand = Random.Range(0, spawnValue);
        float currentValue = 0;
        for (int i = 0; i < al.Count; i++) {
            var a = al[i];
            currentValue += a.chanceToSpawn;
            if (currentValue >= rand) {
                spawnValue -= a.chanceToSpawn;
                al.RemoveAt(i);
                return a.ability;
            }
        }
        return null;
    }

    IEnumerator Spawn() {
        sphere = Resources.Load<GameObject>("SphereMed");

        while (true) {
            yield return new WaitForSeconds(speed);
            if (!paused) {
                score.SpawnedSphere();
                GameObject g = (GameObject)Instantiate(sphere, randomPositionInSphere, new Quaternion());
                SphereStats s = g.GetComponent<SphereStats>();
                if (Random.value <= chanceToSpawnSpecial) {
                    float abilityChance = 1f;
                    List<AbilityInfo> ab = new List<AbilityInfo>(abilities);
                    float spawnValue = totalSpawnValue;

                    while (ab.Count > 0) {
                        if (Random.value <= abilityChance) {
                            Ability a = GetRandomAbility(ab, ref spawnValue);
                            s.AddAbility(a);
                            if (a.GetType() == typeof(Parasite))
                                break;
                        } else break;

                        abilityChance /= 4;

                    }
                    chanceToSpawnSpecial = BASE_CHANCE_TO_SPAWN_SPECIAL;
                } else {
                    s.AddAbility(standard);
                    chanceToSpawnSpecial += INCREASE_CHANCE_BY;
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
        _score.resultsActive = true;
        _score.Summary();
        StartCoroutine("RestartIn");
    }

    void Restart() {
        ChangeSeed();
        destroyed = 0;
        speed = 2;
        _score = new GameStats(this, transform.root.Find("/canvas"));
    }

    void ChangeSeed() {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        Random.InitState(System.BitConverter.ToInt32(data, 0));
    }

    public static void Pop(int value) {
        instance._score.AddScore(value);
    }

}

