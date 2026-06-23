using UnityEngine;
using TMPro;
using JRPG.Core;

namespace JRPG.UI
{
    // Mengatur tampilan menu pause di map untuk melihat status pemain.
    public class OverworldMenuUI : MonoBehaviour
    {
        public GameObject MenuPanel;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI ExpText;
        public TextMeshProUGUI GoldText;

        private void Start()
        {
            MenuPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        private void ToggleMenu()
        {
            bool isActive = !MenuPanel.activeSelf;
            MenuPanel.SetActive(isActive);

            if (isActive)
            {
                UpdateUI();
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void UpdateUI()
        {
            if (PersistentPlayerData.Instance != null)
            {
                LevelText.text = $"Level: {PersistentPlayerData.Instance.Level}";
                ExpText.text = $"EXP: {PersistentPlayerData.Instance.CurrentExp}";
                GoldText.text = $"Gold: {PersistentPlayerData.Instance.Gold}";
            }
        }
    }
}