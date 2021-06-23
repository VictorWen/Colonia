using Items;
using System;
using System.Collections.Generic;

namespace Units.Combat
{
    public class CombatEquipmentManager
    {
        private readonly Inventory inventory;
        private readonly Dictionary<UnitEntityEquipmentSlotID, EquipmentItem> equipmentSlots;

        public CombatEquipmentManager(Inventory inventory)
        {
            this.inventory = inventory;
            equipmentSlots = new Dictionary<UnitEntityEquipmentSlotID, EquipmentItem>();
            foreach (UnitEntityEquipmentSlotID id in Enum.GetValues(typeof(UnitEntityEquipmentSlotID)))
            {
                equipmentSlots.Add(id, null);
            }
        }

        public void EquipEquipmentItem(EquipmentItem equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentTypeID.HELMET:
                    EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.HEAD);
                    break;
                case EquipmentTypeID.BODY_ARMOR:
                    EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.BODY);
                    break;
                case EquipmentTypeID.BOOTS:
                    EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.BOOTS);
                    break;
                case EquipmentTypeID.ARTIFACT:
                    EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.ARTIFACT);
                    break;
                case EquipmentTypeID.ONE_HANDED:
                    if (equipmentSlots[UnitEntityEquipmentSlotID.WEAPON1] != null &&
                        equipmentSlots[UnitEntityEquipmentSlotID.WEAPON2] == null)
                        equipmentSlots[UnitEntityEquipmentSlotID.WEAPON2] = equipment;
                    else
                        EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.WEAPON1);
                    break;
                case EquipmentTypeID.TWO_HANDED:
                    EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.WEAPON1);
                    EquipEquipmentIntoSlot(equipment, UnitEntityEquipmentSlotID.WEAPON2);
                    break;
            }
        }

        public int CalculateEquipmentAttribute(CombatAttributeID attribute)
        {
            int sum = 0;
            foreach (EquipmentItem equipment in equipmentSlots.Values)
            {
                if (equipment != null && equipment.Additives.ContainsKey(attribute))
                    sum += equipment.Additives[attribute];
            }
            return sum;
        }

        private void EquipEquipmentIntoSlot(EquipmentItem equipment, UnitEntityEquipmentSlotID slot)
        {
            UnequipEquipmentSlot(slot);
            equipmentSlots[slot] = equipment;
        }

        private void UnequipEquipmentSlot(UnitEntityEquipmentSlotID slot)
        {
            EquipmentItem equipment = equipmentSlots[slot];
            if (equipment == null)
                return;
            inventory.AddItem(equipment);
            equipmentSlots[slot] = null;
            if (equipment.EquipmentType == EquipmentTypeID.TWO_HANDED)
                equipmentSlots[UnitEntityEquipmentSlotID.WEAPON2] = null;
        }
    }
}
