using UnityEngine;
using System.Collections.Generic;
using Abilities;

public class Stats : MonoBehaviour {
    const int LIFE_MULTIPLIER = 25;
    const int LIFE_LENGTH = 15;

    public float multiplier = 1;
    public float lifeLeft = LIFE_LENGTH;

    public float bonus;

    public List<Ability> abilities = new List<Ability>();
    ushort activeAbilities = 0;

    public void AbilityUpdate(Rigidbody r) {
        foreach (var ability in abilities)
            ability.FixedUpdate(r);
    }

    void Update() {
        //lifeLeft -= Time.deltaTime;
        if (lifeLeft <= 0)
            Destroy(gameObject);
    }

    public void IncreaseLife(float value) {
        lifeLeft += value;
    }

    public int Pop() {
        double value = lifeLeft * LIFE_MULTIPLIER;
        foreach (var ability in abilities) {
            value += ability.Pop();
            StartCoroutine(ability.PopAnimation(AbilityRemoved));
        }
        value *= multiplier;
        return (int)value;
    }

    public void AddAbility(Ability a) {
        Ability ability = a.Clone();
        abilities.Add(ability);
        ability.Initialize(gameObject);
        activeAbilities++;
        name += a.GetType().Name;
    }

    public void AddCustomAbility(Ability a) {
        abilities.Add(a);
        activeAbilities++;
        name += a.GetType().Name;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            foreach (var ability in abilities)
                ability.OnFieldEnter(other);
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 8) {
            foreach (var ability in abilities)
                ability.OnFieldExit(other);
        }
    }

    void AbilityRemoved() {
        if (--activeAbilities == 0)
            Destroy(gameObject);
    }

    public bool hasAbility(Ability a) {
        return abilities.Find(x => x.GetType() == a.GetType()) != null;
    }

    public void RemoveAllAbilities() {
        foreach (var ability in abilities)
            ability.OnRemove();
        abilities.Clear();
        activeAbilities = 0;
        name.Remove(0);
    }
}
