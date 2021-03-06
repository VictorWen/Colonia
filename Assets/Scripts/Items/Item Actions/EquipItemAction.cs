﻿using Items.EquipmentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units;

namespace Items.ItemActions
{
    public class EquipItemAction : ItemAction
    {
        public override bool Enabled { get { return !equipment.IsEquipped; } }

        private readonly Inventory inventory;
        private readonly EquipmentItem equipment;

        public EquipItemAction(Inventory inventory, EquipmentItem equipment) : base("Equip")
        {
            this.inventory = inventory;
            this.equipment = equipment;
        }

        public override void Action(UnitEntity actor)
        {
            inventory.RemoveItem(equipment);
            actor.Combat.EquipItem(equipment);
        }
    }
}
