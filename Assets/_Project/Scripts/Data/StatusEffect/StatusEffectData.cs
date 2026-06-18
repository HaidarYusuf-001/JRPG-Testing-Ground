using UnityEngine;

namespace JRPG.Data
{
    public enum EffectType { StatModifier, DamageOverTime, HealOverTime }

    [CreateAssetMenu(fileName = "NewStatusEffect", menuName = "JRPG/Data/Status Effect")]
    public class StatusEffectData : ScriptableObject
    {
        public string EffectName;
        public EffectType Type;
        public StatType TargetStat;
        public float Value;
        public int DurationInTurns;
    }
}