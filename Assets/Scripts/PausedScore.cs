using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PausedScore : MonoBehaviour {
    public Text HighscoreText;
    public Text ScoreText;

    public Score Score;

    void Awake() {
        HighscoreText.text = "Highscore " + GameController.score.GetHighscore();
        ScoreText.text = "Score " + GameController.score.score;
    }
}
