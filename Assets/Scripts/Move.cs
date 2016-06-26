using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Move : MonoBehaviour {
    Rigidbody r;
    Vector3 velocity;
    List<Ability> abilities;
    ushort activeAbilities;

    void Start() {
        StartCoroutine("IsInside");
        r = GetComponent<Rigidbody>();
    }

    public void AddAbility(Ability a) {
        abilities.Add(a);
        activeAbilities++;
    }

    IEnumerator IsInside() {
        while (true) {
            if (Mathf.Abs(transform.position.x) > 8 || Mathf.Abs(transform.position.z) > 8 || transform.position.y < -1) {
                transform.position = new Vector3(0, 6, 0);
                r.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void FixedUpdate() {
        foreach (var ability in abilities) {
            ability.FixedUpdate(r);
        }
    }

    public void Touched() {
        StopCoroutine("IsInside");
        GetComponent<SphereCollider>().enabled = false;
        enabled = false;

        foreach (var ability in abilities) {
            GameController.AddScore(ability.Pop());
            StartCoroutine(ability.PopAnimation(AbilityRemoved));
        }

        GameController.destroyed++;
    }

    void AbilityRemoved() {
        if (--activeAbilities == 0)
            Destroy(gameObject);
    }

    public void ToggleFreeze() {
        if (r.isKinematic)
            r.velocity = velocity;
        else {
            velocity = r.velocity;
            r.velocity = Vector3.zero;
        }

        r.isKinematic = !r.isKinematic;
    }

}
