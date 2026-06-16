using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    // Eksekusi skill yang berfokus pada pengurangan HP.
    public class DamageSkillStrategy : ISkillStrategy
    {
        public void Execute(Entity caster, Entity target, SkillData data)
        {
            if (target.TryGetComponent<HealthComponent>(out var healthComp))
            {
                Debug.Log($"{caster.gameObject.name} casts {data.SkillName}!");
                healthComp.TakeDamage(data.Power);
            }
        }
    }
}