using Units;
using Items.EquipmentItems;

namespace Items.ItemActions
{
    public class UnequipItemAction : ItemAction
    {
        public override bool Enabled { get { return item.IsEquipped; } }

        private readonly EquipmentItem item;

        public UnequipItemAction(EquipmentItem item) : base("Unequip")
        {
            this.item = item;
        }

        public override void Action(PlayerUnitEntityController actor)
        {
            actor.Unit.Combat.UnequipItem(item);
        }
    }
}
