using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Freeze : Ability {
        const float FREEZE_RANGE = 5;
        const float FREEZE_TIME = 1.5f;

        List<GameObject> colliding = new List<GameObject>();

        public override int Pop() {
            return 200;
        }

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            g.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Ice");
            AddSphereTrigger(FREEZE_RANGE);
        }

        public override void OnFieldEnter(Collider c) {
            colliding.Add(c.gameObject);
        }

        public override void OnFieldExit(Collider c) {
            colliding.Remove(c.gameObject);
        }

        public override IEnumerator PopAnimation(Action func) {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<Move>().ToggleFreeze();

            yield return new WaitForSeconds(FREEZE_TIME);

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<Move>().ToggleFreeze();

            func();
        }

        public override Ability Clone() {
            return new Freeze();
        }
    }
}