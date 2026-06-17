using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    // Menyimpan daftar skill yang dimiliki entitas saat runtime.
    public class SkillComponent : MonoBehaviour
    {
        public List<SkillData> AvailableSkills = new List<SkillData>();

        // Memuat skill dari data statis ke list runtime.
        public void Initialize(List<SkillData> baseSkills)
        {
            AvailableSkills.Clear();
            if (baseSkills != null)
            {
                AvailableSkills.AddRange(baseSkills);
            }
        }
    }
}