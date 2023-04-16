using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    Stats GetStatsBonus();
}

public interface IArmor
{
    Stats GetStatsBonus();
}

public interface IRing
{
    Stats GetStatsBonus();
}

public abstract class BaseItem : MonoBehaviour
{
    public abstract string ItemID { get; }
    public abstract string ItemName { get; }
    public abstract ItemCategory Category { get; }

    public abstract void Use(CharacterInventory characterInventory);
}

public enum ItemCategory
{
    Weapon,
    Armor,
    Ring,
    Consumable,
    Key
}

public interface IEquipable
{
    EquipSlot equipSlot { get; }
}

public enum EquipSlot
{
    Weapon,
    Armor,
    Ring
}

public interface IConsumable
{
    int GetCount();
    void AddItem(int amount);
    void UseItem();
}
