using UnityEngine;
using UnityEngine.UI;
using JRPG.Core;

namespace JRPG.UI
{
    // Observer untuk memperbarui UI berdasarkan event dari HealthComponent.
    public class HealthBarUI : MonoBehaviour
    {
        public HealthComponent TargetHealth;
        public Slider HealthSlider;

        private void OnEnable()
        {
            if (TargetHealth != null) TargetHealth.OnHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            if (TargetHealth != null) TargetHealth.OnHealthChanged -= UpdateHealthBar;
        }

        private void UpdateHealthBar(float currentHP, float maxHP)
        {
            if (HealthSlider.maxValue != maxHP) HealthSlider.maxValue = maxHP;
            HealthSlider.value = currentHP;
        }
    }
}