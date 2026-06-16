using UnityEngine;

namespace JRPG.Data
{
    public enum SkillType { Damage, Heal }

    // Menyimpan atribut data murni dari sebuah skill.
    [CreateAssetMenu(fileName = "NewSkillData", menuName = "JRPG/Data/Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string SkillName;
        public SkillType Type;
        public float Power;
        public float ManaCost;
    }
}