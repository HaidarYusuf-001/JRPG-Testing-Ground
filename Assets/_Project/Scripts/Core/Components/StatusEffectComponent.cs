using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;
using System.Linq;

namespace JRPG.Core
{
    public class ActiveEffect
    {
        public StatusEffectData Data;
        public int RemainingTurns;
    }

    public class StatusEffectComponent : MonoBehaviour
    {
        public List<ActiveEffect> ActiveEffects = new List<ActiveEffect>();
        private Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        public void ApplyEffect(StatusEffectData effectData)
        {
            var existing = ActiveEffects.FirstOrDefault(e => e.Data == effectData);
            if (existing != null)
            {
                existing.RemainingTurns = effectData.DurationInTurns;
                return;
            }

            ActiveEffects.Add(new ActiveEffect { Data = effectData, RemainingTurns = effectData.DurationInTurns });

            if (effectData.Type == EffectType.StatModifier && entity != null && entity.Stats.ContainsKey(effectData.TargetStat))
            {
                entity.Stats[effectData.TargetStat].AddModifier(effectData.Value);
            }
        }

        public void ProcessTurn()
        {
            for (int i = ActiveEffects.Count - 1; i >= 0; i--)
            {
                var effect = ActiveEffects[i];

                if (effect.Data.Type == EffectType.DamageOverTime)
                {
                    if (TryGetComponent<HealthComponent>(out var health)) health.TakeDamage(effect.Data.Value);
                    Debug.Log($"{gameObject.name} takes {effect.Data.Value} damage from {effect.Data.EffectName}");
                }
                else if (effect.Data.Type == EffectType.HealOverTime)
                {
                    if (TryGetComponent<HealthComponent>(out var health)) health.Heal(effect.Data.Value);
                    Debug.Log($"{gameObject.name} heals {effect.Data.Value} HP from {effect.Data.EffectName}");
                }

                effect.RemainingTurns--;

                if (effect.RemainingTurns <= 0)
                {
                    RemoveEffect(effect);
                }
            }
        }

        private void RemoveEffect(ActiveEffect effect)
        {
            if (effect.Data.Type == EffectType.StatModifier && entity != null && entity.Stats.ContainsKey(effect.Data.TargetStat))
            {
                entity.Stats[effect.Data.TargetStat].RemoveModifier(effect.Data.Value);
            }
            ActiveEffects.Remove(effect);
            Debug.Log($"{effect.Data.EffectName} wore off from {gameObject.name}");
        }
    }
}