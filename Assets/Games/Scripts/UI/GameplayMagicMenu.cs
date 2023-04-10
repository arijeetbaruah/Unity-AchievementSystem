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

    public void SpawnBtn(Spell spell, Action<Spell> onClick)
    {
        spellBtnRef.InstantiateAsync(content).Completed += handler =>
        {
            SpellBtn spellBtn = handler.Result.GetComponent<SpellBtn>();
            spellBtn.SetSpell(spell, onClick);
        };
    }
}
