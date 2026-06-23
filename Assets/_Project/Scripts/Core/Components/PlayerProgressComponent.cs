using System;
using UnityEngine;

namespace JRPG.Core
{
    // Mengelola data progres karakter dan mensinkronisasikannya dengan data persisten.
    public class PlayerProgressComponent : MonoBehaviour
    {
        public int Level = 1;
        public int CurrentExp = 0;
        public int Gold = 0;

        public event Action<int> OnLevelUp;
        public event Action<int> OnGoldChanged;

        private void Start()
        {
            if (PersistentPlayerData.Instance != null)
            {
                Level = PersistentPlayerData.Instance.Level;
                CurrentExp = PersistentPlayerData.Instance.CurrentExp;
                Gold = PersistentPlayerData.Instance.Gold;
            }
        }

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

            if (PersistentPlayerData.Instance != null)
            {
                PersistentPlayerData.Instance.CurrentExp = CurrentExp;
                PersistentPlayerData.Instance.Level = Level;
            }
        }

        public void AddGold(int amount)
        {
            Gold += amount;
            Debug.Log($"Gained {amount} Gold. Total: {Gold}");
            OnGoldChanged?.Invoke(Gold);

            if (PersistentPlayerData.Instance != null)
            {
                PersistentPlayerData.Instance.Gold = Gold;
            }
        }
    }
}