using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuEventController : MonoBehaviour {
	public UnityEngine.UI.Text HighscoreText;

	void Start() {
		HighscoreText.text = "highscore " + PlayerPrefs.GetInt("hs_normal");
	}

	public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
	}

    public void Exit() {
        Application.Quit();
    }
}
