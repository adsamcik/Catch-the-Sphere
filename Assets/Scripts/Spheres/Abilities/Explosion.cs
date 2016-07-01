using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Abilities {
    public class Explosion : Ability {
        const float EXPLOSION_FORCE = 10;
        const float MAX_DIST = 25;

        List<GameObject> inRange = new List<GameObject>();

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.radius = MAX_DIST;
            sc.isTrigger = true;
            g.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Exploding");
        }

        public override void OnFieldEnter(GameObject g) {
            inRange.Add(g);
        }

        public override void OnFieldExit(GameObject g) {
            inRange.Add(g);
        }

        public override int Pop() {
            int val = 0;
            foreach (var item in inRange) {
                val += Mathf.RoundToInt(MAX_DIST - Vector3.Distance(item.transform.position, gameObject.transform.position));
                item.GetComponent<Rigidbody>().AddExplosionForce(EXPLOSION_FORCE, gameObject.transform.position, MAX_DIST);
            }
            return val;
        }

        public override IEnumerator PopAnimation(System.Action func) {
            yield return new WaitForEndOfFrame();
        }

        public override Ability Clone() {
            return new Explosion();
        }

        public override int GetBonus() {
            return 0;
        }
    }
}
