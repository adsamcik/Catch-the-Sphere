using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PausedScore : MonoBehaviour {
    public Text HighscoreText;
    public Text ScoreText;

    public GameStats Score;

    void Awake() {
        HighscoreText.text = "Total power " + PlayerStats.power;
        ScoreText.text = "Game power " + GameController.score.power;
    }
}
