using System.Collections.Generic;
using UnityEngine;
using JRPG.Data;

namespace JRPG.Core
{
    public abstract class Entity : MonoBehaviour
    {
        public Dictionary<StatType, Stat> Stats = new Dictionary<StatType, Stat>();

        protected virtual void Awake()
        {
            InitializeStats();
        }

        protected abstract void InitializeStats();
    }
}