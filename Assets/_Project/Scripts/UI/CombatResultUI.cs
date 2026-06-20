using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JRPG.Combat;

namespace JRPG.UI
{
    // Mengatur tampilan panel hasil akhir pertarungan.
    public class CombatResultUI : MonoBehaviour
    {
        public CombatManager Manager;
        public GameObject ResultPanel;
        public TextMeshProUGUI ResultText;
        public Button FinishButton;

        private void OnEnable()
        {
            Manager.OnCombatEnded += ShowResult;
            FinishButton.onClick.AddListener(CloseResult);
        }

        private void OnDisable()
        {
            Manager.OnCombatEnded -= ShowResult;
            FinishButton.onClick.RemoveListener(CloseResult);
        }

        private void Start()
        {
            ResultPanel.SetActive(false);
        }

        private void ShowResult(bool isWin)
        {
            ResultPanel.SetActive(true);
            ResultText.text = isWin ? "VICTORY!" : "GAME OVER";

            // Logika untuk scene transition atau kembali ke map akan dieksekusi dari sini nantinya.
        }

        private void CloseResult()
        {
            ResultPanel.SetActive(false);
            Debug.Log("Returning to map or restarting game...");
        }
    }
}