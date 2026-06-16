using UnityEngine;
using JRPG.Core;
using System;

namespace JRPG.Combat
{
    public class CombatManager : MonoBehaviour
    {
        private CombatState currentState;

        [Header("Combatants")]
        public PlayerEntity Player;
        public EnemyEntity Enemy;

        public event Action OnPlayerTurnStarted;
        public event Action OnPlayerTurnEnded;

        private void Start()
        {
            ChangeState(new CombatStartState(this));
        }

        private void Update()
        {
            currentState?.UpdateState();
        }

        public void ChangeState(CombatState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
        }

        // Mengeksekusi serangan pemain saat tombol UI ditekan
        public void OnAttackButtonClicked()
        {
            if (currentState is PlayerTurnState)
            {
                Player.DealDamage(Enemy);

                if (Enemy.Stats["HP"].CurrentValue <= 0)
                {
                    ChangeState(new WinState(this));
                }
                else
                {
                    ChangeState(new EnemyTurnState(this));
                }
            }
        }

        // Memancarkan event bahwa giliran pemain dimulai
        public void TriggerPlayerTurnUI()
        {
            OnPlayerTurnStarted?.Invoke();
        }

        // Memancarkan event bahwa giliran pemain selesai
        public void HidePlayerTurnUI()
        {
            OnPlayerTurnEnded?.Invoke();
        }
    }
}