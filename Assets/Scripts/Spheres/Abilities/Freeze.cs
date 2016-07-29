using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Freeze : Ability {
        const float FREEZE_RANGE = 5;
        const float FREEZE_TIME = 1.5f;

        List<GameObject> colliding = new List<GameObject>();

        public override int GetValue() {
            return 200;
        }

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            ObjectController controller = g.GetComponent<ObjectController>();
            controller.AddMaterial(Resources.Load<Material>("Materials/Ice"));
            controller.SetModel(Resources.Load<Mesh>("Models/MedIce"));
            AddSphereTrigger(FREEZE_RANGE);
        }

        public override void OnFieldEnter(Collider c) {
            colliding.Add(c.gameObject);
        }

        public override void OnFieldExit(Collider c) {
            colliding.Remove(c.gameObject);
        }

        public override IEnumerator Pop() {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<ObjectController>().ToggleFreeze();

            yield return new WaitForSeconds(FREEZE_TIME);

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<ObjectController>().ToggleFreeze();
        }

        public override Ability Clone() {
            return new Freeze();
        }
    }
}