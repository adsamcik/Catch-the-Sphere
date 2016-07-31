using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStats {
    GameController gc;

    public Text resultsText;
    public Text scoreToAddText;
    public Text scoreText;
    public Text spawnInfo;

    public int scoreTemp { get; private set; }
    public int score { get; private set; }
    public int spawned { get; private set; }
    public int poped { get; private set; }

    static Vector3 OrigPos;

    string highScoreKey;

    float timeToReset;

    public GameStats(GameController gc, Transform canvas) {
        this.gc = gc;
        CheckLevel();
        resultsText = canvas.Find("results").GetComponent<Text>();

        scoreToAddText = canvas.Find("scoreToAdd").GetComponent<Text>();
        scoreToAddText.text = "";
        scoreText = canvas.Find("score").GetComponent<Text>();
        scoreText.text = "0";

        spawnInfo = canvas.Find("spawned").GetComponent<Text>();
        spawnInfo.text = "0/0";

        OrigPos = scoreText.transform.position;
    }

    public void CheckLevel() {
        string level = SceneManager.GetActiveScene().name;
        if (level == "Normal") { highScoreKey = "hs_normal"; }
    }

    public void Summary() {
        CountScore();
        resultsText.text = "Your final score is " + score + " points.";
        if (SetHighscore()) { resultsText.text += "You are getting better! You have beaten your high score"; } else { resultsText.text += "You have " + (PlayerPrefs.GetInt(highScoreKey) - score) + " left to beat your high score"; }
    }

    public void SpawnedSphere() {
        spawned++;
        UpdateSphereInfo();
    }

    public void AddScore(int value) {
        scoreTemp += value;
        poped++;
        UpdateSphereInfo();

        if (scoreToAddText != null)
            scoreToAddText.text = (scoreTemp > 0 ? "+" : "") + scoreTemp;

        var temp = timeToReset;
        timeToReset = Time.unscaledTime + 2;
        if (temp < Time.unscaledTime)
            gc.StartCoroutine(ResetWaiter());
    }

    void SetScore(int stbs) {
        score += stbs;
        scoreText.text = score.ToString();
        if (stbs != 0)
            gc.StartCoroutine(ScoreAddedAnim());
    }

    void CountScore() {
        SetScore(scoreTemp);
        scoreToAddText.text = "";
        scoreTemp = 0;
    }

    void UpdateSphereInfo() {
        spawnInfo.text = poped + "/" + spawned;
    }

    IEnumerator ResetWaiter() {
        while (timeToReset > Time.unscaledTime)
            yield return new WaitForSecondsRealtime(timeToReset - Time.unscaledTime + 0.1f);
        CountScore();
    }

    IEnumerator ScoreAddedAnim() {
        while (scoreText.transform.position.y < OrigPos.y + 2) {
            scoreText.transform.position += new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForEndOfFrame();
        }

        while (scoreText.transform.position.y > OrigPos.y) {
            scoreText.transform.position -= new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForEndOfFrame();
        }

        scoreText.transform.position = OrigPos;
    }

    bool SetHighscore() {
        int highscore = score;
        if (PlayerPrefs.GetInt(highScoreKey) <= highscore) {
            PlayerPrefs.SetInt(highScoreKey, highscore);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public int GetHighscore() {
        return PlayerPrefs.GetInt(highScoreKey);
    }
}
