using System.Threading.Tasks;
using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    public class BuffSkillStrategy : ISkillStrategy
    {
        public async Task ExecuteAsync(Entity caster, Entity target, SkillData data, CombatManager manager)
        {
            bool hasTimeline = manager.GenericSkillTimeline != null && manager.CombatDirector != null;
            manager.PlayTimeline(manager.GenericSkillTimeline);

            await manager.WaitForImpact(hasTimeline);

            if (data.EffectToApply != null && target.TryGetComponent<StatusEffectComponent>(out var statusComp))
            {
                statusComp.ApplyEffect(data.EffectToApply);
            }

            await manager.WaitForTimelineEnd(hasTimeline);
            manager.CheckCombatEndCondition();
        }
    }
}