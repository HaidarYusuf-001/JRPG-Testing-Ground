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

        // Membuat tombol secara dinamis berdasarkan data skill karakter.
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

        private void ShowPanel() => ActionPanel.SetActive(true);
        private void HidePanel() => ActionPanel.SetActive(false);
    }
}