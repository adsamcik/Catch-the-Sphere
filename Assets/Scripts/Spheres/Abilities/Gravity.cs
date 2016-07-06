using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Gravity : Ability {
        const float MAX_DIST = 3;
        const float FORCE = 3;

        List<Rigidbody> inRange = new List<Rigidbody>();

        public override int Pop() {
            return 200;
        }

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            Transform dt = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GravityDistortion")).transform;
            dt.parent = gameObject.transform;
            dt.localPosition = dt.position;
            dt.localScale = new Vector3(2 * MAX_DIST, 2 * MAX_DIST, 2 * MAX_DIST);

            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = MAX_DIST;
        }

        public override void FixedUpdate(Rigidbody rigidbody) {
            for (int i = 0; i < inRange.Count; i++) {
                var rb = inRange[i];
                if (rb == null)
                    inRange.RemoveAt(i--);
                else {
                    float force = Vector3.Distance(gameObject.transform.position, rb.transform.position) / MAX_DIST * FORCE;
                    Vector3 forceVector = (gameObject.transform.position - rb.transform.position) * force;
                    rb.AddForce(forceVector, ForceMode.Force);
                }
            }
        }

        public override void OnFieldEnter(Collider c) {
            inRange.Add(c.GetComponent<Rigidbody>());
        }

        public override void OnFieldExit(Collider c) {
            inRange.Remove(c.GetComponent<Rigidbody>());
        }

        public override IEnumerator PopAnimation(Action func) {
            

            yield return new WaitForEndOfFrame();
            func();
        }

        public override Ability Clone() {
            return new Gravity();
        }
    }
}