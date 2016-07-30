﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Gravity : Ability {
        const int BONUS = (int)(FORCE / MAX_DIST * 100);
        const float MAX_DIST = 3;
        const float FORCE = 3;
        const string GRAVITY_EFFECT_NAME = "gravity";

        List<Rigidbody> inRange = new List<Rigidbody>();

        public override int GetValue() {
            return 200;
        }

        public override void Initialize(Stats s) {
            base.Initialize(s);
            Transform dt = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GravityDistortion")).transform;
            dt.parent = gameObject.transform;
            dt.localPosition = dt.position;
            dt.localScale = new Vector3(2 * MAX_DIST, 2 * MAX_DIST, 2 * MAX_DIST);
            dt.name = GRAVITY_EFFECT_NAME;

            AddSphereTrigger(MAX_DIST);
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
            c.GetComponent<Stats>().AddBonus(BONUS);
        }

        public override void OnFieldExit(Collider c) {
            inRange.Remove(c.GetComponent<Rigidbody>());
            c.GetComponent<Stats>().RemoveBonus(BONUS);
        }

        public override IEnumerator Pop() {
            Transform t = gameObject.transform.Find(GRAVITY_EFFECT_NAME);
            float val = 0;
            while (val < 1) {
                t.localScale = Vector3.Lerp(t.localScale, Vector3.zero, val);
                val += Time.deltaTime * 4;
                yield return new WaitForEndOfFrame();
            }
            t.transform.localScale = Vector3.zero;

            val = 0;
            while (val < 1) {
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, val);
                val += Time.deltaTime * 2;
                yield return new WaitForEndOfFrame();
            }
            gameObject.transform.localScale = Vector3.zero;
        }

        public override Ability Clone() {
            return new Gravity();
        }
    }
}