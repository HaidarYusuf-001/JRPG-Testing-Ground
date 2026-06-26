using System;
using UnityEngine;

namespace JRPG.Core
{
    // Mengelola data nyawa entitas dan kalkulasi damage.
    public class HealthComponent : MonoBehaviour
    {
        private float maxHealth;
        private float currentHealth;

        public event Action<float, float> OnHealthChanged;

        public void Initialize(float baseMaxHealth)
        {
            maxHealth = baseMaxHealth;
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void UpgradeMaxHealth(float newMaxHealth, bool fullyRestore)
        {
            maxHealth = newMaxHealth;
            if (fullyRestore) currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        // Memaksa pengubahan nilai nyawa saat memuat data persisten.
        public void SetCurrentHealth(float amount)
        {
            currentHealth = Mathf.Clamp(amount, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
    }
}