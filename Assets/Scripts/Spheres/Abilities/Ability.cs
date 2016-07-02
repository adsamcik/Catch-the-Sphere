using UnityEngine;
using System.Collections;

namespace Abilities {
    public abstract class Ability {
        protected GameObject gameObject;
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

        /// <summary>
        /// Bonus awarded while this sphere is active
        /// </summary>
        /// <returns>bonus</returns>
        public virtual int GetBonus() { return 0; }
        public abstract Ability Clone();
        public abstract IEnumerator PopAnimation(System.Action func);

        public virtual void OnRemove() {}
    }
}
