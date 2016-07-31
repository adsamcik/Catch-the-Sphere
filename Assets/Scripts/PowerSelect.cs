using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerSelect : MonoBehaviour {
    public Slider slider;
    public Text powerValueText;
    AsyncOperation ao;

    int value;
    void Start() {
        ao = SceneManager.LoadSceneAsync("Normal", LoadSceneMode.Single);
        ao.allowSceneActivation = false;
        slider.onValueChanged.AddListener(SliderUpdate);
        slider.maxValue = PlayerStats.power;
    }

    public void SliderUpdate(float value) {
        this.value = Mathf.RoundToInt(value);
        powerValueText.text = this.value.ToString();
    }

    public void Continue() {
        PlayerStats.SetAvailablePower(value);
        ao.allowSceneActivation = true;

    }
}
