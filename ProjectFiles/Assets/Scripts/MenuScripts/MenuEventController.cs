using UnityEngine;
using System.Collections;

public class MenuEventController : MonoBehaviour {
	public UnityEngine.UI.Text HighscoreText;

	void Start() {
		HighscoreText.text = "highscore " + PlayerPrefs.GetInt("hs_normal");
	}

	public void LoadLevel(string name) {
		Application.LoadLevel(name);
	}
}
