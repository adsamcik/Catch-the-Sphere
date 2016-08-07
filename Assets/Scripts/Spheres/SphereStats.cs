using UnityEngine;
using System.Collections.Generic;
using Abilities;
using System;
using System.Collections;

public class SphereStats : MonoBehaviour {
    const int BASE_LIFE_MULTIPLIER = 25;
    const int BASE_TIME_TO_LIVE = 15;

    float multiplier = 1;
    float timeMultiplier = BASE_LIFE_MULTIPLIER;
    float timeLeft = BASE_TIME_TO_LIVE;
    float totalTime = BASE_TIME_TO_LIVE;

    public List<Ability> abilities = new List<Ability>();
    ushort activeAbilities = 0;

    public BonusManager bonusManager = new BonusManager();

    public void AbilityUpdate(Rigidbody r) {
        foreach (var ability in abilities)
            ability.FixedUpdate(r);
    }

    void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            GetComponent<SphereController>().enabled = false;
            enabled = false;
            foreach (var ability in abilities)
                StartCoroutine(ability.FadeOutAnimation(AbilityRemoved));
        }
    }

    public void AddTime(float value) {
        timeLeft += value;
        totalTime += value;
    }

    public void SetTime(float value) {
        totalTime = (totalTime - timeLeft) + value;
        timeLeft = value;
    }

    public List<SphereStats> FindSpheresInRange(float range) {
        List<SphereStats> inRange = new List<SphereStats>();
        foreach (var sphere in GameController.activeSpheres) {
            if ((sphere.transform.position - transform.position).magnitude <= range)
                inRange.Add(sphere);
        }
        return inRange;
    }

    public int Pop() {
        GetComponent<SphereController>().RemoveColliders();
        double value = (timeLeft / totalTime) * timeMultiplier + bonusManager.CalculateBonus(this);
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
