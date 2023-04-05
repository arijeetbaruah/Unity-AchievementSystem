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
    public Dictionary<string, AssetReference> references => 
        characters.ToDictionary(e => e.key, e => e.value);

    [System.Serializable]
    public struct CharacterDetailReferenceListElement
    {
        public string key;
        public AssetReference value;
    }
}

