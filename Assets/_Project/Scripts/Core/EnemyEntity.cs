using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    // Mengatur inisialisasi data untuk karakter musuh.
    public class EnemyEntity : Entity
    {
        public EntityData BaseData;

        // Memuat base stat dan menginisialisasi komponen jika tersedia.
        protected override void InitializeStats()
        {
            if (BaseData == null) return;

            Stats["Attack"] = new Stat { BaseValue = BaseData.BaseAttack, CurrentValue = BaseData.BaseAttack };
            Stats["Defense"] = new Stat { BaseValue = BaseData.BaseDefense, CurrentValue = BaseData.BaseDefense };

            if (TryGetComponent<HealthComponent>(out var healthComp))
            {
                healthComp.Initialize(BaseData.BaseHP);
            }

            if (TryGetComponent<ManaComponent>(out var manaComp))
            {
                manaComp.Initialize(BaseData.BaseMP);
            }
        }
    }
}