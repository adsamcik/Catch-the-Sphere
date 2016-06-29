using UnityEngine;
using System.Collections;
using System;

public class LightsOff : Ability {
    Color aColor;
    Light light;

    public override void Initialize(GameObject g) {
        base.Initialize(g);
        aColor = RenderSettings.ambientLight;
        RenderSettings.ambientLight = Color.black;
        GameController.sun.enabled = false;
        light = g.AddComponent<Light>();
        light.type = LightType.Point;
        light.intensity = 8;
        light.range = 5;
    }

    public override int Pop() {
        return 500;
    }

    public override IEnumerator PopAnimation(Action func) {
        float val = 0;
        while (RenderSettings.ambientLight != aColor) {
            if ((val += Time.deltaTime) > 1)
                val = 1;
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, aColor, val);
            yield return new WaitForEndOfFrame();
        }
        GameController.sun.enabled = true;
    }

    public override Ability Clone() {
        return new LightsOff();
    }

    public override int GetBonus() {
        return 200;
    }
}
