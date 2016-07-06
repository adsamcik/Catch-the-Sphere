using UnityEngine;
using System.Collections;
using System.Linq;

namespace Abilities {
    public class AbilityInfo {
        public string abilityName
        {
            get { return ability.GetType().Name; }
            set { ability = GameController.abilityList.FirstOrDefault(x => x.GetType().Name == value); }
        }

        [System.NonSerialized]
        public Ability ability;
        public bool enabled;
        public float chanceToSpawn;

        public AbilityInfo() {
            ability = null;
            enabled = true;
            chanceToSpawn = 1;
        }

        public AbilityInfo(Ability ability, float chanceToSpawn, bool enabled) {
            this.ability = ability;
            this.chanceToSpawn = chanceToSpawn;
            this.enabled = enabled;
        }

        public string ToJson() {
            return "{name:\"" + abilityName + "\",chanceToSpawn:" + chanceToSpawn + ",enabled:" + enabled.ToString() + "}";
        }
    }
}
