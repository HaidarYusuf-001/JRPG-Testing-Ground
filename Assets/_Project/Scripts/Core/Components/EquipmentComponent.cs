using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    public class EquipmentComponent : MonoBehaviour
    {
        public Dictionary<EquipmentSlot, EquipmentData> EquippedItems = new Dictionary<EquipmentSlot, EquipmentData>();
        private Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        public void Equip(EquipmentData equipment)
        {
            if (EquippedItems.ContainsKey(equipment.SlotType))
            {
                Unequip(equipment.SlotType);
            }

            EquippedItems[equipment.SlotType] = equipment;

            if (entity.Stats.ContainsKey(equipment.TargetStat))
            {
                entity.Stats[equipment.TargetStat].AddModifier(equipment.ModifierValue);
            }

            Debug.Log($"{gameObject.name} equipped {equipment.ItemName}");
        }

        public void Unequip(EquipmentSlot slot)
        {
            if (EquippedItems.TryGetValue(slot, out EquipmentData equipment))
            {
                if (entity.Stats.ContainsKey(equipment.TargetStat))
                {
                    entity.Stats[equipment.TargetStat].RemoveModifier(equipment.ModifierValue);
                }

                EquippedItems.Remove(slot);
                Debug.Log($"{gameObject.name} unequipped {equipment.ItemName}");
            }
        }
    }
}