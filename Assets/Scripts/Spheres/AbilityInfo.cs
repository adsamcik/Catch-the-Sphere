using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Abilities {
    [System.Serializable]
    public class AbilityInfo {
        public Ability ability {
            get { if (_ability == null) _ability = GameController.abilityList.FirstOrDefault(x => x.GetType().Name == name); return _ability; }
        }

        [System.NonSerialized]
        Ability _ability;

        public string name;
        public string description;
        public bool enabled;
        public float chanceToSpawn;

        public AbilityInfo(Ability ability, float chanceToSpawn, bool enabled) {
            this._ability = ability;
            this.name = ability.GetType().Name;
            this.chanceToSpawn = chanceToSpawn;
            this.enabled = enabled;
        }
    }

    /// <summary>
    /// Fucking useless unity cant even list
    /// </summary>
    public class AbilityInfoList {
        public AbilityInfo[] array; 

        public AbilityInfoList() {

        }

        public AbilityInfoList(List<AbilityInfo> list) {
            this.array = list.ToArray();
        }
    }
}
