using Items;
using Items.EquipmentItems;
using System;
using System.Collections.Generic;

namespace Units.Combat
{
    public class CombatEquipmentManager
    {
        public Dictionary<UnitEntityEquipmentSlotID, EquipmentItem> EquipmentSlots { get; private set; }

        private readonly Inventory inventory;
        private readonly HashSet<EquipmentItem> equipped;

        public CombatEquipmentManager(Inventory inventory)
        {
            this.inventory = inventory;
            EquipmentSlots = new Dictionary<UnitEntityEquipmentSlotID, EquipmentItem>();
            foreach (UnitEntityEquipmentSlotID id in Enum.GetValues(typeof(UnitEntityEquipmentSlotID)))
            {
                EquipmentSlots.Add(id, null);
            }
            equipped = new HashSet<EquipmentItem>();
        }

        public bool SlotIsOccupied(UnitEntityEquipmentSlotID slotID)
        {
            return EquipmentSlots[slotID] != null;
        }

        public void PutEquipmentIntoSlot(EquipmentItem equipment, UnitEntityEquipmentSlotID slotID)
        {
            EquipmentSlots[slotID] = equipment;
        }

        public void ClearEquipmentSlot(UnitEntityEquipmentSlotID slotID)
        {
            EquipmentSlots[slotID] = null;
        }

        public void UnequipEquipmentFromSlot(UnitEntityEquipmentSlotID slotID)
        {
            EquipmentItem equipment = EquipmentSlots[slotID];
            UnequipEquipmentItem(equipment);
        }

        public void EquipEquipmentItem(EquipmentItem equipment)
        {
            equipment.SlotEquipper.Equip(equipment, this);
            equipped.Add(equipment);
        }

        public void UnequipEquipmentItem(EquipmentItem equipment)
        {
            if (!equipped.Contains(equipment))
                return;
            equipment.SlotEquipper.Unequip(this);
            equipped.Remove(equipment);
            inventory.AddItem(equipment);
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
