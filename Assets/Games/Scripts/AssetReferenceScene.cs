using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceScene : AssetReference
{
    public AssetReferenceScene(string assetPath) : base(assetPath) { }

    public override bool ValidateAsset(Object obj)
    {
#if UNITY_EDITOR
        var type = obj.GetType();
        return typeof(UnityEditor.SceneAsset).IsAssignableFrom(type);
#else
        return false;
#endif
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        var type = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path);
        return typeof(UnityEditor.SceneAsset).IsAssignableFrom(type);
#else
        return false;
#endif
    }
}
