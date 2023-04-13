using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[System.Serializable]
public class AssetReferenceButton : AssetReference
{
    public AssetReferenceButton(string name) : base(name)
    {
    }

    public override bool ValidateAsset(Object obj)
    {
        var type = obj.GetType();
        if (!typeof(Button).IsAssignableFrom(type))
        {
            return false;
        }

        Button button = ((GameObject)obj).GetComponent<Button>();

        return button != null;
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        Button button = UnityEditor.AssetDatabase.LoadAssetAtPath<Button>(path);
        return button != null;
#else
        return false;
#endif
    }
}
