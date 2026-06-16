using UnityEngine;
using UnityEngine.UI;
using JRPG.Core;

namespace JRPG.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        public Entity TargetEntity;
        public Slider HealthSlider;

        // Mendaftarkan fungsi ke event saat script aktif
        private void OnEnable()
        {
            if (TargetEntity != null) TargetEntity.OnHealthChanged += UpdateHealthBar;
        }

        // Mencabut fungsi dari event saat script nonaktif mencegah memory leak
        private void OnDisable()
        {
            if (TargetEntity != null) TargetEntity.OnHealthChanged -= UpdateHealthBar;
        }

        // Memperbarui visual slider sesuai nilai HP
        private void UpdateHealthBar(float currentHP, float maxHP)
        {
            if (HealthSlider.maxValue != maxHP) HealthSlider.maxValue = maxHP;
            HealthSlider.value = currentHP;
        }
    }
}