using UnityEngine;
using JRPG.Data;
using JRPG.Core;

namespace JRPG.Combat
{
    public class CombatStartState : CombatState
    {
        public CombatStartState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Combat Started: Setting up battlefield.");
            combatManager.ChangeState(new PlayerTurnState(combatManager));
        }
    }

    public class PlayerTurnState : CombatState
    {
        public PlayerTurnState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Player's Turn: Waiting for UI input...");
            combatManager.TriggerPlayerTurnUI();
        }

        public override void ExitState()
        {
            combatManager.HidePlayerTurnUI();
        }
    }

    public class EnemyTurnState : CombatState
    {
        public EnemyTurnState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            bool skillExecuted = false;

            // Logika AI sederhana untuk mengecek kepemilikan skill dan kapasitas MP.
            if (combatManager.Enemy.TryGetComponent<SkillComponent>(out var skillComp) && skillComp.AvailableSkills.Count > 0)
            {
                SkillData skillToUse = skillComp.AvailableSkills[0];

                if (combatManager.Enemy.TryGetComponent<ManaComponent>(out var manaComp))
                {
                    if (manaComp.Consume(skillToUse.ManaCost))
                    {
                        Debug.Log($"Enemy AI uses {skillToUse.SkillName}!");
                        Entity target = skillToUse.Type == SkillType.Heal ? combatManager.Enemy : combatManager.Player;
                        combatManager.ExecuteSkill(combatManager.Enemy, target, skillToUse);
                        skillExecuted = true;
                    }
                }
            }

            // Jika tidak ada skill, MP habis, atau tidak punya komponen, lakukan serangan dasar.
            if (!skillExecuted)
            {
                Debug.Log("Enemy's Turn: AI is attacking physically...");
                combatManager.ExecutePhysicalAttack(combatManager.Enemy, combatManager.Player);
            }

            combatManager.CheckCombatEndCondition();
        }
    }

    public class WinState : CombatState
    {
        public WinState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Result: You Win! Combat Ended.");
        }
    }

    public class LoseState : CombatState
    {
        public LoseState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Result: You Lose! Game Over.");
        }
    }
}