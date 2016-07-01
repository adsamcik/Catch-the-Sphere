using UnityEngine;
using System.Collections.Generic;
using Abilities;

public class Stats : MonoBehaviour {
    const int LIFE_MULTIPLIER = 25;
    const int LIFE_LENGTH = 10;

    public float multiplier = 1;
    public float lifeLeft = LIFE_LENGTH;

    public List<Ability> abilities = new List<Ability>();
    ushort activeAbilities = 0;

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
        Debug.Log("Added ability " + a.GetType().Name);
        activeAbilities++;
        name += a.GetType().Name;
    }


    void AbilityRemoved() {
        if (--activeAbilities == 0)
            Destroy(gameObject);
    }
}
