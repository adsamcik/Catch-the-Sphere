using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusManager {
    Dictionary<uint, Bonus> dict = new Dictionary<uint, Bonus>();
    uint nextID = 0;

    public uint AddBonus(Bonus b) {
        dict.Add(nextID, b);
        return nextID++;
    }

    public void RemoveBonus(uint id) {
        dict.Remove(id);
    }

    public int CalculateBonus(Rigidbody r) {
        int bonus = 0;
        foreach (var item in dict) 
            bonus += item.Value.CountBonus(r);
        return bonus;
    }
}
