using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Abilities {
    public class Explosion : Ability {
        const int BONUS_VELOCITY_MULTIPLIER = 5;
        const float EXPLOSION_FORCE = 1000;
        const float MAX_DIST = 25;

        List<GameObject> inRange = new List<GameObject>();

        public override void Initialize(SphereStats s) {
            base.Initialize(s);
            AddSphereTrigger(MAX_DIST);
            s.GetComponent<SphereController>().SetBaseMaterial(Resources.Load<Material>("Materials/Exploding"));
        }

        public override void OnFieldEnter(Collider c) {
            inRange.Add(c.gameObject);
        }

        public override void OnFieldExit(Collider c) {
            inRange.Add(c.gameObject);
        }

        public override int GetValue() {
            int val = 0;
            foreach (var item in inRange) {
                if (item.gameObject != null) {
                    val += Mathf.RoundToInt(MAX_DIST - Vector3.Distance(item.transform.position, gameObject.transform.position));
                    Rigidbody r = item.GetComponent<Rigidbody>();
                    r.AddExplosionForce(EXPLOSION_FORCE, gameObject.transform.position, MAX_DIST);
                    item.GetComponent<SphereStats>().bonusManager.AddBonus(this, new Bonus(stats, BONUS_VELOCITY_MULTIPLIER * (int)r.velocity.sqrMagnitude, true, 5));
                }
            }
            return val;
        }

        public override IEnumerator Pop() {
            gameObject.GetComponent<Rigidbody>().Sleep();
            Camera.main.GetComponent<CameraEffects>().ShakeCamera(0.25f);

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            Material m = new Material(mr.material);
            mr.material = m;

            for (float i = m.GetVector("_ChannelFactor").x; i < 2; i += Time.deltaTime) {
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
