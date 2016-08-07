using UnityEngine;
using System.Collections;
using System;

namespace Abilities {
    public class LightsOff : Ability {
        static int active = 0;
        Light light;

        public override void Initialize(SphereStats s) {
            base.Initialize(s);
            GlobalManager.SetAmbientLight(Color.black);
            GlobalManager.SetSunLight(Color.black);
            GlobalManager.SetReflectionIntensity(0);
            GlobalManager.bonusManager.AddBonus(this, new Bonus(stats, 75));
            light = gameObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.intensity = 8;
            light.range = 5;
            active++;
           
            s.SetTime(8);
        }

        public override int GetValue() {
            return 25;
        }

        public override IEnumerator FadeOutAnimation() {
            GlobalManager.ReleaseAmbientLight(Color.black);
            GlobalManager.ReleaseSunLight(Color.black);
            GlobalManager.bonusManager.RemoveBonus(this);
            yield return new WaitForEndOfFrame();
        }

        public override Ability Clone() {
            return new LightsOff();
        }

        public override void OnRemove() {
            /*RenderSettings.ambientLight = GameController.ambientLight;
            GameController.sun.color = GameController.sunLight;*/
        }
    }
}
