using UnityEngine;
using Units.Loot;

namespace Units
{
    [CreateAssetMenu(menuName = "Unit Entity Data", order = 1)]
    public class UnitEntityData : ScriptableObject
    {
        [Header("General")]
        public bool isPlayerControlled = false;
        public int maxHealth = 100;
        public int movementSpeed = 3;
        public int sight = 4;

        [Header("Mana")]
        public int maxMana = 100;

        [Header("Combat Stats")]
        public int attack = 10;
        public int defence = 5;
        public int piercing = 3;
        public int magic = 10;
        public int resistance = 3;

        public string[] abilities;
        public LootTableSO lootTable;
    }
}
