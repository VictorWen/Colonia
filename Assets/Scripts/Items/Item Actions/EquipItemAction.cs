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
        public override bool Enabled { get { return true; } }

        private readonly Inventory inventory;
        private readonly EquipmentItem equipment;

        public EquipItemAction(Inventory inventory, EquipmentItem equipment) : base("Equip")
        {
            this.inventory = inventory;
            this.equipment = equipment;
        }

        public override void Action(PlayerUnitEntityController actor)
        {
            inventory.RemoveItem(equipment);
            actor.Unit.Combat.EquipItem(equipment);
        }
    }
}
