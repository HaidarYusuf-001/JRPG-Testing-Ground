using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    // Eksekusi skill yang berfokus pada penambahan HP.
    public class HealSkillStrategy : ISkillStrategy
    {
        public void Execute(Entity caster, Entity target, SkillData data)
        {
            if (target.TryGetComponent<HealthComponent>(out var healthComp))
            {
                Debug.Log($"{caster.gameObject.name} uses {data.SkillName}!");
                healthComp.Heal(data.Power);
            }
        }
    }
}