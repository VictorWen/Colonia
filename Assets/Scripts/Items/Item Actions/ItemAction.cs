using Units;

namespace Items.ItemActions
{
    public abstract class ItemAction
    {
        public string Name { get; private set; }

        public abstract bool Enabled { get; }

        public ItemAction(string name)
        {
            Name = name;
        }

        public abstract void Action(UnitEntity actor);
    }
}