using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[GlobalConfig]
public class StatusIconSO : GlobalConfig<StatusIconSO>
{
    public List<StatusIcon> statusIcons;
    public List<SerializedKeyValuePair<CombatStatus, DamageType>> technicalCombo;

    [System.Serializable]
    public struct StatusIcon
    {
        public CombatStatus status;
        public Sprite texture;
    }
}

[System.Serializable]
public class SerializedKeyValuePair<T, K>
{
    public T key;
    public K value;
}
