using System.Collections.Generic;
using UnityEngine;

namespace JRPG.Data
{
    // Aset statis untuk konfigurasi awal atribut dan kepemilikan entitas.
    [CreateAssetMenu(fileName = "NewEntityData", menuName = "JRPG/Data/Entity Data")]
    public class EntityData : ScriptableObject
    {
        public string EntityName;
        public float BaseHP;
        public float BaseMP;
        public float BaseAttack;
        public float BaseDefense;
        public List<SkillData> BaseSkills;
        public List<ItemSlot> StartingItems;

        [Header("Rewards (For Enemies)")]
        public int ExpReward;
        public int GoldReward;
        public List<LootDrop> LootDrops;
    }
}