using System;
using System.Collections.Generic;
using UnityEngine;

namespace JRPG.Core
{
    public abstract class Entity : MonoBehaviour
    {
        public Dictionary<string, Stat> Stats = new Dictionary<string, Stat>();

        public event Action<float, float> OnHealthChanged;
        public event Action<Entity> OnEntityDied;

        protected virtual void Awake()
        {
            InitializeStats();
        }

        protected abstract void InitializeStats();

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

        public virtual void DealDamage(Entity target)
        {
            if (!Stats.ContainsKey("Attack")) return;

            float damage = Stats["Attack"].CurrentValue;

            if (target.Stats.ContainsKey("Defense"))
            {
                damage = Mathf.Max(1f, damage - target.Stats["Defense"].CurrentValue);
            }

            Debug.Log($"{gameObject.name} attacks {target.gameObject.name} for {damage} damage!");
            target.TakeDamage(damage);
        }

        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            OnEntityDied?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}