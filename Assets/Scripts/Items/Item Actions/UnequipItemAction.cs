using Units;
using Items.EquipmentItems;

namespace Items.ItemActions
{
    public class UnequipItemAction : ItemAction
    {
        public override bool Enabled { get; }

        private readonly EquipmentItem item;

        public UnequipItemAction(bool enabled, EquipmentItem item) : base("Unequip")
        {
            Enabled = enabled;
            this.item = item;
        }

        public override void Action(PlayerUnitEntityController actor)
        {
            actor.Unit.Combat.UnequipItem(item);
        }
    }
}
