using UnityEngine;
using System.Collections;

namespace Abilities {
    public class Standard : Ability {
        const int MAX_SPEED = 50;
        const int MIN_SPEED = 10;

        float sqrspeed;

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            float speed = Random.Range(MIN_SPEED, MAX_SPEED);
            sqrspeed = speed * speed;
        }

        public override int Pop() {
            return Mathf.RoundToInt(sqrspeed);
        }

        public override Ability Clone() {
            return new Standard();
        }
    }
}
