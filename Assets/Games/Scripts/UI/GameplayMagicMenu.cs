using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMagicMenu : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceSpellButton spellBtnRef;
    [SerializeField]
    private Transform content;

    private Dictionary<string, SpellBtn> spellDic = new Dictionary<string, SpellBtn>();

    public void SpawnBtn(Spell spell, Action<Spell> onClick)
    {
        if (spellDic.ContainsKey(spell.spellId))
        {
            spellDic[spell.spellId].SetSpell(spell, onClick);
        }
        else
        {
            spellBtnRef.InstantiateAsync(content).Completed += handler =>
            {
                SpellBtn spellBtn = handler.Result.GetComponent<SpellBtn>();
                spellDic.Add(spell.spellId, spellBtn);
                spellBtn.SetSpell(spell, onClick);
            };
        }
    }
}
