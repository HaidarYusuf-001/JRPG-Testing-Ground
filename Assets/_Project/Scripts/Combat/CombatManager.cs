using UnityEngine;
using JRPG.Core;
using System;

namespace JRPG.Combat
{
    // Context utama pengatur state machine combat.
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

        // Mengeksekusi serangan fisik dari UI dan mengatur transisi turn.
        public void OnAttackButtonClicked()
        {
            if (currentState is PlayerTurnState)
            {
                ExecutePhysicalAttack(Player, Enemy);
                CheckCombatEndCondition();
            }
        }

        // Logika kalkulasi serangan fisik murni.
        public void ExecutePhysicalAttack(Entity attacker, Entity defender)
        {
            if (!defender.TryGetComponent<HealthComponent>(out var targetHealth)) return;

            float damage = attacker.Stats["Attack"].CurrentValue;
            if (defender.Stats.ContainsKey("Defense"))
            {
                damage = Mathf.Max(1f, damage - defender.Stats["Defense"].CurrentValue);
            }

            Debug.Log($"{attacker.gameObject.name} attacks for {damage} damage!");
            targetHealth.TakeDamage(damage);
        }

        // Memeriksa status HP untuk menentukan kemenangan atau transisi.
        public void CheckCombatEndCondition()
        {
            var enemyHealth = Enemy.GetComponent<HealthComponent>();
            var playerHealth = Player.GetComponent<HealthComponent>();

            if (enemyHealth != null && enemyHealth.GetCurrentHealth() <= 0)
            {
                ChangeState(new WinState(this));
            }
            else if (playerHealth != null && playerHealth.GetCurrentHealth() <= 0)
            {
                ChangeState(new LoseState(this));
            }
            else
            {
                // Toggle turn sederhana.
                if (currentState is PlayerTurnState) ChangeState(new EnemyTurnState(this));
                else ChangeState(new PlayerTurnState(this));
            }
        }

        public void TriggerPlayerTurnUI() => OnPlayerTurnStarted?.Invoke();
        public void HidePlayerTurnUI() => OnPlayerTurnEnded?.Invoke();
    }
}