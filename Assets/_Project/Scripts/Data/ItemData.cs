using UnityEngine;

namespace JRPG.Data
{
    public enum ItemType { Consumable, Equipment, KeyItem }

    // Menyimpan atribut murni dari sebuah item.
    [CreateAssetMenu(fileName = "NewItemData", menuName = "JRPG/Data/Item Data")]
    public class ItemData : ScriptableObject
    {
        public string ItemName;
        public ItemType Type;
        public float EffectValue;
        [TextArea(2, 3)] public string Description;
    }
}