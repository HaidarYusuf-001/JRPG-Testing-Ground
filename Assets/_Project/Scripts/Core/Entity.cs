using System;
using System.Collections.Generic;
using UnityEngine;

namespace JRPG.Core
{
    // Class dasar untuk semua karakter
    public abstract class Entity : MonoBehaviour
    {
        public Dictionary<string, Stat> Stats = new Dictionary<string, Stat>();

        public event Action<float, float> OnHealthChanged;
        public event Action<Entity> OnEntityDied;

        protected virtual void Awake()
        {
            InitializeStats();
        }

        // Diimplementasikan oleh class turunan untuk inisialisasi stat
        protected abstract void InitializeStats();

        // Mengkalkulasi pengurangan HP dari damage
        public virtual void TakeDamage(float amount)
        {
            if (!Stats.ContainsKey("HP")) return;

            Stats["HP"].CurrentValue = Mathf.Clamp(Stats["HP"].CurrentValue - amount, 0f, Stats["HP"].BaseValue);
            OnHealthChanged?.Invoke(Stats["HP"].CurrentValue, Stats["HP"].BaseValue);

            if (Stats["HP"].CurrentValue <= 0f)
            {
                Die();
            }
        }

        // Menonaktifkan entitas saat HP habis
        protected virtual void Die()
        {
            OnEntityDied?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}