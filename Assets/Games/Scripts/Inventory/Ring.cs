using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : BaseItem, IRing, IEquipable
{
    public string itemID;
    public string itemName;

    public Stats statsBonus;

    public override string ItemID => itemID;

    public override string ItemName => itemName;

    public override ItemCategory Category => ItemCategory.Armor;

    public EquipSlot equipSlot => EquipSlot.Armor;

    public Stats GetStatsBonus()
    {
        return statsBonus;
    }

    public override void Use(CharacterInventory characterInventory)
    {
        characterInventory.Ring = this;
    }
}
