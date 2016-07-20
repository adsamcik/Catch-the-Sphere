using UnityEngine;
using System.Collections;

namespace Abilities {
    public abstract class Ability {
        const float POP_ANIMATION_LENGTH = 0.1f;

        protected GameObject gameObject;
        protected Transform transform { get { return gameObject.transform; } }

        /// <summary>
        /// Called when Ability is added
        /// </summary>
        /// <param name="g">gameobject</param>
        public virtual void Initialize(GameObject g) {
            gameObject = g;
        }
        public virtual void OnFieldEnter(Collider g) { }
        public virtual void OnFieldExit(Collider g) { }

        /// <summary>
        /// Called when sphere is destroyed
        /// </summary>
        /// <returns>Score for the sphere to award</returns>
        public abstract int Pop();

        /// <summary>
        /// Behaves as FixedUpdate on Monobehavior
        /// </summary>
        /// <param name="rigidbody">Rigidbody</param>
        public virtual void FixedUpdate(Rigidbody rigidbody) { }

        public abstract Ability Clone();

        public virtual IEnumerator PopAnimation(System.Action func) {
            float mod = transform.localScale.x / POP_ANIMATION_LENGTH;
            while (transform.localScale.x > 0) {
                float val = Time.deltaTime * mod;
                transform.localScale -= new Vector3(val, val, val);
                yield return new WaitForEndOfFrame();
            }

            if (func != null)
                func();
        }

        public virtual void OnRemove(MonoBehaviour mb) {
            mb.StartCoroutine(PopAnimation(null));
        }

        protected SphereCollider AddSphereTrigger(float radius) {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = radius;
            return sc;
        }
    }
}
