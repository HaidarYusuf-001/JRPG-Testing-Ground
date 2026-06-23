using UnityEngine;
using UnityEngine.UI;
using JRPG.Core;

namespace JRPG.UI
{
    // Mengendalikan antarmuka dan interaksi pada layar menu utama permainan.
    public class MainMenuUI : MonoBehaviour
    {
        public Button NewGameButton;
        public Button LoadGameButton;
        public Button QuitButton;
        public string MapSceneName = "Scene_Map";

        private void OnEnable()
        {
            if (NewGameButton != null) NewGameButton.onClick.AddListener(StartNewGame);
            if (LoadGameButton != null) LoadGameButton.onClick.AddListener(LoadSavedGame);
            if (QuitButton != null) QuitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            if (NewGameButton != null) NewGameButton.onClick.RemoveListener(StartNewGame);
            if (LoadGameButton != null) LoadGameButton.onClick.RemoveListener(LoadSavedGame);
            if (QuitButton != null) QuitButton.onClick.RemoveListener(QuitGame);
        }

        private async void StartNewGame()
        {
            SetButtonsInteractable(false);
            if (PersistentPlayerData.Instance != null)
            {
                PersistentPlayerData.Instance.ResetData();
            }

            if (SceneTransitionManager.Instance != null)
            {
                await SceneTransitionManager.Instance.LoadSceneAsync(MapSceneName);
            }
        }

        private async void LoadSavedGame()
        {
            SetButtonsInteractable(false);
            if (SaveManager.LoadGame())
            {
                if (SceneTransitionManager.Instance != null)
                {
                    await SceneTransitionManager.Instance.LoadSceneAsync(MapSceneName);
                }
            }
            else
            {
                Debug.Log("File save tidak ditemukan.");
                SetButtonsInteractable(true);
            }
        }

        private void QuitGame()
        {
            Debug.Log("Keluar dari permainan.");
            Application.Quit();
        }

        private void SetButtonsInteractable(bool state)
        {
            if (NewGameButton != null) NewGameButton.interactable = state;
            if (LoadGameButton != null) LoadGameButton.interactable = state;
            if (QuitButton != null) QuitButton.interactable = state;
        }
    }
}