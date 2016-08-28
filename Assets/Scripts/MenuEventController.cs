using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuEventController : MonoBehaviour {
    public UnityEngine.UI.Text HighscoreText;

    void Start() {
        HighscoreText.text = PlayerStats.power + "\npower";
        PlayerStats.AddPower(500);
    }

    public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
    }

    public void Journal() {
        LoadLevel("Journal");
    }

    public void Stats() {

    }

    public void Exit() {
        Application.Quit();
    }
}
