using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Parasite : Ability {
        const int VALUE_MAX = 10000;
        const int SPHERE_VALUE = 1000;
        static int value;
        static int active;

        List<GameObject> inRange = new List<GameObject>();

        public Parasite() { }

        public Parasite(GameObject g) {
            base.Initialize(g);
            Spread();
        }

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            if (active++ > 0) {
                value += VALUE_MAX;
                if (value > VALUE_MAX)
                    value = VALUE_MAX;
            }
            else
                value = VALUE_MAX;
            Spread();
        }

        public void Spread() {
            gameObject.GetComponent<Renderer>().materials = new Material[] { Resources.Load<Material>("Materials/Parasite") };
            foreach (var item in gameObject.GetComponents<Collider>()) {
                if (item.isTrigger)
                    UnityEngine.Object.Destroy(item);
            }
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.radius = 10;
            sc.isTrigger = true;
        }

        public override int Pop() {
            active--;
            foreach (var item in inRange) {
                Stats s = item.GetComponent<Stats>();
                s.abilities.Clear();
                s.abilities.Add(new Parasite(item));
                value -= SPHERE_VALUE;
            }

            return active == 0 ? value : active;
        }

        public override IEnumerator PopAnimation(Action func) {
            SphereCollider[] sc = gameObject.GetComponents<SphereCollider>();
            SphereCollider main = null;
            foreach (var item in sc) {
                if (item.isTrigger)
                    UnityEngine.Object.Destroy(item);
                else if (main == null)
                    main = item;
                else
                    Debug.LogWarning("Sphere has more than one non-trigger collider");
            }
            yield return new WaitForEndOfFrame();
            func();
        }

        public override void OnFieldEnter(Collider g) {
            inRange.Add(g.gameObject);
        }

        public override void OnFieldExit(Collider g) {
            inRange.Remove(g.gameObject);
        }

        public override Ability Clone() {
            return new Parasite();
        }
    }
}
