using UnityEngine;
using System.Collections;
using System;

public class Bonus {
    readonly SphereStats source;
    readonly float totalTime;
    float timeLeft;
    readonly bool useTime;

    readonly bool shouldDecay;
    readonly int value;
    readonly Func<SphereStats, Rigidbody, int> function;

    public Bonus(SphereStats source, int value, bool shouldDecay = false, float timeLeft = -1) {
        this.source = source;
        this.useTime = timeLeft > 0;
        this.timeLeft = timeLeft;
        this.totalTime = timeLeft;
        this.value = value;

        if (!useTime && shouldDecay)
            throw new Exception("Bonus must have limited lifetime when decay is enabled");
        this.shouldDecay = shouldDecay;
    }

    public Bonus(SphereStats source, Func<SphereStats, Rigidbody, int> function, bool shouldDecay = false, float timeLeft = -1) {
        this.source = source;
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
        int bonus = function != null ? function(source, r) : value;
        return shouldDecay ? Mathf.RoundToInt(bonus * (timeLeft / totalTime)) : bonus;
    }
}
