using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
    public void ReturnToMenu() {
        Application.LoadLevel(0);
    }
}
