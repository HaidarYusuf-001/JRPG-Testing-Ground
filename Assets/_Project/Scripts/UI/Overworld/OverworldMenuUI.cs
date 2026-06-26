using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JRPG.Core;
using JRPG.Exploration;

namespace JRPG.UI
{
    // Mengatur tampilan menu pause utama dan fungsionalitas save/load game.
    public class OverworldMenuUI : MonoBehaviour
    {
        public GameObject MenuPanel;

        [Header("Status Texts")]
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI ExpText;
        public TextMeshProUGUI GoldText;
        public TextMeshProUGUI HPText;
        public TextMeshProUGUI MPText;

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

        public void UpdateUI()
        {
            if (PersistentPlayerData.Instance != null)
            {
                LevelText.text = $"Level: {PersistentPlayerData.Instance.Level}";
                ExpText.text = $"EXP: {PersistentPlayerData.Instance.CurrentExp}";
                GoldText.text = $"Gold: {PersistentPlayerData.Instance.Gold}";

                PlayerEntity player = Object.FindAnyObjectByType<PlayerEntity>();
                if (player != null && player.TryGetComponent<HealthComponent>(out var hp) && player.TryGetComponent<ManaComponent>(out var mp))
                {
                    HPText.text = $"HP: {hp.GetCurrentHealth()}/{hp.GetMaxHealth()}";
                    MPText.text = $"MP: {mp.GetCurrentMana()}/{mp.GetMaxMana()}";
                }
                else
                {
                    HPText.text = $"HP: {PersistentPlayerData.Instance.CurrentHP}";
                    MPText.text = $"MP: {PersistentPlayerData.Instance.CurrentMP}";
                }
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