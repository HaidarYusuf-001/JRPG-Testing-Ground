using System;
using UnityEngine;

namespace JRPG.Core
{
    // Mengelola data resource mana entitas.
    public class ManaComponent : MonoBehaviour
    {
        private float maxMana;
        private float currentMana;

        public event Action<float, float> OnManaChanged;

        public void Initialize(float baseMaxMana)
        {
            maxMana = baseMaxMana;
            currentMana = maxMana;
            OnManaChanged?.Invoke(currentMana, maxMana);
        }

        public void UpgradeMaxMana(float newMaxMana, bool fullyRestore)
        {
            maxMana = newMaxMana;
            if (fullyRestore) currentMana = maxMana;
            OnManaChanged?.Invoke(currentMana, maxMana);
        }

        public bool Consume(float amount)
        {
            if (currentMana >= amount)
            {
                currentMana -= amount;
                OnManaChanged?.Invoke(currentMana, maxMana);
                return true;
            }
            return false;
        }

        public void Restore(float amount)
        {
            currentMana = Mathf.Min(maxMana, currentMana + amount);
            OnManaChanged?.Invoke(currentMana, maxMana);
        }

        public float GetCurrentMana() => currentMana;
        public float GetMaxMana() => maxMana;
    }
}