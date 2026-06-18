using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    public class PlayerEntity : Entity
    {
        public EntityData BaseData;

        protected override void InitializeStats()
        {
            if (BaseData == null) return;

            Stats[StatType.Attack] = new Stat { BaseValue = BaseData.BaseAttack };
            Stats[StatType.Defense] = new Stat { BaseValue = BaseData.BaseDefense };

            if (TryGetComponent<HealthComponent>(out var healthComp)) healthComp.Initialize(BaseData.BaseHP);
            if (TryGetComponent<ManaComponent>(out var manaComp)) manaComp.Initialize(BaseData.BaseMP);
            if (TryGetComponent<SkillComponent>(out var skillComp)) skillComp.Initialize(BaseData.BaseSkills);
            if (TryGetComponent<InventoryComponent>(out var invComp)) invComp.Initialize(BaseData.StartingItems);
        }
    }
}