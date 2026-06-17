using UnityEngine;
using UnityEngine.UI;
using JRPG.Core;

namespace JRPG.UI
{
    // Observer untuk memperbarui slider MP atau menyembunyikannya jika tidak relevan.
    public class ManaBarUI : MonoBehaviour
    {
        public ManaComponent TargetMana;
        public Slider ManaSlider;
        public GameObject UIParent;

        private void OnEnable()
        {
            if (TargetMana != null)
            {
                TargetMana.OnManaChanged += UpdateManaBar;
            }
            else
            {
                if (UIParent != null) UIParent.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (TargetMana != null) TargetMana.OnManaChanged -= UpdateManaBar;
        }

        private void UpdateManaBar(float currentMP, float maxMP)
        {
            if (ManaSlider.maxValue != maxMP) ManaSlider.maxValue = maxMP;
            ManaSlider.value = currentMP;
        }
    }
}