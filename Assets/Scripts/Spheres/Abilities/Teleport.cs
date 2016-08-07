using UnityEngine;
using System.Collections;
using System;

namespace Abilities {
    public class Teleport : Ability {
        const int BASE_REWARD = 300;
        const float TIME_TO_LIVE = 2;
        const float REWARD_PER_SECOND = BASE_REWARD / TIME_TO_LIVE;
        float ttl = TIME_TO_LIVE;

        public override void FixedUpdate(Rigidbody rigidbody) {
            ttl -= Time.fixedDeltaTime;
            if (ttl < 0) {
                rigidbody.transform.position = GameController.randomPositionInSphere;
                ttl = TIME_TO_LIVE;
            }
        }

        public override Ability Clone() {
            return new Teleport();
        }

        public override int GetValue() {
            return Mathf.RoundToInt(ttl * REWARD_PER_SECOND);
        }
    }
}
