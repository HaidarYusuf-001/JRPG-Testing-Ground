using System.Threading.Tasks;
using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    public class HealSkillStrategy : ISkillStrategy
    {
        public async Task ExecuteAsync(Entity caster, Entity target, SkillData data, CombatManager manager)
        {
            bool hasTimeline = manager.GenericSkillTimeline != null && manager.CombatDirector != null;
            manager.PlayTimeline(manager.GenericSkillTimeline);

            await manager.WaitForImpact(hasTimeline);

            if (target.TryGetComponent<HealthComponent>(out var healthComp))
            {
                healthComp.Heal(data.Power);
            }

            await manager.WaitForTimelineEnd(hasTimeline);
            manager.CheckCombatEndCondition();
        }
    }
}