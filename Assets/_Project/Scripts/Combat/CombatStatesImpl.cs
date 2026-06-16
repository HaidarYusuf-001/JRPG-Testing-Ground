using UnityEngine;

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
            Debug.Log("Enemy's Turn: AI is attacking...");
            combatManager.ExecutePhysicalAttack(combatManager.Enemy, combatManager.Player);
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