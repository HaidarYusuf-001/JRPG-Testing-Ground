using UnityEngine;

namespace JRPG.Data
{
    [CreateAssetMenu(fileName = "NewEquipment", menuName = "JRPG/Data/Equipment Data")]
    public class EquipmentData : ItemData
    {
        public EquipmentSlot SlotType;
        public StatType TargetStat;
        public float ModifierValue;

        private void OnEnable()
        {
            Type = ItemType.Equipment;
        }
    }
}