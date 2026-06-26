using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    // Menyimpan data pemain secara permanen.
    public class PersistentPlayerData : MonoBehaviour
    {
        public static PersistentPlayerData Instance { get; private set; }

        public Vector3 LastMapPosition;
        public bool HasSavedPosition = false;

        public int Level = 1;
        public int CurrentExp = 0;
        public int Gold = 0;

        public float CurrentHP = -1f;
        public float CurrentMP = -1f;

        public List<ItemSlot> SavedInventory = new List<ItemSlot>();
        public Dictionary<EquipmentSlot, EquipmentData> SavedEquipment = new Dictionary<EquipmentSlot, EquipmentData>();
        public bool HasInitializedInventory = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ResetData()
        {
            Level = 1;
            CurrentExp = 0;
            Gold = 0;
            CurrentHP = -1f;
            CurrentMP = -1f;
            HasSavedPosition = false;
            LastMapPosition = Vector3.zero;
            HasInitializedInventory = false;
            SavedInventory.Clear();
            SavedEquipment.Clear();
        }
    }
}