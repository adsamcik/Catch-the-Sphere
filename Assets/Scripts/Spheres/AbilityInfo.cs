using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Abilities {
    [System.Serializable]
    public class AbilityInfo {
        public string abilityName {
            get { return ability.GetType().Name; }
            set { _ability = GameController.abilityList.FirstOrDefault(x => x.GetType().Name == value); name = value; }
        }

        public Ability ability {
            get { if (_ability == null) _ability = GameController.abilityList.FirstOrDefault(x => x.GetType().Name == name); return _ability; }
        }

        [System.NonSerialized]
        Ability _ability;

        public string name;
        public bool enabled;
        public float chanceToSpawn;

        public AbilityInfo(Ability ability, float chanceToSpawn, bool enabled) {
            this._ability = ability;
            this.name = ability.GetType().Name;
            this.chanceToSpawn = chanceToSpawn;
            this.enabled = enabled;
        }

        public string ToJson() {
            return "{name:\"" + abilityName + "\",chanceToSpawn:" + chanceToSpawn + ",enabled:" + enabled.ToString() + "}";
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
