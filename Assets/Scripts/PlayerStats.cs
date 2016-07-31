using UnityEngine;
using System.Collections;

public static class PlayerStats {

    public const int DEFAULT_POWER = 1000;
    public const string PLAYER_KEY = "PLAYAKEYA";

    public static int power { get { return _power; } private set { _power = value; stats.Save(); } }
    static int _power;

    public static int spheresSpawned { get { return _spheresSpawned; } private set { _spheresSpawned = value; stats.Save(); } }
    static int _spheresSpawned;

    public static int spheresCaught { get { return _spheresCaught; } private set { _spheresCaught = value; stats.Save(); } }
    static int _spheresCaught;

    static Stats stats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(PLAYER_KEY));

    [System.Serializable]
    class Stats {
        public int power = DEFAULT_POWER;
        public int spheresSpawned;
        public int spheresCaught;

        public void Save() {
            PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }
    }
}
