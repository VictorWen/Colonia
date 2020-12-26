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

        public float Durability { get; private set; }

        // Combat Attributes
        public float Attack { get; private set; }
        public float Piercing { get; private set; }
        public float MagicAttack { get; private set; }
        public float Armor { get; private set; }
        public float Defence { get; private set; }
        public float MagicArmor { get; private set; }

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
    }
}