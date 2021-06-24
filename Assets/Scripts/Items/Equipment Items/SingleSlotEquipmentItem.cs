using Units.Combat;

namespace Items.EquipmentItems
{
    public interface SlotEquipper
    {
        bool SlotEquipIsAvailable(CombatEquipmentManager manager);

        void Equip(EquipmentItem equipment, CombatEquipmentManager manager);

        void Unequip(CombatEquipmentManager manage);
    }

    /// <summary>
    /// Will try to equip equipment into a single slot
    /// </summary>
    public class SingleSlotEquipper : SlotEquipper
    {
        private readonly UnitEntityEquipmentSlotID slotID;

        public SingleSlotEquipper(UnitEntityEquipmentSlotID slotID)
        {
            this.slotID = slotID;
        }

        public bool SlotEquipIsAvailable(CombatEquipmentManager manager)
        {
            return !manager.SlotIsOccupied(slotID);
        }

        public void Equip(EquipmentItem equipment, CombatEquipmentManager manager)
        {
            if (manager.SlotIsOccupied(slotID))
                manager.UnequipEquipmentFromSlot(slotID);
            manager.PutEquipmentIntoSlot(equipment, slotID);
        }

        public void Unequip(CombatEquipmentManager manager)
        {
            manager.ClearEquipmentSlot(slotID);
        }
    }
}
