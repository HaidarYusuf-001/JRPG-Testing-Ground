using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    // Entitas spesifik untuk musuh yang dikendalikan AI
    public class EnemyEntity : Entity
    {
        public EntityData BaseData;

        // Inisialisasi stat dasar musuh
        protected override void InitializeStats()
        {
            if (BaseData == null) return;

            Stats["HP"] = new Stat { BaseValue = BaseData.BaseHP, CurrentValue = BaseData.BaseHP };
            Stats["MP"] = new Stat { BaseValue = BaseData.BaseMP, CurrentValue = BaseData.BaseMP };
            Stats["Attack"] = new Stat { BaseValue = BaseData.BaseAttack, CurrentValue = BaseData.BaseAttack };
            Stats["Defense"] = new Stat { BaseValue = BaseData.BaseDefense, CurrentValue = BaseData.BaseDefense };
        }
    }
}