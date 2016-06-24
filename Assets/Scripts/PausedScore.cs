using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PausedScore : MonoBehaviour {
    public Text HighscoreText;
    public Text ScoreText;

    public Score Score;

    void Awake() {
        int highscore = Score.GetHighscore();
        int score = Score.score;

        HighscoreText.text = "Highscore " + highscore;

        ScoreText.text = "Score " + score;
    }
}
