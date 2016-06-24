using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {
    static Score instance;
    static GameObject sphere;

    public static Text FinalResults;
    public static Text ScoreToAdd;
    public static Text ScoreText;

    static int comboModifier;

    public static int scoreTemp { get; private set; }
    public static int score { get; private set; }
    static float comboScore, combotimer;
    static float cTimerMax = 2f;

    static string highScoreKey;

    public static bool resultsActive;
    static Vector3 OrigPos;

    void Start() {
        instance = this;
        CheckLevel();
        OrigPos = transform.position;

        FinalResults = transform.parent.Find("Results").GetComponent<Text>();
        ScoreToAdd = transform.Find("ScoreToAdd").GetComponent<Text>();
        ScoreText = transform.Find("ScoreValue").GetComponent<Text>();
    }

    public static void CheckLevel() {
        string level = SceneManager.GetActiveScene().name;
        if (level == "Normal") { highScoreKey = "hs_normal"; }
    }

    static bool SetHighscore() {
        int highscore = score;
        if (PlayerPrefs.GetInt(highScoreKey) <= highscore) {
            PlayerPrefs.SetInt(highScoreKey, highscore);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static int GetHighscore() {
        return PlayerPrefs.GetInt(highScoreKey);
    }

    public static void NoScore() { score = 0; ScoreText.text = "0"; resultsActive = false; }
    public static void Summary() {
        instance.StopCoroutine("ComboTimer");
        CountScore();
        FinalResults.text = "\nYour final score is\n" + score + " points.\n\n";
        if (SetHighscore()) { FinalResults.text += "You are getting better!\nYou have beaten your\nhigh score"; }
        else { FinalResults.text += "You have " + (PlayerPrefs.GetInt(highScoreKey) - score) + " left\n to beat your high score"; }
    }

    public static void AddScoreNoModifier(float spherescore) {
        scoreTemp += Mathf.RoundToInt(spherescore);
        combotimer = cTimerMax;
        ScoreToAdd.text = "+" + scoreTemp;
    }

    public static void AddScore(float spherescore) {
        if (combotimer > 0) {
            scoreTemp += Mathf.RoundToInt(spherescore * comboModifier);
            comboModifier = (comboModifier + Mathf.FloorToInt(spherescore / 25)) / 2;
            combotimer = cTimerMax;
        }
        else {
            scoreTemp += Mathf.RoundToInt(spherescore);
            comboModifier = Mathf.FloorToInt(spherescore / 25);
            combotimer = cTimerMax;
            instance.StartCoroutine("ComboTimer");
        }

        if(ScoreToAdd != null)
            ScoreToAdd.text = (scoreTemp > 0 ? "+" : "") + scoreTemp;
    }

    static void SetScore(int stbs) {
        score += Mathf.RoundToInt(stbs);
        ScoreText.text = score.ToString();
        if (stbs != 0) instance.StartCoroutine("ScoreAddedAnim");
    }

    static void CountScore() {
        SetScore(scoreTemp);
        ScoreToAdd.text = "";
        scoreTemp = 0;
        comboModifier = 0;
        combotimer = 0;
    }

    IEnumerator ComboTimer() {
        while (combotimer > 0) {
            combotimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        CountScore();
    }

    IEnumerator ScoreAddedAnim() {
        while (transform.position.y < OrigPos.y + 1) {
            transform.position += new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForFixedUpdate();
        }

        while (transform.position.y > OrigPos.y) {
            transform.position -= new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForFixedUpdate();
        }

        transform.position = OrigPos;
    }



}
