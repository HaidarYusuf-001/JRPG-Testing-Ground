using System;
using UnityEngine;

namespace JRPG.Core
{
    // Mengatur logika HP dan memancarkan event terkait.
    public class HealthComponent : MonoBehaviour
    {
        private float currentHealth;
        private float maxHealth;

        public event Action<float, float> OnHealthChanged;
        public event Action OnDied;

        // Inisialisasi nilai HP awal.
        public void Initialize(float maxHP)
        {
            maxHealth = maxHP;
            currentHealth = maxHP;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        // Mengurangi HP dan mengecek kondisi mati.
        public void TakeDamage(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0f)
            {
                OnDied?.Invoke();
                gameObject.SetActive(false);
            }
        }

        // Menambah HP tanpa melebihi batas maksimal.
        public void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        // Mengembalikan nilai HP saat ini.
        public float GetCurrentHealth() => currentHealth;
    }
}