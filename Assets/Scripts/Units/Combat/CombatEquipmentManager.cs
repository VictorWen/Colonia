using Items;
using Items.EquipmentItems;
using System;
using System.Collections.Generic;

namespace Units.Combat
{
    public class CombatEquipmentManager
    {
        private readonly Inventory inventory;
        private readonly Dictionary<UnitEntityEquipmentSlotID, EquipmentItem> equipmentSlots;
        private readonly HashSet<EquipmentItem> equipped;

        public CombatEquipmentManager(Inventory inventory)
        {
            this.inventory = inventory;
            equipmentSlots = new Dictionary<UnitEntityEquipmentSlotID, EquipmentItem>();
            foreach (UnitEntityEquipmentSlotID id in Enum.GetValues(typeof(UnitEntityEquipmentSlotID)))
            {
                equipmentSlots.Add(id, null);
            }
            equipped = new HashSet<EquipmentItem>();
        }

        public bool SlotIsOccupied(UnitEntityEquipmentSlotID slotID)
        {
            return equipmentSlots[slotID] != null;
        }

        public void PutEquipmentIntoSlot(EquipmentItem equipment, UnitEntityEquipmentSlotID slotID)
        {
            equipmentSlots[slotID] = equipment;
        }

        public void ClearEquipmentSlot(UnitEntityEquipmentSlotID slotID)
        {
            equipmentSlots[slotID] = null;
        }

        public void UnequipEquipmentFromSlot(UnitEntityEquipmentSlotID slotID)
        {
            EquipmentItem equipment = equipmentSlots[slotID];
            equipment.SlotEquipper.Unequip(this);
            equipped.Remove(equipment);
            inventory.AddItem(equipment);
        }

        public void EquipEquipmentItem(EquipmentItem equipment)
        {
            equipment.SlotEquipper.Equip(equipment, this);
            equipped.Add(equipment);
        }

        public int CalculateEquipmentAttribute(CombatAttributeID attribute)
        {
            int sum = 0;
            foreach (EquipmentItem equipment in equipped)
            {
                if (equipment != null && equipment.Additives.ContainsKey(attribute))
                    sum += equipment.Additives[attribute];
            }
            return sum;
        }
    }
}
