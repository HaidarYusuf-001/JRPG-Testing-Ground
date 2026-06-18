using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JRPG.Combat;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.UI
{
    public class CombatActionUI : MonoBehaviour
    {
        public CombatManager Manager;
        public Button AttackButton;
        public GameObject ActionPanel;

        [Header("Skills Setup")]
        public GameObject SkillButtonPrefab;
        public Transform SkillPanel;

        [Header("Items Setup")]
        public GameObject ItemButtonPrefab;
        public Transform ItemPanel;

        private void OnEnable()
        {
            Manager.OnPlayerTurnStarted += ShowPanel;
            Manager.OnPlayerTurnEnded += HidePanel;
            AttackButton.onClick.AddListener(Manager.OnAttackButtonClicked);
            HidePanel();
        }

        private void OnDisable()
        {
            Manager.OnPlayerTurnStarted -= ShowPanel;
            Manager.OnPlayerTurnEnded -= HidePanel;
            AttackButton.onClick.RemoveListener(Manager.OnAttackButtonClicked);
        }

        private void Start()
        {
            GenerateSkillButtons();
        }

        private void GenerateSkillButtons()
        {
            if (Manager.Player.TryGetComponent<SkillComponent>(out var skillComp))
            {
                foreach (SkillData skill in skillComp.AvailableSkills)
                {
                    GameObject btnObj = Instantiate(SkillButtonPrefab, SkillPanel);
                    btnObj.GetComponentInChildren<TextMeshProUGUI>().text = $"{skill.SkillName} ({skill.ManaCost} MP)";

                    btnObj.GetComponent<Button>().onClick.AddListener(() => Manager.OnSkillButtonClicked(skill));
                }
            }
        }

        // Menghapus tombol lama dan membuat ulang berdasarkan data inventory terbaru.
        private void RefreshItemButtons()
        {
            foreach (Transform child in ItemPanel)
            {
                Destroy(child.gameObject);
            }

            if (Manager.Player.TryGetComponent<InventoryComponent>(out var invComp))
            {
                foreach (ItemSlot slot in invComp.Slots)
                {
                    if (slot.Item.Type == ItemType.Consumable)
                    {
                        GameObject btnObj = Instantiate(ItemButtonPrefab, ItemPanel);
                        btnObj.GetComponentInChildren<TextMeshProUGUI>().text = $"{slot.Item.ItemName} (x{slot.Quantity})";

                        // Menangkap reference item ke variabel lokal untuk lambda expression.
                        ItemData currentItem = slot.Item;
                        btnObj.GetComponent<Button>().onClick.AddListener(() => Manager.OnItemButtonClicked(currentItem));
                    }
                }
            }
        }

        private void ShowPanel()
        {
            ActionPanel.SetActive(true);
            RefreshItemButtons();
        }

        private void HidePanel()
        {
            ActionPanel.SetActive(false);
        }
    }
}