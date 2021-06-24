using Units.Combat;

namespace Items.EquipmentItems
{
    public class AllSlotEquipper : SlotEquipper
    {
        private readonly SlotEquipper[] equippers;

        public AllSlotEquipper(SlotEquipper[] equippers)
        {
            this.equippers = equippers;
        }

        public bool IsTrivialEquip(CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
            {
                if (!equipper.IsTrivialEquip(manager))
                    return false;
            }
            return true;
        }

        public void Equip(EquipmentItem equipment, CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
                equipper.Equip(equipment, manager);
        }

        public void Unequip(CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
                equipper.Unequip(manager);
        }
    }
}
