using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Parasite : Ability {
        const float PARASITE_SPEED = 20;
        const int BASE_VALUE = 50;
        const int SPHERE_VALUE = 100;
        const float SPREAD_RADIUS = 4;

        static int active;

        List<SphereStats> inRange;


        public override void Initialize(SphereStats s) {
            base.Initialize(s);
            active++;
            stats.RemoveAllAbilitiesExcept(this);
            controller.SetMaterial(Resources.Load<Material>("Materials/Parasite"));
            controller.RemoveTriggerColliders();
            stats.AddTime(999999);
            GlobalManager.bonusManager.AddBonus(this, new Bonus(stats, -25));
        }

        public override int GetValue() {
            GlobalManager.bonusManager.RemoveBonus(this);
            inRange = stats.FindSpheresInRange(SPREAD_RADIUS);
            return --active == 0 && inRange.Count == 0 ? BASE_VALUE + (SPHERE_VALUE * GameController.activeSpheres.Count) : 0;
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
                    var dir = posDif.normalized * PARASITE_SPEED * Time.deltaTime;
                    if (dir.sqrMagnitude > posDif.sqrMagnitude) {
                        SphereStats s = target.GetComponent<SphereStats>();
                        s.AddAbility(this);
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
