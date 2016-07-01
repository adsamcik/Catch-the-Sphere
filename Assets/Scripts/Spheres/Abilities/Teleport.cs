using UnityEngine;
using System.Collections;
using System;

public class Teleport : Ability {
    const float TIME_TO_LIVE = 2;
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

    public override int Pop() {
        return Mathf.RoundToInt((ttl / TIME_TO_LIVE) * 200);
    }

    public override IEnumerator PopAnimation(Action func) {
        yield return new WaitForEndOfFrame();
    }
}
