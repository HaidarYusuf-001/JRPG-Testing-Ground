using UnityEngine;
using JRPG.Data;
using JRPG.Core;
using System.Collections.Generic;

namespace JRPG.Combat
{
    // ... (CombatStartState, PlayerTurnState, EnemyTurnState tetap sama persis seperti sebelumnya) ...

    public class CombatStartState : CombatState
    {
        public CombatStartState(CombatManager manager) : base(manager) { }
        public override void EnterState() { combatManager.ChangeState(new PlayerTurnState(combatManager)); }
    }

    public class PlayerTurnState : CombatState
    {
        public PlayerTurnState(CombatManager manager) : base(manager) { }
        public override void EnterState()
        {
            combatManager.ProcessStatusEffects(combatManager.Player);
            if (combatManager.Player.TryGetComponent<HealthComponent>(out var hp) && hp.GetCurrentHealth() <= 0) { combatManager.CheckCombatEndCondition(); return; }
            combatManager.TriggerPlayerTurnUI();
        }
        public override void ExitState() { }
    }

    public class EnemyTurnState : CombatState
    {
        public EnemyTurnState(CombatManager manager) : base(manager) { }
        public override async void EnterState()
        {
            combatManager.ProcessStatusEffects(combatManager.Enemy);
            if (combatManager.Enemy.TryGetComponent<HealthComponent>(out var hp) && hp.GetCurrentHealth() <= 0) { combatManager.CheckCombatEndCondition(); return; }

            bool skillExecuted = false;
            if (combatManager.Enemy.TryGetComponent<SkillComponent>(out var skillComp) && skillComp.AvailableSkills.Count > 0)
            {
                SkillData skillToUse = skillComp.AvailableSkills[0];
                if (combatManager.Enemy.TryGetComponent<ManaComponent>(out var manaComp) && manaComp.Consume(skillToUse.ManaCost))
                {
                    Entity target = skillToUse.Type == SkillType.Heal || skillToUse.Type == SkillType.Buff ? combatManager.Enemy : combatManager.Player;
                    await combatManager.ExecuteSkillAsync(combatManager.Enemy, target, skillToUse);
                    skillExecuted = true;
                }
            }

            if (!skillExecuted) await combatManager.ExecutePhysicalAttackAsync(combatManager.Enemy, combatManager.Player, combatManager.EnemyAttackTimeline);
        }
    }

    public class WinState : CombatState
    {
        public WinState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Result: You Win! Combat Ended.");
            ProcessRewards();
            SyncDataToPersistence();
            combatManager.TriggerCombatEnd(true);
        }

        private void ProcessRewards()
        {
            var enemyData = combatManager.Enemy.BaseData;
            if (enemyData == null) return;

            if (combatManager.Player.TryGetComponent<PlayerProgressComponent>(out var progress))
            {
                progress.AddExp(enemyData.ExpReward);
                progress.AddGold(enemyData.GoldReward);
            }

            if (combatManager.Player.TryGetComponent<InventoryComponent>(out var inventory) && enemyData.LootDrops != null)
            {
                foreach (var drop in enemyData.LootDrops)
                {
                    if (Random.Range(0f, 100f) <= drop.DropChance)
                    {
                        int amount = Random.Range(drop.MinQuantity, drop.MaxQuantity + 1);
                        inventory.AddItem(drop.Item, amount);
                    }
                }
            }
        }

        private void SyncDataToPersistence()
        {
            if (PersistentPlayerData.Instance == null) return;
            if (combatManager.Player.TryGetComponent<HealthComponent>(out var hp)) PersistentPlayerData.Instance.CurrentHP = hp.GetCurrentHealth();
            if (combatManager.Player.TryGetComponent<ManaComponent>(out var mp)) PersistentPlayerData.Instance.CurrentMP = mp.GetCurrentMana();
            if (combatManager.Player.TryGetComponent<InventoryComponent>(out var inv)) PersistentPlayerData.Instance.SavedInventory = new List<ItemSlot>(inv.Slots);

            // Equipment dikelola oleh Overworld UI, jika combat mendukung mid-fight equip, tambahkan sync equipment di sini.
        }
    }

    public class LoseState : CombatState
    {
        public LoseState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Result: You Lose! Game Over.");
            SyncDataToPersistence();
            combatManager.TriggerCombatEnd(false);
        }

        private void SyncDataToPersistence()
        {
            if (PersistentPlayerData.Instance == null) return;
            if (combatManager.Player.TryGetComponent<HealthComponent>(out var hp)) PersistentPlayerData.Instance.CurrentHP = hp.GetCurrentHealth();
            if (combatManager.Player.TryGetComponent<ManaComponent>(out var mp)) PersistentPlayerData.Instance.CurrentMP = mp.GetCurrentMana();
            if (combatManager.Player.TryGetComponent<InventoryComponent>(out var inv)) PersistentPlayerData.Instance.SavedInventory = new List<ItemSlot>(inv.Slots);
        }
    }
}