using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score {
    GameController gc;

    public Text resultsText;
    public Text scoreToAddText;
    public Text scoreText;

    public int scoreTemp { get; private set; }
    public int score { get; private set; }

    public bool resultsActive;
    static Vector3 OrigPos;

    string highScoreKey;

    float timeToReset;

    public Score(GameController gc, Transform canvas) {
        this.gc = gc;
        CheckLevel();
        resultsText = canvas.Find("results").GetComponent<Text>();

        Transform u = canvas.Find("upper");
        scoreToAddText = u.Find("scoreToAdd").GetComponent<Text>();
        scoreText = u.Find("score").GetComponent<Text>();

        OrigPos = scoreText.transform.position;
    }

    public void CheckLevel() {
        string level = SceneManager.GetActiveScene().name;
        if (level == "Normal") { highScoreKey = "hs_normal"; }
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

    public void NoScore() {
        score = 0;
        scoreText.text = "0";
        resultsActive = false;
    }
    public void Summary() {
        CountScore();
        resultsText.text = "Your final score is " + score + " points.";
        if (SetHighscore()) { resultsText.text += "You are getting better! You have beaten your high score"; } else { resultsText.text += "You have " + (PlayerPrefs.GetInt(highScoreKey) - score) + " left to beat your high score"; }
    }

    public void AddScore(int value) {
        scoreTemp += value;

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



}
