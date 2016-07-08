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

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            AddSphereTrigger(MAX_DIST);
            g.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Exploding");
        }

        public override void OnFieldEnter(Collider c) {
            inRange.Add(c.gameObject);
        }

        public override void OnFieldExit(Collider c) {
            inRange.Add(c.gameObject);
        }

        public override int Pop() {
            int val = 0;
            foreach (var item in inRange) {
                if (item.gameObject != null) {
                    val += Mathf.RoundToInt(MAX_DIST - Vector3.Distance(item.transform.position, gameObject.transform.position));
                    Rigidbody r = item.GetComponent<Rigidbody>();
                    r.AddExplosionForce(EXPLOSION_FORCE, gameObject.transform.position, MAX_DIST);
                    item.GetComponent<Stats>().AddBonus(BONUS_VELOCITY_MULTIPLIER * r.velocity.sqrMagnitude);
                }
            }
            return val;
        }

        public override IEnumerator PopAnimation(Action func) {
            gameObject.GetComponent<Collider>().enabled = false;
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

            func();
        }

        public override Ability Clone() {
            return new Explosion();
        }

        public override int GetBonus() {
            return 0;
        }
    }
}
