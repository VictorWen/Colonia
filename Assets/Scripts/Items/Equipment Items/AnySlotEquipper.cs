using Units.Combat;

namespace Items.EquipmentItems
{
    public class AnySlotEquipper : SlotEquipper
    {
        private readonly SlotEquipper[] equippers;
        private SlotEquipper equipped;

        public AnySlotEquipper(SlotEquipper[] equippers) 
        {
            this.equippers = equippers;
        }

        public bool IsTrivialEquip(CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
            {
                if (equipper.IsTrivialEquip(manager))
                    return true;
            }
            return false;
        }

        public void Equip(EquipmentItem equipment, CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
            {
                if (equipper.IsTrivialEquip(manager))
                {
                    equipper.Equip(equipment, manager);
                    equipped = equipper;
                    return;
                }
            }
            equippers[0].Equip(equipment, manager);
            equipped = equippers[0];
        }

        public void Unequip(CombatEquipmentManager manager)
        {
            equipped.Unequip(manager);
        }
    }
}
