using UnityEngine;
using System.Collections.Generic;
using Abilities;
using System;
using System.Collections;

public class Stats : MonoBehaviour {
    const int BASE_LIFE_MULTIPLIER = 25;
    const int BASE_TIME_TO_LIVE = 15;

    float multiplier = 1;
    float timeMultiplier = BASE_LIFE_MULTIPLIER;
    float timeLeft = BASE_TIME_TO_LIVE;
    float totalTime = BASE_TIME_TO_LIVE;

    public List<Ability> abilities = new List<Ability>();
    ushort activeAbilities = 0;

    int bonus = 0;
    List<Func<int>> bonusFunctions;

    public void AbilityUpdate(Rigidbody r) {
        foreach (var ability in abilities)
            ability.FixedUpdate(r);
    }

    void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            GetComponent<Stats>().enabled = false;
            enabled = false;
            foreach (var ability in abilities)
                StartCoroutine(ability.FadeOutAnimation(AbilityRemoved));
        }
    }

    public void AddTime(float value) {
        timeLeft += value;
        totalTime += value;
    }

    public void AddBonus(int value) {
        bonus += value;
    }

    public void AddBonus(Func<int> f, float expire = -1) {
        bonusFunctions.Add(f);
        if (expire > 0)
            StartCoroutine(BonusTimer(f, expire));
    }

    public void RemoveBonus(int value) {
        bonus -= value;
    }

    public void RemoveBonus(Func<int> f) {
        bonusFunctions.Remove(f);
    }

    IEnumerator BonusTimer(Func<int> f, float expire) {
        yield return new WaitForSeconds(expire);
        RemoveBonus(f);
    }

    public int Pop() {
        double value = (timeLeft / totalTime) * timeMultiplier + bonus;
        foreach (var ability in abilities) {
            value += ability.GetValue();
            StartCoroutine(ability.Pop(AbilityRemoved));
        }

        value *= multiplier;
        return (int)value;
    }

    public void AddAbility(Ability a) {
        foreach (var item in abilities)
            if (!item.CanAdd(a))
                return;
        Ability ability = a.Clone();
        abilities.Add(ability);
        ability.Initialize(this);
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
