using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Abilities {
    public class GravityField : Ability {
        List<GameObject> colliding = new List<GameObject>();

        public override int Pop() {
            return 200;
        }

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            g.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Gravity");
        }

        public override void OnFieldEnter(Collider c) {
            colliding.Add(c.gameObject);
        }

        public override void OnFieldExit(Collider c) {
            colliding.Remove(c.gameObject);
        }

        public override IEnumerator PopAnimation(Action func) {
            GameController.AddScore(2000);
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 0);
            //StartCoroutine("SpriteField", gameObject.GetComponentInChildren<SpriteRenderer>());

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<Move>().ToggleFreeze();

            yield return new WaitForSeconds(1.5f);

            while (GameController.paused) yield return new WaitForFixedUpdate();

            foreach (GameObject sphere in colliding)
                if (sphere) sphere.GetComponent<Move>().ToggleFreeze();

            func();
        }

        public override Ability Clone() {
            return new GravityField();
        }
    }
}