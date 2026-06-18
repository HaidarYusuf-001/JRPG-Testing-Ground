using System;
using UnityEngine;

namespace JRPG.Core
{
    // Mengelola data progres karakter seperti level, experience, dan mata uang.
    public class PlayerProgressComponent : MonoBehaviour
    {
        public int Level = 1;
        public int CurrentExp = 0;
        public int Gold = 0;

        public event Action<int> OnLevelUp;
        public event Action<int> OnGoldChanged;

        // Menambahkan experience dan memicu level up jika threshold tercapai.
        public void AddExp(int amount)
        {
            CurrentExp += amount;
            Debug.Log($"Gained {amount} EXP. Total: {CurrentExp}");

            int requiredExp = Level * 100;
            if (CurrentExp >= requiredExp)
            {
                CurrentExp -= requiredExp;
                Level++;
                Debug.Log($"Level Up! Now Level {Level}");
                OnLevelUp?.Invoke(Level);
            }
        }

        // Menambahkan mata uang dan memancarkan event perubahan nilai.
        public void AddGold(int amount)
        {
            Gold += amount;
            Debug.Log($"Gained {amount} Gold. Total: {Gold}");
            OnGoldChanged?.Invoke(Gold);
        }
    }
}