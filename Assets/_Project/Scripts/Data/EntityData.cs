using UnityEngine;

namespace JRPG.Data
{
    // Aset data statis untuk menyimpan base stat karakter dan musuh
    [CreateAssetMenu(fileName = "NewEntityData", menuName = "JRPG/Data/Entity Data")]
    public class EntityData : ScriptableObject
    {
        public string EntityName;
        public float BaseHP;
        public float BaseMP;
        public float BaseAttack;
        public float BaseDefense;
    }
}