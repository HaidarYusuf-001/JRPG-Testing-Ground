using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JRPG.Core;
using JRPG.Data;

namespace JRPG.UI
{
    // Mengatur interaksi visual inventory dan equipment di luar pertarungan.
    public class OverworldInventoryUI : MonoBehaviour
    {
        [Header("References")]
        public Transform InventoryContentPanel;
        public GameObject InventorySlotPrefab;
        public OverworldMenuUI MenuUIController;

        [Header("Equipped UI")]
        public TextMeshProUGUI WeaponText;
        public TextMeshProUGUI ArmorText;
        public Button UnequipWeaponButton;
        public Button UnequipArmorButton;

        private PlayerEntity player;

        private void OnEnable()
        {
            player = FindAnyObjectByType<PlayerEntity>();
            if (MenuUIController == null) MenuUIController = GetComponent<OverworldMenuUI>();

            if (UnequipWeaponButton != null) UnequipWeaponButton.onClick.AddListener(() => OnUnequipButtonClicked(EquipmentSlot.Weapon));
            if (UnequipArmorButton != null) UnequipArmorButton.onClick.AddListener(() => OnUnequipButtonClicked(EquipmentSlot.Armor));

            RefreshUI();
        }

        private void OnDisable()
        {
            if (UnequipWeaponButton != null) UnequipWeaponButton.onClick.RemoveAllListeners();
            if (UnequipArmorButton != null) UnequipArmorButton.onClick.RemoveAllListeners();
        }

        public void OnEquipButtonClicked(EquipmentData equipment)
        {
            if (player != null)
            {
                player.EquipItem(equipment);
                RefreshUI();
            }
        }

        public void OnUnequipButtonClicked(EquipmentSlot slotType)
        {
            if (player != null)
            {
                player.UnequipItem(slotType);
                RefreshUI();
            }
        }

        private void HandleItemAction(ItemData item)
        {
            if (item is EquipmentData equipment)
            {
                OnEquipButtonClicked(equipment);
            }
            else if (item.Type == ItemType.Consumable)
            {
                if (player != null)
                {
                    player.UseConsumableItem(item);
                    RefreshUI();
                    if (MenuUIController != null) MenuUIController.UpdateUI();
                }
            }
        }

        public void RefreshUI()
        {
            if (PersistentPlayerData.Instance == null) return;

            foreach (Transform child in InventoryContentPanel) Destroy(child.gameObject);

            foreach (var slot in PersistentPlayerData.Instance.SavedInventory)
            {
                GameObject newSlot = Instantiate(InventorySlotPrefab, InventoryContentPanel);
                if (newSlot.TryGetComponent<InventorySlotUI>(out var slotUI))
                {
                    slotUI.Setup(slot, HandleItemAction);
                }
            }

            UpdateEquippedDisplay(EquipmentSlot.Weapon, WeaponText, UnequipWeaponButton);
            UpdateEquippedDisplay(EquipmentSlot.Armor, ArmorText, UnequipArmorButton);
        }

        private void UpdateEquippedDisplay(EquipmentSlot slotType, TextMeshProUGUI textComp, Button unequipBtn)
        {
            if (textComp == null || unequipBtn == null) return;

            if (PersistentPlayerData.Instance.SavedEquipment.TryGetValue(slotType, out EquipmentData equippedItem))
            {
                textComp.text = $"{slotType}: {equippedItem.ItemName}";
                unequipBtn.interactable = true;
            }
            else
            {
                textComp.text = $"{slotType}: None";
                unequipBtn.interactable = false;
            }
        }
    }
}