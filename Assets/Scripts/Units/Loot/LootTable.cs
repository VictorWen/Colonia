using Items;
using System.Collections.Generic;
using Tiles;

namespace Units.Loot
{
    public class LootTable
    {
        private List<float> chances;
        private List<Item> drops;

        public LootTable(List<float> chances, List<Item> drops)
        {
            if (chances.Count != drops.Count)
                throw new System.Exception("chances should be the same size as drops");
            this.chances = chances;
            this.drops = drops;
        }

        public void DropLoot(IWorld world, UnitEntity receiver)
        {
            for (int i = 0; i < chances.Count; i++)
            {
                float roll = (float)world.RNG.NextDouble();
                if (roll < chances[i])
                {
                    receiver.Inventory.AddItem(drops[i]);
                }
            }
        }
    }
}
