using UnityEngine;
using System.Collections;

public abstract class Ability {
    public abstract void Initialize(GameObject g);
    public abstract void OnFieldEnter(GameObject g);
    public abstract void OnFieldExit(GameObject g);
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
}
