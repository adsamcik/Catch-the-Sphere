using UnityEngine;
using System.Collections;
using System;

public class Bonus {
    float timeLeft;
    int value;
    Func<Rigidbody, int> function;

    public Bonus(float timeLeft, int value) {
        this.timeLeft = timeLeft;
        this.value = value;
    }

    public Bonus(float timeLeft, Func<Rigidbody, int> function) {
        this.timeLeft = timeLeft;
        this.function = function;
    }

    public bool UpdateTime(float timeDiff) {
        timeLeft -= timeDiff;
        return timeLeft > 0;
    }

    public int CountBonus(Rigidbody r) {
        return function != null ? function(r) : value;
    }
}
