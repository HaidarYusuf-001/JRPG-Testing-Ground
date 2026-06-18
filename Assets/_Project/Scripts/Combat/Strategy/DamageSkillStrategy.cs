using System;
using System.Threading.Tasks;
using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    public class DamageSkillStrategy : ISkillStrategy
    {
        public async Task ExecuteAsync(Entity caster, Entity target, SkillData data, CombatManager manager)
        {
            bool hasTimeline = manager.GenericSkillTimeline != null && manager.CombatDirector != null;
            manager.PlayTimeline(manager.GenericSkillTimeline);

            await manager.WaitForImpact(hasTimeline);

            if (target.TryGetComponent<HealthComponent>(out var targetHealth))
            {
                float totalDamage = data.Power;
                if (caster.Stats.ContainsKey(StatType.Attack)) totalDamage += caster.Stats[StatType.Attack].Value;
                if (target.Stats.ContainsKey(StatType.Defense)) totalDamage = Mathf.Max(1f, totalDamage - target.Stats[StatType.Defense].Value);
                targetHealth.TakeDamage(totalDamage);
            }

            await manager.WaitForTimelineEnd(hasTimeline);
            manager.CheckCombatEndCondition();
        }
    }
}