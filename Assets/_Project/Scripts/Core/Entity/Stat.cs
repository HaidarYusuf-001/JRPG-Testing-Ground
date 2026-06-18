using System;
using System.Collections.Generic;

namespace JRPG.Core
{
    [Serializable]
    public class Stat
    {
        public float BaseValue;
        private List<float> modifiers = new List<float>();

        // Mengkalkulasi nilai akhir dengan menjumlahkan base dan semua modifier.
        public float Value
        {
            get
            {
                float finalValue = BaseValue;
                foreach (float mod in modifiers)
                {
                    finalValue += mod;
                }
                return finalValue;
            }
        }

        public void AddModifier(float mod) => modifiers.Add(mod);
        public void RemoveModifier(float mod) => modifiers.Remove(mod);
    }
}
