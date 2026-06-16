using System.Collections.Generic;
using UnityEngine;

namespace JRPG.Data
{
    // Aset statis untuk konfigurasi awal atribut entitas.
    [CreateAssetMenu(fileName = "NewEntityData", menuName = "JRPG/Data/Entity Data")]
    public class EntityData : ScriptableObject
    {
        public string EntityName;
        public float BaseHP;
        public float BaseMP;
        public float BaseAttack;
        public float BaseDefense;
        public List<SkillData> BaseSkills;
    }
}