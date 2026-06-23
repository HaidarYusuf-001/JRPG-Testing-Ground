using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    // Implementasi entitas khusus untuk karakter pemain dengan fitur scaling level.
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

            if (TryGetComponent<InventoryComponent>(out var invComp))
            {
                invComp.Initialize(BaseData.StartingItems);
                if (TryGetComponent<EquipmentComponent>(out var equipComp)) AutoEquipStartingGear(invComp, equipComp);
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
                if (isInitialization) healthComp.Initialize(calculatedMaxHP);
                else healthComp.UpgradeMaxHealth(calculatedMaxHP, true);
            }

            if (TryGetComponent<ManaComponent>(out var manaComp))
            {
                if (isInitialization) manaComp.Initialize(calculatedMaxMP);
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
            foreach (var slot in inventory.Slots)
            {
                if (slot.Item is EquipmentData equipData) equipment.Equip(equipData);
            }
        }
    }
}