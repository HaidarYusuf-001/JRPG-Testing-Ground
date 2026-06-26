using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    public class PlayerEntity : Entity
    {
        public EntityData BaseData;
        private PlayerProgressComponent progress;

        protected override void InitializeStats()
        {
            if (BaseData == null) return;

            int currentLevel = PersistentPlayerData.Instance != null ? PersistentPlayerData.Instance.Level : 1;
            CalculateStats(currentLevel, true);

            if (TryGetComponent<SkillComponent>(out var skillComp)) skillComp.Initialize(BaseData.BaseSkills);

            // Tampung status pengecekan di awal
            bool isNewGame = PersistentPlayerData.Instance == null || !PersistentPlayerData.Instance.HasInitializedInventory;

            if (TryGetComponent<InventoryComponent>(out var invComp))
            {
                if (!isNewGame)
                {
                    invComp.Slots = new List<ItemSlot>(PersistentPlayerData.Instance.SavedInventory);
                }
                else
                {
                    invComp.Initialize(BaseData.StartingItems);
                }

                if (TryGetComponent<EquipmentComponent>(out var equipComp))
                {
                    if (!isNewGame)
                    {
                        equipComp.EquippedItems = new Dictionary<EquipmentSlot, EquipmentData>(PersistentPlayerData.Instance.SavedEquipment);
                        ApplySavedEquipmentModifiers(equipComp);
                    }
                    else
                    {
                        AutoEquipStartingGear(invComp, equipComp);
                    }
                }

                // Kunci status inisialisasi hanya setelah inventory dan equipment selesai diproses
                if (isNewGame && PersistentPlayerData.Instance != null)
                {
                    PersistentPlayerData.Instance.SavedInventory = new List<ItemSlot>(invComp.Slots);
                    if (TryGetComponent<EquipmentComponent>(out var ec))
                    {
                        PersistentPlayerData.Instance.SavedEquipment = new Dictionary<EquipmentSlot, EquipmentData>(ec.EquippedItems);
                    }
                    PersistentPlayerData.Instance.HasInitializedInventory = true;
                }
            }

            if (TryGetComponent(out progress)) progress.OnLevelUp += HandleLevelUp;
        }

        private void OnDestroy()
        {
            if (progress != null) progress.OnLevelUp -= HandleLevelUp;
        }

        private void CalculateStats(int level, bool isInitialization)
        {
            float levelMultiplier = level - 1;

            Stats[StatType.Attack] = new Stat { BaseValue = BaseData.BaseAttack + (BaseData.GrowthAttack * levelMultiplier) };
            Stats[StatType.Defense] = new Stat { BaseValue = BaseData.BaseDefense + (BaseData.GrowthDefense * levelMultiplier) };

            float calculatedMaxHP = BaseData.BaseHP + (BaseData.GrowthHP * levelMultiplier);
            float calculatedMaxMP = BaseData.BaseMP + (BaseData.GrowthMP * levelMultiplier);

            if (TryGetComponent<HealthComponent>(out var healthComp))
            {
                if (isInitialization)
                {
                    healthComp.Initialize(calculatedMaxHP);
                    if (PersistentPlayerData.Instance != null && PersistentPlayerData.Instance.CurrentHP != -1f)
                    {
                        healthComp.SetCurrentHealth(PersistentPlayerData.Instance.CurrentHP);
                    }
                }
                else healthComp.UpgradeMaxHealth(calculatedMaxHP, true);
            }

            if (TryGetComponent<ManaComponent>(out var manaComp))
            {
                if (isInitialization)
                {
                    manaComp.Initialize(calculatedMaxMP);
                    if (PersistentPlayerData.Instance != null && PersistentPlayerData.Instance.CurrentMP != -1f)
                    {
                        manaComp.SetCurrentMana(PersistentPlayerData.Instance.CurrentMP);
                    }
                }
                else manaComp.UpgradeMaxMana(calculatedMaxMP, true);
            }
        }

        private void HandleLevelUp(int newLevel)
        {
            Debug.Log($"Level Up Triggered! PlayerEntity recalculating stats for Level {newLevel}");
            CalculateStats(newLevel, false);
        }

        private void AutoEquipStartingGear(InventoryComponent inventory, EquipmentComponent equipment)
        {
            for (int i = inventory.Slots.Count - 1; i >= 0; i--)
            {
                if (inventory.Slots[i].Item is EquipmentData equipData) EquipItem(equipData);
            }
        }

        private void ApplySavedEquipmentModifiers(EquipmentComponent equipment)
        {
            foreach (var kvp in equipment.EquippedItems)
            {
                if (Stats.ContainsKey(kvp.Value.TargetStat))
                {
                    Stats[kvp.Value.TargetStat].AddModifier(kvp.Value.ModifierValue);
                }
            }
        }

        public void EquipItem(EquipmentData equipment)
        {
            if (!TryGetComponent<InventoryComponent>(out var invComp) || !TryGetComponent<EquipmentComponent>(out var equipComp)) return;

            if (equipComp.EquippedItems.ContainsKey(equipment.SlotType)) UnequipItem(equipment.SlotType);

            if (invComp.RemoveItem(equipment, 1))
            {
                equipComp.Equip(equipment);
                if (Stats.ContainsKey(equipment.TargetStat)) Stats[equipment.TargetStat].AddModifier(equipment.ModifierValue);
                SyncInventoryAndEquipment();
            }
        }

        public void UnequipItem(EquipmentSlot slotType)
        {
            if (!TryGetComponent<InventoryComponent>(out var invComp) || !TryGetComponent<EquipmentComponent>(out var equipComp)) return;

            if (equipComp.EquippedItems.TryGetValue(slotType, out EquipmentData eq))
            {
                equipComp.Unequip(slotType);
                if (Stats.ContainsKey(eq.TargetStat)) Stats[eq.TargetStat].RemoveModifier(eq.ModifierValue);
                invComp.AddItem(eq, 1);
                SyncInventoryAndEquipment();
            }
        }

        private void SyncInventoryAndEquipment()
        {
            if (PersistentPlayerData.Instance != null)
            {
                if (TryGetComponent<InventoryComponent>(out var invComp)) PersistentPlayerData.Instance.SavedInventory = new List<ItemSlot>(invComp.Slots);
                if (TryGetComponent<EquipmentComponent>(out var equipComp)) PersistentPlayerData.Instance.SavedEquipment = new Dictionary<EquipmentSlot, EquipmentData>(equipComp.EquippedItems);
            }
        }
    }
}