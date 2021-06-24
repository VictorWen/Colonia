using Items.ItemActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.EquipmentItems
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

        public SlotEquipper SlotEquipper { get; private set; }

        public Dictionary<CombatAttributeID, int> Additives { get; private set; }

        public EquipmentItem(ResourceItem[] materials, float level) : base("Equipment")
        {
            // TODO: Craft equipment
            throw new System.NotImplementedException();
        }

        public EquipmentItem(string name, Dictionary<CombatAttributeID, int> effects, SlotEquipper slotEquipper) : base("Equipment")
        {
            Name = name;
            Tier = 1;
            Hardness = 1;
            Weight = 1;
            Value = 1;
            ID = "test_equipment";
            Additives = effects;

            SlotEquipper = slotEquipper;
        }

        public override List<ItemAction> GetItemActions()
        {
            List<ItemAction> actions = base.GetItemActions();
            if (inventory == null)
                actions.Add(new UnequipItemAction(true, this));
            else
                actions.Add(new EquipItemAction(inventory, this));
            return actions;
        }
    }
}