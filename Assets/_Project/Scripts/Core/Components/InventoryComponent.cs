using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;
using System.Linq;

namespace JRPG.Core
{
    // Mengatur penyimpanan dan manipulasi item entitas saat runtime.
    public class InventoryComponent : MonoBehaviour
    {
        public List<ItemSlot> Slots = new List<ItemSlot>();

        // Memuat item awal dari data statis ke inventory.
        public void Initialize(List<ItemSlot> startingItems)
        {
            Slots.Clear();
            if (startingItems == null) return;

            foreach (var slot in startingItems)
            {
                AddItem(slot.Item, slot.Quantity);
            }
        }

        // Menambahkan item baru atau menambah quantity jika item sudah ada.
        public void AddItem(ItemData item, int amount = 1)
        {
            var existingSlot = Slots.FirstOrDefault(s => s.Item == item);
            if (existingSlot != null)
            {
                existingSlot.Quantity += amount;
            }
            else
            {
                Slots.Add(new ItemSlot(item, amount));
            }
        }

        // Mengurangi kuantitas item dan menghapus slot jika habis.
        public bool RemoveItem(ItemData item, int amount = 1)
        {
            var existingSlot = Slots.FirstOrDefault(s => s.Item == item);
            if (existingSlot == null || existingSlot.Quantity < amount) return false;

            existingSlot.Quantity -= amount;
            if (existingSlot.Quantity <= 0)
            {
                Slots.Remove(existingSlot);
            }
            return true;
        }
    }
}