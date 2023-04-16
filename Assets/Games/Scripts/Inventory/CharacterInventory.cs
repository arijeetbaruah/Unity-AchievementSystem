using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private Armor armor;
    [SerializeField]
    private Ring ring;

    public List<InventoryItemData> inventoryItems;

    public List<InventoryItemData> weaponInventory => inventoryItems
        .Where(i => ItemRegistry.Instance.itemDictionary[i.inventoryID].Category == ItemCategory.Weapon)
        .ToList();
    public List<InventoryItemData> armorInventory => inventoryItems
        .Where(i => ItemRegistry.Instance.itemDictionary[i.inventoryID].Category == ItemCategory.Armor)
        .ToList();
    public List<InventoryItemData> ringInventory => inventoryItems
        .Where(i => ItemRegistry.Instance.itemDictionary[i.inventoryID].Category == ItemCategory.Ring)
        .ToList();
    public List<InventoryItemData> consumableInventory => inventoryItems
        .Where(i => ItemRegistry.Instance.itemDictionary[i.inventoryID].Category == ItemCategory.Consumable)
        .ToList();

    public Weapon Weapon { get => weapon; set => weapon = value; }
    public Armor Armor { get => armor; set => armor = value; }
    public Ring Ring { get => ring; set => ring = value; }

    public Stats WeaponStats => weapon == null ? new Stats() : weapon.statsBonus;
    public Stats ArmorStats => armor == null ? new Stats() : armor.statsBonus;
    public Stats RingStats => ring == null ? new Stats() : ring.statsBonus;

    public Stats GetBonus => WeaponStats + ArmorStats + RingStats;
}
