using UnityEngine;
using System.Collections;
using System;

namespace Abilities {
    public class LightsOff : Ability {
        Color aColor;
        Color sColor;
        Light light;

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            aColor = RenderSettings.ambientLight;
            sColor = GameController.sun.color;
            RenderSettings.ambientLight = Color.black;
            GameController.sun.color = Color.black;
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
                GameController.sun.color = Color.Lerp(GameController.sun.color, sColor, val);
                light.color = Color.Lerp(light.color, Color.black, val);
                yield return new WaitForEndOfFrame();
            }
            GameController.sun.enabled = true;
            func();
        }

        public override Ability Clone() {
            return new LightsOff();
        }

        public override void OnRemove(MonoBehaviour mb) {
            RenderSettings.ambientLight = GameController.ambientLight;
            GameController.sun.color = GameController.sunLight;
            mb.StartCoroutine(base.PopAnimation(null));
        }
    }
}
