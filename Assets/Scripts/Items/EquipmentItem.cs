using Items.ItemActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class EquipmentItem : Item
    {
        public override string Name => throw new System.NotImplementedException();
        public override int Tier => throw new System.NotImplementedException();
        public override float Hardness => throw new System.NotImplementedException(); //Max Durability
        public override float Weight => throw new System.NotImplementedException();
        public override float Value => throw new System.NotImplementedException();
        public override string ID => throw new System.NotImplementedException();
        public override bool IsStackable => throw new System.NotImplementedException();

        public float Durability { get; private set; }

        Dictionary<CombatAttributeID, int> additives;

        public EquipmentItem(ResourceItem[] materials, float level, EquipmentSlotID slot) : base("Equipment")
        {
            // TODO: Craft equipment
            throw new System.NotImplementedException();
        }

        public EquipmentItem() : base("Equipment")
        {
            // TODO: Spawn loot equipment
            throw new System.NotImplementedException();
        }

        public EquipmentItem(Dictionary<CombatAttributeID, int> effects) : base("Equipment")
        {
            additives = effects;
        }

        public override List<ItemAction> GetItemActions()
        {
            List<ItemAction> actions = base.GetItemActions();
            actions.Add(new EquipItemAction(inventory, this));
            return actions;
        }
    }
}