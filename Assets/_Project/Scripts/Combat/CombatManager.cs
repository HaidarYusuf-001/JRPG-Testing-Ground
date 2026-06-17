using UnityEngine;
using JRPG.Core;
using JRPG.Data;
using System;
using System.Collections.Generic;

namespace JRPG.Combat
{
    public class CombatManager : MonoBehaviour
    {
        private CombatState currentState;
        private Dictionary<SkillType, ISkillStrategy> skillStrategies;

        [Header("Combatants")]
        public PlayerEntity Player;
        public EnemyEntity Enemy;

        public event Action OnPlayerTurnStarted;
        public event Action OnPlayerTurnEnded;

        // Mendaftarkan implementasi strategy pattern ke dalam dictionary.
        private void Awake()
        {
            skillStrategies = new Dictionary<SkillType, ISkillStrategy>
            {
                { SkillType.Damage, new DamageSkillStrategy() },
                { SkillType.Heal, new HealSkillStrategy() }
            };
        }

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

        public void OnAttackButtonClicked()
        {
            if (currentState is PlayerTurnState)
            {
                ExecutePhysicalAttack(Player, Enemy);
                CheckCombatEndCondition();
            }
        }

        // Mengeksekusi skill berdasarkan input tombol dinamis dari UI.
        public void OnSkillButtonClicked(SkillData skill)
        {
            if (currentState is PlayerTurnState)
            {
                if (Player.TryGetComponent<ManaComponent>(out var manaComp))
                {
                    if (!manaComp.Consume(skill.ManaCost))
                    {
                        Debug.Log("Not enough MP!");
                        return;
                    }
                }

                Entity target = skill.Type == SkillType.Heal ? Player : Enemy;

                if (skillStrategies.TryGetValue(skill.Type, out var strategy))
                {
                    strategy.Execute(Player, target, skill);
                    CheckCombatEndCondition();
                }
            }
        }

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
                if (currentState is PlayerTurnState) ChangeState(new EnemyTurnState(this));
                else ChangeState(new PlayerTurnState(this));
            }
        }

        public void TriggerPlayerTurnUI() => OnPlayerTurnStarted?.Invoke();
        public void HidePlayerTurnUI() => OnPlayerTurnEnded?.Invoke();
    }
}