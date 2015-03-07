using UnityEngine;
using System.Collections;

public class PausedScore : MonoBehaviour {
    public TextMesh HighscoreLight;
    public TextMesh HighscoreDark;
    public TextMesh ScoreLight;
    public TextMesh ScoreDark;

    public Score Score;

    void OnEnable() {
        int highscore = Score.GetHighscore();
        int score = Score.score;

        //HighscoreDark.text = "Highscore " + highscore;
        HighscoreLight.text = "Highscore " + highscore;

        ScoreLight.text = "Score " + score;
        //ScoreDark.text = "Score " + score;
    }
}
