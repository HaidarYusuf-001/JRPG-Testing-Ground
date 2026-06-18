using UnityEngine;

namespace JRPG.Data
{
    public enum SkillType { Damage, Heal, Buff }

    [CreateAssetMenu(fileName = "NewSkillData", menuName = "JRPG/Data/Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string SkillName;
        public SkillType Type;
        public float Power;
        public float ManaCost;
        public StatusEffectData EffectToApply; // Efek opsional yang diterapkan oleh skill ini
    }
}