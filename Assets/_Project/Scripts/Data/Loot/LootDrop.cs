using System;
using UnityEngine;

namespace JRPG.Data
{
    // Mendefinisikan probabilitas dan kuantitas item yang dijatuhkan setelah pertarungan.
    [Serializable]
    public class LootDrop
    {
        public ItemData Item;
        [Range(0f, 100f)] public float DropChance;
        public int MinQuantity = 1;
        public int MaxQuantity = 1;
    }
}