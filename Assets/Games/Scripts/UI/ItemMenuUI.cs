using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemMenuUI : MonoBehaviour
{
    [SerializeField]
    private CharacterInventory characterInventory;
    [SerializeField]
    private ItemBtn itemBtnPrefab;
    [SerializeField]
    private Transform content;

    public List<ItemTabButton> tabButtons;

    public PlayerTurn playerTurn;

    public Dictionary<ItemCategory, ItemTabButton> categoryTabButtons => tabButtons.ToDictionary(tb => tb.category);

    public ItemCategory selectedCategory;

    private Dictionary<ItemCategory, List<InventoryItemData>> categoryItems = new Dictionary<ItemCategory, List<InventoryItemData>>();

    private void Awake()
    {
        categoryItems.Add(ItemCategory.Weapon, characterInventory.weaponInventory);
        categoryItems.Add(ItemCategory.Armor, characterInventory.armorInventory);
        categoryItems.Add(ItemCategory.Ring, characterInventory.ringInventory);
        categoryItems.Add(ItemCategory.Consumable, characterInventory.consumableInventory);
    }

    private void OnEnable()
    {
        OnTabChange();
    }

    private void OnTabChange()
    {
        tabButtons.ForEach(tb => tb.IsActive = false);
        categoryTabButtons[selectedCategory].IsActive = true;

        foreach(Transform cTransform in content)
        {
            Destroy(cTransform.gameObject);
        }

        List<InventoryItemData> items = categoryItems[selectedCategory];
        items.ForEach(i =>
        {
            var btn = Instantiate(itemBtnPrefab, content);
            bool isEquiped = false;
            switch(selectedCategory)
            {
                case ItemCategory.Weapon:
                    isEquiped = characterInventory.Weapon != null && characterInventory.Weapon.ItemID == i.inventoryID;
                    break;
                case ItemCategory.Armor:
                    isEquiped = characterInventory.Armor != null && characterInventory.Armor.ItemID == i.inventoryID;
                    break;
                case ItemCategory.Ring:
                    isEquiped = characterInventory.Ring != null && characterInventory.Ring.ItemID == i.inventoryID;
                    break;
            }

            btn.SetItem(i, isEquiped);
            btn.OnClick = OnItemClick;
        });
    }

    public void OnItemClick(string itemID)
    {
        BaseItem item = ItemRegistry.Instance.itemDictionary[itemID];

        item.Use(characterInventory);
        OnTabChange();
        playerTurn.GoToNextTurn();
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct InventoryItemData
{
    [ValueDropdown("GetInventoryIDs")]
    public string inventoryID;
    public int count;

    public IEnumerable<string> GetInventoryIDs => ItemRegistry.Instance.itemDictionary.Keys;
}
