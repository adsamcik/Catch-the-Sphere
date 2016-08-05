using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Abilities;

public class BonusManager {
    List<KeyValuePair<Ability, Bonus>> dict = new List<KeyValuePair<Ability, Bonus>>();

    public void AddBonus(Ability a, Bonus b) {
        dict.Add(new KeyValuePair<Ability, Bonus>(a, b));
    }

    public void RemoveBonus(Ability a) {
        for (int i = 0; i < dict.Count; i++) {
            if (dict[i].Key == a) {
                dict.RemoveAt(i);
                break;
            }
        }
    }

    public int CalculateBonus(Rigidbody r) {
        int bonus = 0;
        foreach (var item in dict)
            bonus += item.Value.CountBonus(r);
        return bonus;
    }
}
