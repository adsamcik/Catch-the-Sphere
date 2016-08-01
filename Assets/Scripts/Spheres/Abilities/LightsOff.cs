using UnityEngine;
using System.Collections;
using System;

namespace Abilities {
    public class LightsOff : Ability {
        static int active = 0;

        Color aColor;
        Color sColor;
        Light light;

        public override void Initialize(SphereStats s) {
            base.Initialize(s);
            aColor = RenderSettings.ambientLight;
            sColor = GameController.sun.color;
            RenderSettings.ambientLight = Color.black;
            GameController.sun.color = Color.black;
            light = gameObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.intensity = 8;
            light.range = 5;
            active++;
           
            s.SetTime(8);
        }

        public override int GetValue() {
            return 20;
        }

        public override IEnumerator FadeOutAnimation() {
            if(--active == 0) {
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
            }
        }

        public override Ability Clone() {
            return new LightsOff();
        }

        public override void OnRemove() {
            RenderSettings.ambientLight = GameController.ambientLight;
            GameController.sun.color = GameController.sunLight;
        }
    }
}
