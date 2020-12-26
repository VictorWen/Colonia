using Units;
using Items.UtilityItems;

namespace Items.ItemActions
{
    public class UseItemAction : ItemAction
    {
        public override bool Enabled { get { return item.IsUsable(); } }

        private readonly UtilityItem item;

        public UseItemAction(UtilityItem item) : base ("Use")
        {
            this.item = item;
        }

        public override void Action(UnitEntityScript actor)
        {
            item.Use(actor);
            actor.Unit.OnUtilityItemUse();
        }
    }
}