using System.Collections.Generic;
using UnityEngine;

using Sirenix.Utilities;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;
using System.Linq;

[GlobalConfig, CreateAssetMenu]
public class CharacterDatabase : GlobalConfig<CharacterDatabase> 
{
    public List<CharacterDetailReferenceListElement> characters;

    [ReadOnly, ShowInInspector]
    public Dictionary<string, AssetReferenceCharacterDetails> references => 
        characters.ToDictionary(e => e.key, e => e.value);

    [System.Serializable]
    public struct CharacterDetailReferenceListElement
    {
        public string key;
        public AssetReferenceCharacterDetails value;
    }
}

#region  AssetReferenceCharacterDetails
[System.Serializable]
public class AssetReferenceCharacterDetails : AssetReference
{
    public AssetReferenceCharacterDetails(string assetPath) : base(assetPath) { }

    public override bool ValidateAsset(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        if (obj is GameObject)
        {
            GameObject gameObject = (GameObject)obj;
            return gameObject.GetComponent<CharacterDetails>() != null;
        }

        return false;
#else
        return false;
#endif
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        CharacterDetails characterDetails = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterDetails>(path);
        return characterDetails != null;
#else
        return false;
#endif
    }
}
#endregion
