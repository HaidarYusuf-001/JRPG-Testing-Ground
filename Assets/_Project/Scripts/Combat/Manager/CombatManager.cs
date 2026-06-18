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

        private void Awake()
        {
            skillStrategies = new Dictionary<SkillType, ISkillStrategy>
            {
                { SkillType.Damage, new DamageSkillStrategy() },
                { SkillType.Heal, new HealSkillStrategy() },
                { SkillType.Buff, new BuffSkillStrategy() }
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

                Entity target = skill.Type == SkillType.Heal || skill.Type == SkillType.Buff ? Player : Enemy;
                ExecuteSkill(Player, target, skill);
                CheckCombatEndCondition();
            }
        }

        public void OnItemButtonClicked(ItemData item)
        {
            if (currentState is PlayerTurnState)
            {
                if (Player.TryGetComponent<InventoryComponent>(out var invComp))
                {
                    if (invComp.RemoveItem(item, 1))
                    {
                        Debug.Log($"{Player.gameObject.name} uses {item.ItemName}!");
                        ApplyItemEffect(Player, item);
                        CheckCombatEndCondition();
                    }
                }
            }
        }

        private void ApplyItemEffect(Entity target, ItemData item)
        {
            if (item.Type == ItemType.Consumable)
            {
                if (target.TryGetComponent<HealthComponent>(out var healthComp))
                {
                    healthComp.Heal(item.EffectValue);
                }
            }
        }

        public void ExecuteSkill(Entity caster, Entity target, SkillData skill)
        {
            if (skillStrategies.TryGetValue(skill.Type, out var strategy))
            {
                strategy.Execute(caster, target, skill);
            }
        }

        public void ExecutePhysicalAttack(Entity attacker, Entity defender)
        {
            if (!defender.TryGetComponent<HealthComponent>(out var targetHealth)) return;

            float damage = attacker.Stats[StatType.Attack].Value;
            if (defender.Stats.ContainsKey(StatType.Defense))
            {
                damage = Mathf.Max(1f, damage - defender.Stats[StatType.Defense].Value);
            }

            Debug.Log($"{attacker.gameObject.name} attacks for {damage} damage!");
            targetHealth.TakeDamage(damage);
        }

        public void ProcessStatusEffects(Entity entity)
        {
            if (entity.TryGetComponent<StatusEffectComponent>(out var statusComp))
            {
                statusComp.ProcessTurn();
            }
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