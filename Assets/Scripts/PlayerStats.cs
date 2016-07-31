using UnityEngine;
using System.Collections;

public static class PlayerStats {

    public const int DEFAULT_POWER = 1000;
    public const string PLAYER_KEY = "PLAYAKEYA";

    public static int power { get { return stats.power; } private set { stats.power = value; stats.Save(); } }
    public static int spheresSpawned { get { return stats.spheresSpawned; } private set { stats.spheresSpawned = value; stats.Save(); } }
    public static int spheresCaught { get { return stats.spheresCaught; } private set { stats.spheresCaught = value; stats.Save(); } }

    static Stats stats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(PLAYER_KEY, "{}"));

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
