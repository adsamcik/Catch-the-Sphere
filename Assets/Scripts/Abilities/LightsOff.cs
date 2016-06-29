using UnityEngine;
using System.Collections;
using System;

public class LightsOff : Ability {
    Color aColor;
    Light light;

    public void FixedUpdate(Rigidbody rigidbody) {

    }

    public void Initialize(GameObject g) {
        aColor = RenderSettings.ambientLight;
        RenderSettings.ambientLight = Color.black;
        GameController.sun.enabled = false;
        light = g.AddComponent<Light>();
        light.type = LightType.Point;
        light.intensity = 8;
        light.range = 5;
    }

    public void OnFieldEnter(GameObject g) {

    }

    public void OnFieldExit(GameObject g) {

    }

    public int Pop() {
        return 500;
    }

    public IEnumerator PopAnimation(Action func) {
        float val = 0;
        while (RenderSettings.ambientLight != aColor) {
            if ((val += Time.deltaTime) > 1)
                val = 1;
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, aColor, val);
            yield return new WaitForEndOfFrame();
        }
        GameController.sun.enabled = true;
    }

    public Ability Clone() {
        return new LightsOff();
    }

    public int GetBonus() {
        return 0;
    }
}
