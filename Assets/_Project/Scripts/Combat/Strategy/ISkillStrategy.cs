using System.Threading.Tasks;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.Combat
{
    // Interface strategy yang mendukung eksekusi asynchronous menggunakan Task.
    public interface ISkillStrategy
    {
        Task ExecuteAsync(Entity caster, Entity target, SkillData data, CombatManager manager);
    }
}