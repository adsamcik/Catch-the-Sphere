using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class Freeze : Ability {
        const float FREEZE_RANGE = 3;
        const float FREEZE_TIME = 1.5f;

        List<GameObject> colliding = new List<GameObject>();

        Material material;

        SphereCollider sc;

        public override int GetValue() {
            return 200;
        }

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            ObjectController controller = g.GetComponent<ObjectController>();
            material = Resources.Load<Material>("Materials/Ice");
            controller.AddMaterial(material);
            controller.SetModel(Resources.Load<Mesh>("Models/MedIce"));
            sc = AddSphereTrigger(FREEZE_RANGE);
        }

        public override void OnFieldEnter(Collider c) {
            colliding.Add(c.gameObject);
        }

        public override void OnFieldExit(Collider c) {
            colliding.Remove(c.gameObject);
        }

        public override IEnumerator Pop() {
            gameObject.GetComponent<ObjectController>().SetMaterial(material);
            gameObject.transform.localScale = new Vector3(FREEZE_RANGE * 2, FREEZE_RANGE * 2, FREEZE_RANGE * 2);
            sc.enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<ObjectController>().SetFreeze(true);

            yield return new WaitForSeconds(FREEZE_TIME);

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<ObjectController>().SetFreeze(false);
        }

        public override Ability Clone() {
            return new Freeze();
        }
    }
}