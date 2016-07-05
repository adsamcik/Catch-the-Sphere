using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Parasite : Ability {
        const float PARASITE_SPEED = 20;
        const int VALUE_MAX = 10000;
        const int SPHERE_VALUE = 1000;
        const float SPREAD_RADIUS = 4;

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
            sc.radius = SPREAD_RADIUS;
            sc.isTrigger = true;
            gameObject.GetComponent<Stats>().IncreaseLife(999999);
        }

        public override int Pop() {
            active--;
            foreach (var item in inRange) {
                if (item != null && !item.GetComponent<Stats>().hasAbility(this))
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

            //Pair<Parasite, Target>
            List<Pair<Transform, Transform>> parasiteSpreads = new List<Pair<Transform, Transform>>();

            foreach (var item in inRange)
                if (item != null && !item.GetComponent<Stats>().hasAbility(this))
                    parasiteSpreads.Add(new Pair<Transform, Transform>(((GameObject)UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ParasiteSpread"), gameObject.transform.position, new Quaternion())).transform, item.transform));

            while (parasiteSpreads.Count > 0) {
                for (int i = 0; i < parasiteSpreads.Count; i++) {
                    var pair = parasiteSpreads[i];
                    var parasite = pair.first;
                    var target = pair.second;
                    var posDif = target.position - parasite.position;
                    var dir = (posDif).normalized * PARASITE_SPEED * Time.deltaTime;
                    if (dir.sqrMagnitude > posDif.sqrMagnitude) {
                        Stats s = target.GetComponent<Stats>();
                        s.RemoveAllAbilities();
                        s.AddCustomAbility(new Parasite(target.gameObject));
                        UnityEngine.Object.Destroy(parasite.gameObject);
                        parasiteSpreads.RemoveAt(i);
                        i--;
                    }
                    else
                        parasite.position += dir;

                }
                yield return new WaitForEndOfFrame();
            }
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
