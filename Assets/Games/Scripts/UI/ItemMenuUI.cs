using Game.Events;
using Game.Logger;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMenuUI : MonoBehaviour
{
    [SerializeField]
    private CharacterInventory characterInventory;
    [SerializeField]
    private ItemBtn itemBtnPrefab;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Transform content;

    public List<ItemTabButton> tabButtons;

    public PlayerTurn playerTurn;

    public System.Action OnClose;

    public Dictionary<ItemCategory, ItemTabButton> categoryTabButtons => tabButtons.ToDictionary(tb => tb.category);

    public ItemCategory selectedCategory;

    private Dictionary<ItemCategory, List<InventoryItemData>> categoryItems = new Dictionary<ItemCategory, List<InventoryItemData>>();

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

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
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        OnClose?.Invoke();
    }

    private void Update()
    {
        int index = 0;
        foreach(var key in categoryItems.Keys)
        {
            if (Input.GetKey(keyCodes[index]))
            {
                ChangeTab(key);
            }
            index++;
        }
    }

    public void ChangeTab(ItemCategory tab)
    {
        selectedCategory = tab;
        OnTabChange();
    }

    private void OnTabChange()
    {
        Log.Print(selectedCategory, FilterLog.Game);

        tabButtons.ForEach(tb => {
            tb.GetComponent<Button>().onClick.AddListener(() =>
            {
                var category = tb.category;
                ChangeTab(category);
            });
            tb.IsActive = false;
        });
        categoryTabButtons[selectedCategory].IsActive = true;

        foreach(Transform cTransform in content)
        {
            Destroy(cTransform.gameObject);
        }

        List<InventoryItemData> items = categoryItems[selectedCategory];
        bool isFirst = true;
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
            if (isFirst)
            {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                isFirst = false;
            }
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
