using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Combat;

namespace Units {
    public static class GlobalUnitEntityDictionary
    {
        private readonly static Dictionary<string, UnitEntityCombatData> unitData;

        static GlobalUnitEntityDictionary()
        {
            unitData = new Dictionary<string, UnitEntityCombatData>();
        }

        public static void LoadCombatData(string path)
        {
            foreach (UnitEntityData data in Resources.LoadAll<UnitEntityData>(path + '/'))
            {
                UnitEntityCombatData combatData = UnitEntityCombatData.LoadFromSO(data);
                unitData.Add(data.name, combatData);
            }
        }

        public static UnitEntityData GetUnitEntityData(string path, string id)
        {
            return Resources.Load<UnitEntityData>(path + '/' + id);
        }
        
        public static UnitEntityCombatData GetCombatData(string id)
        {
            return unitData[id];
        }
    }
}