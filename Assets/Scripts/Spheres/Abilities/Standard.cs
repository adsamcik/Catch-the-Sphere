using UnityEngine;
using System.Collections;

namespace Abilities {
    public class Standard : Ability {
        const int MAX_SPEED = 50;
        const int MIN_SPEED = 10;
        const int BASE_VALUE = 14;

        public override void Initialize(SphereStats s) {
            base.Initialize(s);
        }

        public override int GetValue() {
            return Mathf.RoundToInt(Mathf.Log10(controller.GetSqrVelocity()) * BASE_VALUE);
        }

        public override Ability Clone() {
            return new Standard();
        }
    }
}
