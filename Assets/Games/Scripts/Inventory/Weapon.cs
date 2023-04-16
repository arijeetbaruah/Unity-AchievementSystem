using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : BaseItem, IWeapon, IEquipable
{
    public string itemID;
    public string itemName;

    public Stats statsBonus;
    [MinValue(1)]
    public float baseDamage = 1;

    public override string ItemID => itemID;

    public override string ItemName => itemName;

    public override ItemCategory Category => ItemCategory.Weapon;

    public EquipSlot equipSlot => EquipSlot.Weapon;

    public Stats GetStatsBonus()
    {
        return statsBonus;
    }

    public override void Use(CharacterInventory characterInventory)
    {
        characterInventory.Weapon = this;
    }
}
