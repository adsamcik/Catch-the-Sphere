using UnityEngine;
using System.Collections;

public class AbilityInfo {
    public Ability ability;
    public bool enabled;
    public float chanceToSpawn;

    public AbilityInfo(Ability ability, float chanceToSpawn, bool enabled) {
        this.ability = ability;
        this.chanceToSpawn = chanceToSpawn;
        this.enabled = enabled;
    }
}
