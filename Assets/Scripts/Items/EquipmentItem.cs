using Items.ItemActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class EquipmentItem : Item
    {
        public override string Name { get; }
        public override int Tier { get; }
        public override float Hardness { get; } //Max Durability
        public override float Weight { get; }
        public override float Value { get; }
        public override string ID { get; }
        public override bool IsStackable { get { return false; } }

        public float Durability { get; private set; }

        public Dictionary<CombatAttributeID, int> Additives { get; private set; }

        public EquipmentItem(ResourceItem[] materials, float level, EquipmentSlotID slot) : base("Equipment")
        {
            // TODO: Craft equipment
            throw new System.NotImplementedException();
        }

        public EquipmentItem(Dictionary<CombatAttributeID, int> effects) : base("Equipment")
        {
            Name = "TEST EQUIPMENT PLACEHOLDER NAME";
            Tier = 1;
            Hardness = 1;
            Weight = 1;
            Value = 1;
            ID = "test_equipment";
            Additives = effects;
        }

        public override List<ItemAction> GetItemActions()
        {
            List<ItemAction> actions = base.GetItemActions();
            actions.Add(new EquipItemAction(inventory, this));
            return actions;
        }
    }
}