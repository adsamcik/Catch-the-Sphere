using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public void ReturnToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
