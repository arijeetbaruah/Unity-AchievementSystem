using TMPro;
using UnityEngine.AddressableAssets;

public static class Constants
{
    public const float DmgConst = 100f;
}

[System.Serializable]
public class TMPAssetReference : AssetReferenceT<TextMeshProUGUI>
{
    public TMPAssetReference(string guid) : base(guid)
    {
    }
}
