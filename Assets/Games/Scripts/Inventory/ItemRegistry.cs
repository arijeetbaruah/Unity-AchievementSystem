using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[GlobalConfig("Item Registry"), CreateAssetMenu]
public class ItemRegistry : GlobalConfig<ItemRegistry>
{
    [ShowInInspector, SerializeReference]
    public List<BaseItem> items = new List<BaseItem>();

    [ShowInInspector, ReadOnly]
    public SerializedItemDictionary itemDictionary => new SerializedItemDictionary(items
        .Where(i => i != null)
        .ToDictionary(i => i.ItemID));
}

[System.Serializable]
public class SerializedItemDictionary : UnitySerializedDictionary<string, BaseItem>
{
    public SerializedItemDictionary(Dictionary<string, BaseItem> dict) : base(dict)
    {

    }
}
