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
    //source, target, bonus
    readonly Func<SphereStats, SphereStats, int> function;

    /// <summary>
    /// Bonus costructor
    /// </summary>
    /// <param name="source">Source sphere of the bonus</param>
    /// <param name="value">Bonus value</param>
    /// <param name="shouldDecay">Should decay over time</param>
    /// <param name="timeLeft">Time to live</param>
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

    /// <summary>
    /// Bonus costructor
    /// </summary>
    /// <param name="source">Source sphere of the bonus</param>
    /// <param name="function">Function func(source, target) returns reward</param>
    /// <param name="shouldDecay">Should decay over time</param>
    /// <param name="timeLeft">Time to live</param>
    public Bonus(SphereStats source, Func<SphereStats, SphereStats, int> function, bool shouldDecay = false, float timeLeft = -1) {
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

    public int CountBonus(SphereStats stats) {
        int bonus = function != null ? function(source, stats) : value;
        return shouldDecay ? Mathf.RoundToInt(bonus * (timeLeft / totalTime)) : bonus;
    }
}
