using Game.Service;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMagicMenu : MonoBehaviour
{
    private SpellBtnObjectPool buttonPool => ServiceRegistry.Get<PoolService>().spellBtnPool;
    [SerializeField]
    private Transform content;

    private Dictionary<string, SpellBtn> spellDic = new Dictionary<string, SpellBtn>();

    public void SpawnBtn(Spell spell, Action<Spell> onClick)
    {
        if (spellDic.TryGetValue(spell.spellId, out SpellBtn btn))
        {
            btn.SetSpell(spell, onClick);
            return;
        }

        buttonPool.GetInstance(btn =>
        {
            spellDic.Add(spell.spellId, btn);
            btn.transform.SetParent(content);
            btn.GetComponent<RectTransform>().position = Vector3.zero;
            btn.GetComponent<RectTransform>().localScale = Vector3.one;
            btn.SetSpell(spell, onClick);
        });
    }
}
