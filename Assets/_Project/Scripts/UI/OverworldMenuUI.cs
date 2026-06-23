using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JRPG.Core;
using JRPG.Exploration;

namespace JRPG.UI
{
    // Mengatur tampilan menu pause dan fungsionalitas save/load game.
    public class OverworldMenuUI : MonoBehaviour
    {
        public GameObject MenuPanel;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI ExpText;
        public TextMeshProUGUI GoldText;

        [Header("Buttons")]
        public Button SaveButton;
        public Button LoadButton;

        private void OnEnable()
        {
            if (SaveButton != null) SaveButton.onClick.AddListener(OnSaveClicked);
            if (LoadButton != null) LoadButton.onClick.AddListener(OnLoadClicked);
        }

        private void OnDisable()
        {
            if (SaveButton != null) SaveButton.onClick.RemoveListener(OnSaveClicked);
            if (LoadButton != null) LoadButton.onClick.RemoveListener(OnLoadClicked);
        }

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

        private void OnSaveClicked()
        {
            if (PersistentPlayerData.Instance != null)
            {
                var player = Object.FindAnyObjectByType<OverworldPlayerController>();
                if (player != null)
                {
                    PersistentPlayerData.Instance.LastMapPosition = player.transform.position;
                    PersistentPlayerData.Instance.HasSavedPosition = true;
                }
            }

            SaveManager.SaveGame();
        }

        private async void OnLoadClicked()
        {
            if (SaveManager.LoadGame())
            {
                Time.timeScale = 1f;
                if (SceneTransitionManager.Instance != null)
                {
                    await SceneTransitionManager.Instance.LoadSceneAsync(gameObject.scene.name);
                }
            }
        }
    }
}