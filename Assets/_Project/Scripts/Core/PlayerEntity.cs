using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    public class PlayerEntity : Entity
    {
        public EntityData BaseData;

        // Memuat base stat dan menginisialisasi semua komponen yang menempel.
        protected override void InitializeStats()
        {
            if (BaseData == null) return;

            Stats["Attack"] = new Stat { BaseValue = BaseData.BaseAttack, CurrentValue = BaseData.BaseAttack };
            Stats["Defense"] = new Stat { BaseValue = BaseData.BaseDefense, CurrentValue = BaseData.BaseDefense };

            if (TryGetComponent<HealthComponent>(out var healthComp)) healthComp.Initialize(BaseData.BaseHP);
            if (TryGetComponent<ManaComponent>(out var manaComp)) manaComp.Initialize(BaseData.BaseMP);
            if (TryGetComponent<SkillComponent>(out var skillComp)) skillComp.Initialize(BaseData.BaseSkills);
        }
    }
}