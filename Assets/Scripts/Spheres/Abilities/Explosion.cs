using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Abilities {
    public class Explosion : Ability {
        const int BONUS_VELOCITY_MULTIPLIER = 5;
        const float EXPLOSION_FORCE = 1000;
        const float MAX_DIST = 25;
        Material m;
        bool active = true;

        public override void Initialize(SphereStats s) {
            base.Initialize(s);
            AddSphereTrigger(MAX_DIST);
            m = new Material(Resources.Load<Material>("Materials/Exploding"));
            controller.SetBaseMaterial(m);
            s.StartCoroutine(Animation());
        }

        public override int GetValue() {
            int val = 0;
            List<SphereStats> inRange = stats.FindSpheresInRange(MAX_DIST);
            foreach (var item in inRange) {
                if (item.gameObject != null) {
                    val += Mathf.RoundToInt(MAX_DIST - Vector3.Distance(item.transform.position, gameObject.transform.position));
                    Rigidbody r = item.GetComponent<Rigidbody>();
                    r.AddExplosionForce(EXPLOSION_FORCE, gameObject.transform.position, MAX_DIST);
                    item.bonusManager.AddBonus(this, new Bonus(stats, BONUS_VELOCITY_MULTIPLIER * (int)r.velocity.sqrMagnitude, true, 5));
                }
            }
            return val;
        }

        IEnumerator Animation() {
            float t = 2;
            while (active) {
                t += Time.deltaTime;
                float r = Mathf.Sin(t * (2 * Mathf.PI)) * 0.5f + 0.25f;
                float g = Mathf.Sin((t + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
                float b = Mathf.Sin((t + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
                float correction = 1 / (r + g + b);
                r *= correction;
                g *= correction;
                b *= correction;
                m.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
                yield return new WaitForEndOfFrame();
            }
        }

        public override IEnumerator Pop() {
            active = false;
            gameObject.GetComponent<Rigidbody>().Sleep();
            Camera.main.GetComponent<CameraEffects>().ShakeCamera(0.25f);

            for (float i = 0; i < 2; i += Time.deltaTime * 2) {
                m.SetFloat("_Displacement", i);
                m.SetVector("_ChannelFactor", new Vector4(i, i, i, 1));
                yield return new WaitForEndOfFrame();
            }
        }

        public override Ability Clone() {
            return new Explosion();
        }

    }
}
