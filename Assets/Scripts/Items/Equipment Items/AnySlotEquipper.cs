using Units.Combat;

namespace Items.EquipmentItems
{
    /// <summary>
    /// Equip using the first equipper that is available
    /// </summary>
    public class AnySlotEquipper : SlotEquipper
    {
        private readonly SlotEquipper[] equippers;
        private SlotEquipper equipped;

        public AnySlotEquipper(SlotEquipper[] equippers) 
        {
            this.equippers = equippers;
        }

        public bool SlotEquipIsAvailable(CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
            {
                if (equipper.SlotEquipIsAvailable(manager))
                    return true;
            }
            return false;
        }

        public void Equip(EquipmentItem equipment, CombatEquipmentManager manager)
        {
            foreach (SlotEquipper equipper in equippers)
            {
                if (equipper.SlotEquipIsAvailable(manager))
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
