using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SpellBtn : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameTxt;
    [SerializeField]
    private TextMeshProUGUI costTxt;
    [SerializeField]
    private Button button;

    private Spell _spell;

    public void SetSpell(Spell spell, Action<Spell> onClick = null)
    {
        nameTxt.text = spell.spellName;
        costTxt.text = spell.spellCost.ToString();

        _spell = spell;
        button.onClick.AddListener(() => onClick?.Invoke(_spell));
    }
}

[System.Serializable]
public class AssetReferenceSpellButton : AssetReference
{
    public AssetReferenceSpellButton(string name) : base(name)
    {
    }

    public override bool ValidateAsset(UnityEngine.Object obj)
    {
        var type = obj.GetType();
        if (!typeof(Button).IsAssignableFrom(type))
        {
            return false;
        }

        SpellBtn button = ((GameObject)obj).GetComponent<SpellBtn>();

        return button != null;
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        SpellBtn button = UnityEditor.AssetDatabase.LoadAssetAtPath<SpellBtn>(path);
        return button != null;
#else
        return false;
#endif
    }
}
