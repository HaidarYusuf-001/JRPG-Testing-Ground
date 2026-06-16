using System;
using UnityEngine;

namespace JRPG.Core
{
    // Mengatur logika MP dan memancarkan event terkait.
    public class ManaComponent : MonoBehaviour
    {
        private float currentMana;
        private float maxMana;

        public event Action<float, float> OnManaChanged;

        // Inisialisasi nilai MP awal.
        public void Initialize(float maxMP)
        {
            maxMana = maxMP;
            currentMana = maxMP;
            OnManaChanged?.Invoke(currentMana, maxMana);
        }

        // Mengurangi MP dan mengembalikan status validasi konsumsi.
        public bool Consume(float amount)
        {
            if (currentMana < amount) return false;

            currentMana -= amount;
            OnManaChanged?.Invoke(currentMana, maxMana);
            return true;
        }

        // Menambah MP tanpa melebihi batas maksimal.
        public void Restore(float amount)
        {
            currentMana = Mathf.Clamp(currentMana + amount, 0f, maxMana);
            OnManaChanged?.Invoke(currentMana, maxMana);
        }
    }
}