using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
    
    GameObject sphere;
  
    public GameObject FinalResults;
    public GameObject Highscore;
	public GameController GameController;
    public GameObject ScoreToAdd;

    int combomodifier;

    public int scoretemp { get; private set; }
    public int score { get; private set; }
    float comboscore, combotimer;
    float ctimermax = 2f;

    string highscorekey;

	public bool resultsactive;
    Vector3 OrigPos;

    void Start() {
        CheckLevel();
        OrigPos = transform.position;
    }

	void Update () {
        if (GameController.destroyed == 20 && !resultsactive) {  
            GameController.Results(); 
        }
	}

    public void CheckLevel()
    {
        string level = Application.loadedLevelName;
        if (level == "Normal") { highscorekey = "hs_normal"; }
    }

    bool SetHighscore()
    {
        int highscore = score;
        if (PlayerPrefs.GetInt(highscorekey) <= highscore)
        {
            PlayerPrefs.SetInt(highscorekey, highscore);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public int GetHighscore() {
        return PlayerPrefs.GetInt(highscorekey);
    }

    public void NoScore() {score = 0; GetComponent<TextMesh>().text = "0"; resultsactive = false;}
    public void Summary() {
        StopCoroutine("ComboTimer");
        CountScore(); 
        FinalResults.GetComponent<TextMesh>().text = "\nYour final score is\n" + score + " points.\n\n";
        if (SetHighscore()) { FinalResults.GetComponent<TextMesh>().text += "You are getting better!\nYou have beaten your\nhigh score"; }
        else { FinalResults.GetComponent<TextMesh>().text += "You have " + (PlayerPrefs.GetInt(highscorekey) - score) + " left\n to beat your high score"; } 
    }

    public void AddScoreNoModifier(float spherescore)
    {
        scoretemp += Mathf.RoundToInt(spherescore);
        combotimer = ctimermax;
        ScoreToAdd.GetComponent<TextMesh>().text = "+" + scoretemp; 
    }

    public void AddScore(float spherescore)
    {
        if (combotimer > 0) {
            scoretemp += Mathf.RoundToInt(spherescore * combomodifier);
            combomodifier = (combomodifier + RoundDown(spherescore / 25)) / 2; 
            combotimer = ctimermax; 
            ScoreToAdd.GetComponent<TextMesh>().text = "+" + scoretemp; 
        }
        else {
            scoretemp += Mathf.RoundToInt(spherescore);
            combomodifier = RoundDown(spherescore / 25); 
            combotimer = ctimermax; 
            StartCoroutine("ComboTimer"); 
            ScoreToAdd.GetComponent<TextMesh>().text = "+" + scoretemp; 
        }
    }

    void SetScore(int stbs) {
        score += Mathf.RoundToInt(stbs);
        GetComponent<TextMesh>().text = score.ToString();
        if(stbs != 0) StartCoroutine("ScoreAddedAnim");
    }

    void CountScore() {
        SetScore(scoretemp);
        ScoreToAdd.GetComponent<TextMesh>().text = "";
        scoretemp = 0;
        combomodifier = 0;
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

    IEnumerator ScoreAddedAnim()
    { 
        while (transform.position.y < OrigPos.y + 1)
        {
            transform.position += new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForFixedUpdate();
        }

        while (transform.position.y > OrigPos.y)
        {
            transform.position -= new Vector3(0, Time.deltaTime * 30, 0);
            yield return new WaitForFixedUpdate();
        }

        transform.position = OrigPos;
    }

    

}
