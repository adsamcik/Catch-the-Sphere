using UnityEngine;
using System.Collections;

namespace Abilities {
    public class Standard : Ability {
        const int MAX_SPEED = 50;
        const int MIN_SPEED = 10;
        const float POP_ANIMATION_LENGTH = 0.5f;

        Rigidbody rigidbody;
        Transform transform;
        float sqrspeed;

        public override void Initialize(GameObject g) {
            base.Initialize(g);
            rigidbody = g.GetComponent<Rigidbody>();
            transform = g.transform;
            float speed = Random.Range(MIN_SPEED, MAX_SPEED);
            sqrspeed = speed * speed;
        }

        public override int Pop() {
            return Mathf.RoundToInt(sqrspeed);
        }

        public override IEnumerator PopAnimation(System.Action func) {
            float mod = transform.localScale.x / POP_ANIMATION_LENGTH;
            while (transform.localScale.x > 0) {
                float val = Time.deltaTime * mod;
                transform.localScale -= new Vector3(val, val, val);
                yield return new WaitForEndOfFrame();
            }
            func();
        }

        public override Ability Clone() {
            return new Standard();
        }
    }
}
