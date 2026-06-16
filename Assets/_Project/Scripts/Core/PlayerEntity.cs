using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    // Entitas spesifik untuk karakter yang dikendalikan pemain
    public class PlayerEntity : Entity
    {
        public EntityData BaseData;

        // Mengambil base stat dari ScriptableObject ke dalam runtime dictionary
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