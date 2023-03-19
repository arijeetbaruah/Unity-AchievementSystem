using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;

[GlobalConfig, CreateAssetMenu]
public class CharacterDatabase : GlobalConfig<CharacterDatabase> 
{
    public List<CharacterDetails> characters;

    [ReadOnly, ShowInInspector]
    public SerializedCharacterDetailsDictionary characterDatabase => new SerializedCharacterDetailsDictionary(characters.ToDictionary(c => c.characterID));
}

[System.Serializable]
public class SerializedCharacterDetailsDictionary : UnitySerializedDictionary<string, CharacterDetails>
{
    public SerializedCharacterDetailsDictionary() : base() { }
    public SerializedCharacterDetailsDictionary(Dictionary<string, CharacterDetails> dic) : base(dic) { }
}
