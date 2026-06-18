using UnityEngine;
using UnityEngine.Playables;
using JRPG.Core;
using JRPG.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JRPG.Combat
{
    public class CombatManager : MonoBehaviour
    {
        private CombatState currentState;
        private Dictionary<SkillType, ISkillStrategy> skillStrategies;
        private TaskCompletionSource<bool> impactSignalTcs;
        private TaskCompletionSource<bool> timelineEndTcs;

        [Header("Combatants")]
        public PlayerEntity Player;
        public EnemyEntity Enemy;

        [Header("Cinematics")]
        public PlayableDirector CombatDirector;
        public PlayableAsset PlayerAttackTimeline;
        public PlayableAsset EnemyAttackTimeline;
        public PlayableAsset GenericSkillTimeline;

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

        private void OnEnable()
        {
            if (CombatDirector != null) CombatDirector.stopped += OnTimelineStopped;
        }

        private void OnDisable()
        {
            if (CombatDirector != null) CombatDirector.stopped -= OnTimelineStopped;
        }

        public void ChangeState(CombatState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
        }

        public async void OnAttackButtonClicked()
        {
            if (currentState is PlayerTurnState)
            {
                HidePlayerTurnUI();
                await ExecutePhysicalAttackAsync(Player, Enemy, PlayerAttackTimeline);
            }
        }

        public async void OnSkillButtonClicked(SkillData skill)
        {
            if (currentState is PlayerTurnState)
            {
                if (Player.TryGetComponent<ManaComponent>(out var manaComp) && !manaComp.Consume(skill.ManaCost)) return;

                HidePlayerTurnUI();
                Entity target = skill.Type == SkillType.Heal || skill.Type == SkillType.Buff ? Player : Enemy;
                await ExecuteSkillAsync(Player, target, skill);
            }
        }

        public async void OnItemButtonClicked(ItemData item)
        {
            if (currentState is PlayerTurnState)
            {
                if (Player.TryGetComponent<InventoryComponent>(out var invComp) && invComp.RemoveItem(item, 1))
                {
                    HidePlayerTurnUI();
                    await ExecuteItemAsync(Player, item);
                }
            }
        }

        // Dipanggil oleh Timeline Signal Receiver saat impact animasi terjadi.
        public void TriggerImpactSignal()
        {
            impactSignalTcs?.TrySetResult(true);
        }

        // Dipanggil otomatis saat Timeline selesai memutar seluruh durasinya.
        private void OnTimelineStopped(PlayableDirector director)
        {
            timelineEndTcs?.TrySetResult(true);
        }

        // Mengatur ulang TaskCompletionSource dan memutar timeline.
        public void PlayTimeline(PlayableAsset timeline)
        {
            if (timeline == null || CombatDirector == null) return;
            impactSignalTcs = new TaskCompletionSource<bool>();
            timelineEndTcs = new TaskCompletionSource<bool>();
            CombatDirector.playableAsset = timeline;
            CombatDirector.Play();
        }

        // Membekukan eksekusi C# hingga sinyal impact masuk atau menggunakan fallback 0.5 detik.
        public async Task WaitForImpact(bool hasTimeline)
        {
            if (hasTimeline && impactSignalTcs != null) await impactSignalTcs.Task;
            else await Task.Delay(500);
        }

        // Membekukan eksekusi C# hingga timeline selesai atau menggunakan fallback 0.5 detik.
        public async Task WaitForTimelineEnd(bool hasTimeline)
        {
            if (hasTimeline && timelineEndTcs != null) await timelineEndTcs.Task;
            else await Task.Delay(500);
        }

        public async Task ExecutePhysicalAttackAsync(Entity attacker, Entity defender, PlayableAsset timeline)
        {
            bool hasTimeline = timeline != null && CombatDirector != null;
            PlayTimeline(timeline);

            await WaitForImpact(hasTimeline);

            if (defender.TryGetComponent<HealthComponent>(out var targetHealth))
            {
                float damage = attacker.Stats[StatType.Attack].Value;
                if (defender.Stats.ContainsKey(StatType.Defense)) damage = Mathf.Max(1f, damage - defender.Stats[StatType.Defense].Value);
                targetHealth.TakeDamage(damage);
            }

            await WaitForTimelineEnd(hasTimeline);
            CheckCombatEndCondition();
        }

        public async Task ExecuteSkillAsync(Entity caster, Entity target, SkillData skill)
        {
            if (skillStrategies.TryGetValue(skill.Type, out var strategy))
            {
                await strategy.ExecuteAsync(caster, target, skill, this);
            }
        }

        public async Task ExecuteItemAsync(Entity target, ItemData item)
        {
            await Task.Delay(500);
            if (item.Type == ItemType.Consumable && target.TryGetComponent<HealthComponent>(out var healthComp)) healthComp.Heal(item.EffectValue);
            await Task.Delay(500);
            CheckCombatEndCondition();
        }

        public void ProcessStatusEffects(Entity entity)
        {
            if (entity.TryGetComponent<StatusEffectComponent>(out var statusComp)) statusComp.ProcessTurn();
        }

        public void CheckCombatEndCondition()
        {
            var enemyHealth = Enemy.GetComponent<HealthComponent>();
            var playerHealth = Player.GetComponent<HealthComponent>();

            if (enemyHealth != null && enemyHealth.GetCurrentHealth() <= 0) ChangeState(new WinState(this));
            else if (playerHealth != null && playerHealth.GetCurrentHealth() <= 0) ChangeState(new LoseState(this));
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