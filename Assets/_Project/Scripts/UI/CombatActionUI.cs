using UnityEngine;
using UnityEngine.UI;
using JRPG.Combat;

namespace JRPG.UI
{
    public class CombatActionUI : MonoBehaviour
    {
        public CombatManager Manager;
        public Button AttackButton;
        public GameObject ActionPanel;

        // Subscribe event dari CombatManager dan daftarkan aksi tombol
        private void OnEnable()
        {
            Manager.OnPlayerTurnStarted += ShowPanel;
            Manager.OnPlayerTurnEnded += HidePanel;
            AttackButton.onClick.AddListener(Manager.OnAttackButtonClicked);
            HidePanel();
        }

        // Unsubscribe event saat tidak digunakan
        private void OnDisable()
        {
            Manager.OnPlayerTurnStarted -= ShowPanel;
            Manager.OnPlayerTurnEnded -= HidePanel;
            AttackButton.onClick.RemoveListener(Manager.OnAttackButtonClicked);
        }

        // Menampilkan panel tombol aksi
        private void ShowPanel()
        {
            ActionPanel.SetActive(true);
        }

        // Menyembunyikan panel tombol aksi
        private void HidePanel()
        {
            ActionPanel.SetActive(false);
        }
    }
}