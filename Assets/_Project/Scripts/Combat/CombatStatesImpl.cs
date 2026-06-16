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
            Debug.Log("Player's Turn: Press SPACE to Attack.");
        }

        public override void UpdateState()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                combatManager.Player.DealDamage(combatManager.Enemy);

                if (combatManager.Enemy.Stats["HP"].CurrentValue <= 0)
                {
                    combatManager.ChangeState(new WinState(combatManager));
                }
                else
                {
                    combatManager.ChangeState(new EnemyTurnState(combatManager));
                }
            }
        }
    }

    public class EnemyTurnState : CombatState
    {
        public EnemyTurnState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Enemy's Turn: AI is attacking...");
            combatManager.Enemy.DealDamage(combatManager.Player);

            if (combatManager.Player.Stats["HP"].CurrentValue <= 0)
            {
                combatManager.ChangeState(new LoseState(combatManager));
            }
            else
            {
                combatManager.ChangeState(new PlayerTurnState(combatManager));
            }
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