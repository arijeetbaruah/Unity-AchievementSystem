using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBtn : MonoBehaviour
{
    private BaseItem item;
    private InventoryItemData inventoryItemData;

    [SerializeField]
    private TextMeshProUGUI txt;

    public Action<string> OnClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClick?.Invoke(item.ItemID);
        });
    }

    public void SetItem(InventoryItemData itemData, bool isEquiped = false)
    {
        inventoryItemData = itemData;
        item = ItemRegistry.Instance.itemDictionary[itemData.inventoryID];

        txt.SetText($"{item.ItemName}{(inventoryItemData.count > 1 ? $" ({inventoryItemData.count})" : "")}{(isEquiped ? " - E" : "")}");
    }
}
