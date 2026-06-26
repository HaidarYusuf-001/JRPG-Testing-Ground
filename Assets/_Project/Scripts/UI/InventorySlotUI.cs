using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JRPG.Data;
using System;

namespace JRPG.UI
{
    // Mengontrol tampilan informasi satu baris item di UI inventory.
    public class InventorySlotUI : MonoBehaviour
    {
        public TextMeshProUGUI ItemNameText;
        public TextMeshProUGUI QuantityText;
        public Button ActionButton;
        public TextMeshProUGUI ActionButtonText;

        public void Setup(ItemSlot slot, Action<ItemData> onActionCallback)
        {
            ItemNameText.text = slot.Item.ItemName;
            QuantityText.text = $"x{slot.Quantity}";

            if (slot.Item is EquipmentData) ActionButtonText.text = "Equip";
            else ActionButtonText.text = "Use";

            ActionButton.onClick.RemoveAllListeners();
            ActionButton.onClick.AddListener(() => onActionCallback(slot.Item));
        }
    }
}