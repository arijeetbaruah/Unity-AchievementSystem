using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Utilities;

[GlobalConfig, CreateAssetMenu]
public class CharacterDatabase : GlobalConfig<CharacterDatabase> 
{
    public List<CharacterDetails> characters;
}
