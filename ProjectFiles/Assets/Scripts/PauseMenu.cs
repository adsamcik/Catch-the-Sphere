using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
    public void ReturnToMenu() {
        Time.timeScale = 1;
        Application.LoadLevel(0);
    }
}
