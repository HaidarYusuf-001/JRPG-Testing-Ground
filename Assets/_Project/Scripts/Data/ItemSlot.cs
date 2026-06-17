using System;

namespace JRPG.Data
{
    // Representasi jumlah item dalam satu slot inventory.
    [Serializable]
    public class ItemSlot
    {
        public ItemData Item;
        public int Quantity;

        public ItemSlot(ItemData item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}