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

        FinalResults = transform.parent.Find("Results").GetComponent<Text>();
        ScoreToAdd = transform.Find("ScoreToAdd").GetComponent<Text>();
        ScoreText = transform.Find("ScoreValue").GetComponent<Text>();
        OrigPos = ScoreText.transform.position;
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

    public static void AddScore(int value) {
        scoreTemp += value;

        if(ScoreToAdd != null)
            ScoreToAdd.text = (scoreTemp > 0 ? "+" : "") + scoreTemp;
    }

    static void SetScore(int stbs) {
        score += Mathf.RoundToInt(stbs);
        ScoreText.text = score.ToString();
        if (stbs != 0) instance.StartCoroutine(instance.ScoreAddedAnim());
    }

    static void CountScore() {
        SetScore(scoreTemp);
        ScoreToAdd.text = "";
        scoreTemp = 0;
        comboModifier = 0;
        combotimer = 0;
    }

    IEnumerator ScoreAddedAnim() {
        while (ScoreText.transform.position.y < OrigPos.y + 2) {
            ScoreText.transform.position += new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForFixedUpdate();
        }

        while (ScoreText.transform.position.y > OrigPos.y) {
            ScoreText.transform.position -= new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForFixedUpdate();
        }

        ScoreText.transform.position = OrigPos;
    }



}
