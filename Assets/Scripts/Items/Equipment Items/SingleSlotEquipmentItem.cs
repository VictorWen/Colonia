using Units.Combat;

namespace Items.EquipmentItems
{
    public interface SlotEquipper
    {
        bool IsTrivialEquip(CombatEquipmentManager manager);

        void Equip(EquipmentItem equipment, CombatEquipmentManager manager);

        void Unequip(CombatEquipmentManager manage);
    }

    public class SingleSlotEquipper : SlotEquipper
    {
        private readonly UnitEntityEquipmentSlotID slotID;

        public SingleSlotEquipper(UnitEntityEquipmentSlotID slotID)
        {
            this.slotID = slotID;
        }

        public bool IsTrivialEquip(CombatEquipmentManager manager)
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
