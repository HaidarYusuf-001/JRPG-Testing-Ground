using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    // Interface untuk pola eksekusi skill.
    public interface ISkillStrategy
    {
        void Execute(Entity caster, Entity target, SkillData data);
    }
}