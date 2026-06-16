using System.Collections.Generic;
using UnityEngine;

namespace JRPG.Core
{
    // Wadah identitas dasar dan data stat murni untuk semua entitas.
    public abstract class Entity : MonoBehaviour
    {
        public Dictionary<string, Stat> Stats = new Dictionary<string, Stat>();

        protected virtual void Awake()
        {
            InitializeStats();
        }

        // Diimplementasikan oleh child class untuk memuat data.
        protected abstract void InitializeStats();
    }
}