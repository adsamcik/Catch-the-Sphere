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

        static int value = 0;
        static int active;

        List<SphereStats> inRange;

        public Parasite() { }

        public Parasite(GameObject g) {
            base.Initialize(g.GetComponent<SphereStats>());
            Spread();
        }

        public override void Initialize(SphereStats s) {
            base.Initialize(s);
            value += VALUE_MAX + SPHERE_VALUE;
            if (value > VALUE_MAX + SPHERE_VALUE)
                value = VALUE_MAX;
            Spread();
        }

        public void Spread() {
            active++;
            value -= SPHERE_VALUE;
            controller.SetMaterial(Resources.Load<Material>("Materials/Parasite"));
            controller.RemoveTriggerColliders();
            stats.AddTime(999999);
        }

        public override int GetValue() {
            inRange = stats.FindSpheresInRange(SPREAD_RADIUS);
            return --active == 0 && inRange.Count == 0 ? value : 0;
        }

        public override IEnumerator Pop() {
            //Pair<Parasite, Target>
            List<Pair<Transform, Transform>> parasiteSpreads = new List<Pair<Transform, Transform>>();

            foreach (var item in inRange) {
                if (!item.hasAbility(this))
                    parasiteSpreads.Add(new Pair<Transform, Transform>(((GameObject)UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ParasiteSpread"), gameObject.transform.position, new Quaternion())).transform, item.transform));
            }

            while (parasiteSpreads.Count > 0) {
                for (int i = 0; i < parasiteSpreads.Count; i++) {
                    var pair = parasiteSpreads[i];
                    var parasite = pair.first;
                    var target = pair.second;
                    var posDif = target.position - parasite.position;
                    var dir = (posDif).normalized * PARASITE_SPEED * Time.deltaTime;
                    if (dir.sqrMagnitude > posDif.sqrMagnitude) {
                        SphereStats s = target.GetComponent<SphereStats>();
                        s.RemoveAllAbilities();
                        s.AddCustomAbility(new Parasite(target.gameObject));
                        UnityEngine.Object.Destroy(parasite.gameObject);
                        parasiteSpreads.RemoveAt(i);
                        i--;
                    } else
                        parasite.position += dir;

                }
                yield return new WaitForEndOfFrame();
            }
        }

        public override Ability Clone() {
            return new Parasite();
        }

        public override bool CanAdd(Ability a) {
            return false;
        }
    }
}
