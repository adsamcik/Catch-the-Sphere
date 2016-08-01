using UnityEngine;
using System.Collections;
using System;

public class Bonus {
    readonly float totalTime;
    float timeLeft;
    readonly bool useTime;

    readonly bool shouldDecay;
    readonly int value;
    readonly Func<Rigidbody, int> function;

    public Bonus(int value, bool shouldDecay = false, float timeLeft = -1) {
        this.useTime = timeLeft > 0;
        this.timeLeft = timeLeft;
        this.totalTime = timeLeft;
        this.value = value;

        if (!useTime && shouldDecay)
            throw new Exception("Bonus must have limited lifetime when decay is enabled");
        this.shouldDecay = shouldDecay;
    }

    public Bonus(Func<Rigidbody, int> function, bool shouldDecay = false, float timeLeft = -1) {
        this.useTime = timeLeft > 0;
        this.timeLeft = timeLeft;
        this.totalTime = timeLeft;
        this.function = function;

        if (!useTime && shouldDecay)
            throw new Exception("Bonus must have limited lifetime when decay is enabled");
        this.shouldDecay = shouldDecay;
    }

    public bool UpdateTime(float timeDiff) {
        if (!useTime)
            return true;
        timeLeft -= timeDiff;
        return timeLeft > 0;
    }

    public int CountBonus(Rigidbody r) {
        int bonus = function != null ? function(r) : value;
        return shouldDecay ? Mathf.RoundToInt(bonus * (1 - (timeLeft / totalTime))) : bonus;
    }
}
