using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JRPG.Combat;
using JRPG.Core;

namespace JRPG.UI
{
    // Mengatur tampilan panel hasil akhir pertarungan dan memicu transisi scene.
    public class CombatResultUI : MonoBehaviour
    {
        public CombatManager Manager;
        public GameObject ResultPanel;
        public TextMeshProUGUI ResultText;
        public Button FinishButton;
        public string MapSceneName = "Scene_Map";

        private void OnEnable()
        {
            Manager.OnCombatEnded += ShowResult;
            FinishButton.onClick.AddListener(OnFinishButtonClicked);
        }

        private void OnDisable()
        {
            Manager.OnCombatEnded -= ShowResult;
            FinishButton.onClick.RemoveListener(OnFinishButtonClicked);
        }

        private void Start()
        {
            ResultPanel.SetActive(false);
        }

        private void ShowResult(bool isWin)
        {
            ResultPanel.SetActive(true);
            ResultText.text = isWin ? "VICTORY!" : "GAME OVER";
        }

        private async void OnFinishButtonClicked()
        {
            FinishButton.interactable = false;

            if (SceneTransitionManager.Instance != null)
            {
                await SceneTransitionManager.Instance.LoadSceneAsync(MapSceneName);
            }
            else
            {
                Debug.LogWarning("SceneTransitionManager tidak ditemukan di scene! Pastikan ada prefab manager yang aktif.");
                FinishButton.interactable = true;
            }
        }
    }
}