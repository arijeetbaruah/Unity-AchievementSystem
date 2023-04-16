using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBtnObjectPool : ObjectPool<SpellBtn>
{
    public AssetReferenceSpellButton prefab;
    
    public override void CreateNewInstance(System.Action<SpellBtn> callback)
    {
        prefab.InstantiateAsync().Completed += handler =>
        {
            callback?.Invoke(handler.Result.GetComponent<SpellBtn>());
        };
    }
}
