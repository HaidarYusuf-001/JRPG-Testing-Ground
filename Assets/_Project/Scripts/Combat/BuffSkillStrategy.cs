using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    // Menerapkan efek status ke target sesuai data dari skill.
    public class BuffSkillStrategy : ISkillStrategy
    {
        public void Execute(Entity caster, Entity target, SkillData data)
        {
            Debug.Log($"{caster.gameObject.name} casts {data.SkillName}!");

            if (data.EffectToApply != null && target.TryGetComponent<StatusEffectComponent>(out var statusComp))
            {
                statusComp.ApplyEffect(data.EffectToApply);
                Debug.Log($"Applied {data.EffectToApply.EffectName} to {target.gameObject.name}");
            }
            else
            {
                Debug.Log("Failed to apply effect. Target might not have StatusEffectComponent or Effect data is null.");
            }
        }
    }
}