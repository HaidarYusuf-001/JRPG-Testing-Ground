using UnityEngine;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    // Mengkalkulasi dan mengeksekusi damage skill dengan menggabungkan base power, attack caster, dan defense target.
    public class DamageSkillStrategy : ISkillStrategy
    {
        public void Execute(Entity caster, Entity target, SkillData data)
        {
            if (target.TryGetComponent<HealthComponent>(out var targetHealth))
            {
                float totalDamage = data.Power;

                if (caster.Stats.ContainsKey(StatType.Attack))
                {
                    totalDamage += caster.Stats[StatType.Attack].Value;
                }

                if (target.Stats.ContainsKey(StatType.Defense))
                {
                    totalDamage = Mathf.Max(1f, totalDamage - target.Stats[StatType.Defense].Value);
                }

                Debug.Log($"{caster.gameObject.name} casts {data.SkillName} for {totalDamage} damage!");
                targetHealth.TakeDamage(totalDamage);
            }
        }
    }
}