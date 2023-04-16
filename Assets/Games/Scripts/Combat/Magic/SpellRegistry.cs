using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellRegistry", menuName = "RPG/Magic/Registry"), GlobalConfig("SpellRegistry")]
public class SpellRegistry : GlobalConfig<SpellRegistry>
{
    public SpellDictionary Spells;
}

[System.Serializable]
public class SpellDictionary : UnitySerializedDictionary<string, Spell>
{

}
