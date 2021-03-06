﻿using Units;
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

        public override void Action(UnitEntity actor)
        {
            actor.Combat.DelegateCastAbility(item.Ability);
            //actor.Unit.OnUtilityItemUse();
        }
    }
}