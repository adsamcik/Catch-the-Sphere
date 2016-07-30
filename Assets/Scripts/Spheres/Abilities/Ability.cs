using UnityEngine;
using System.Collections;

namespace Abilities {
    public abstract class Ability {
        const float POP_ANIMATION_LENGTH = 0.1f;

        protected ObjectController oc;
        protected GameObject gameObject { get { return oc.gameObject; } }
        protected Transform transform { get { return gameObject.transform; } }
        protected Stats stats;

        /// <summary>
        /// Called when Ability is added
        /// </summary>
        /// <param name="g">gameobject</param>
        public virtual void Initialize(Stats s) {
            this.stats = s;
            this.oc = s.GetComponent<ObjectController>();
        }
        public virtual void OnFieldEnter(Collider g) { }
        public virtual void OnFieldExit(Collider g) { }

        /// <summary>
        /// Called when sphere is destroyed
        /// </summary>
        /// <returns>Score for the sphere to award</returns>
        public abstract int GetValue();

        /// <summary>
        /// Behaves as FixedUpdate on Monobehavior
        /// </summary>
        /// <param name="rigidbody">Rigidbody</param>
        public virtual void FixedUpdate(Rigidbody rigidbody) { }

        public abstract Ability Clone();

        /// <summary>
        /// Pop with callback
        /// </summary>
        /// <param name="func">callback</param>
        public IEnumerator Pop(System.Action func) {
            foreach (var collider in gameObject.GetComponents<Collider>())
                collider.enabled = false;
            yield return Pop();
            func();
        }

        /// <summary>
        /// Called when item is clicked
        /// </summary>
        public virtual IEnumerator Pop() {
            yield return FadeOutAnimation();
        }

        /// <summary>
        /// FadeOut animation with callback
        /// </summary>
        /// <param name="func">Callback</param>
        public IEnumerator FadeOutAnimation(System.Action func) {
            yield return FadeOutAnimation();
            func();
        }

        /// <summary>
        /// FadeOut animation
        /// </summary>
        public virtual IEnumerator FadeOutAnimation() {
            float mod = transform.localScale.x / POP_ANIMATION_LENGTH;
            while (transform.localScale.x > 0) {
                float val = Time.deltaTime * mod;
                transform.localScale -= new Vector3(val, val, val);
                yield return new WaitForEndOfFrame();
            }
        }

        public virtual void OnRemove() { }

        protected SphereCollider AddSphereTrigger(float radius) {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = radius;
            return sc;
        }
    }
}
