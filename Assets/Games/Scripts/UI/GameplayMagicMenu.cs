using Game.Service;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMagicMenu : MonoBehaviour
{
    private SpellBtnObjectPool buttonPool => ServiceRegistry.Get<PoolService>().spellBtnPool;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private Button closeBtn;

    public Action OnClose;

    private Dictionary<string, SpellBtn> spellDic = new Dictionary<string, SpellBtn>();

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveAllListeners();
        OnClose?.Invoke();
    }

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
