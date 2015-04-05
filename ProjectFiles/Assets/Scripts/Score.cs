using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    GameObject sphere;

    public Text FinalResults;
    public Text ScoreToAdd;
    public Text ScoreText;

    public GameController GameController;

    int comboModifier;

    public int scoreTemp { get; private set; }
    public int score { get; private set; }
    float comboScore, combotimer;
    float cTimerMax = 2f;

    string highScoreKey;

    public bool resultsActive;
    Vector3 OrigPos;

    void Start() {
        CheckLevel();
        OrigPos = transform.position;
    }

    void Update() {
        if (GameController.destroyed == 20 && !resultsActive) {
            GameController.Results();
        }
    }

    public void CheckLevel() {
        string level = Application.loadedLevelName;
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

    public void NoScore() { score = 0; GetComponent<TextMesh>().text = "0"; resultsActive = false; }
    public void Summary() {
        StopCoroutine("ComboTimer");
        CountScore();
        FinalResults.text = "\nYour final score is\n" + score + " points.\n\n";
        if (SetHighscore()) { FinalResults.text += "You are getting better!\nYou have beaten your\nhigh score"; }
        else { FinalResults.text += "You have " + (PlayerPrefs.GetInt(highScoreKey) - score) + " left\n to beat your high score"; }
    }

    public void AddScoreNoModifier(float spherescore) {
        scoreTemp += Mathf.RoundToInt(spherescore);
        combotimer = cTimerMax;
        ScoreToAdd.text = "+" + scoreTemp;
    }

    public void AddScore(float spherescore) {
        if (combotimer > 0) {
            scoreTemp += Mathf.RoundToInt(spherescore * comboModifier);
            comboModifier = (comboModifier + RoundDown(spherescore / 25)) / 2;
            combotimer = cTimerMax;
            ScoreToAdd.text = "+" + scoreTemp;
        }
        else {
            scoreTemp += Mathf.RoundToInt(spherescore);
            comboModifier = RoundDown(spherescore / 25);
            combotimer = cTimerMax;
            StartCoroutine("ComboTimer");
            ScoreToAdd.text = "+" + scoreTemp;
        }
    }

    void SetScore(int stbs) {
        score += Mathf.RoundToInt(stbs);
        ScoreText.text = score.ToString();
        if (stbs != 0) StartCoroutine("ScoreAddedAnim");
    }

    void CountScore() {
        SetScore(scoreTemp);
        ScoreToAdd.text = "";
        scoreTemp = 0;
        comboModifier = 0;
        combotimer = 0;
    }

    int RoundDown(float input) {
        int rounded = Mathf.RoundToInt(input);
        if ((rounded / input) > 1) return rounded - 1;
        else return rounded;
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
