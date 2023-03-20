using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;
using Cinemachine;
using Game.Events;

public class CharacterDetails : MonoBehaviour
{
    public string characterID;

    [SerializeField]
    private CharacterStats characterStats;
    [SerializeField]
    private HPBar hpBar;
    [SerializeField]
    private HPBar manaBar;
    [SerializeField]
    private TextMeshProUGUI dmgTxtPrefab;
    [SerializeField]
    private Transform dmgTxtParent;
    [SerializeField]
    private GameplayCanvas gameplayCanvas;

    [SerializeField]
    private CinemachineVirtualCamera vcam;

    public int currentHP;
    public int currentMana;

    public bool isPlayer;

    public UnityEvent<string, int, int> OnDamageEvent;
    public UnityEvent<string, int, int> OnDeathEvent;

    public CharacterStats Stats => characterStats;
    public GameplayCanvas GameplayCanvas => gameplayCanvas;
    public CinemachineVirtualCamera VirtualCamera => vcam;

    private void Start()
    {
        currentHP = characterStats.Stats.maxHP;
        currentMana = characterStats.Stats.maxMana;
        hpBar.SetHP(currentHP, characterStats.Stats.maxHP);
        manaBar.SetHP(currentMana, characterStats.Stats.maxMana);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            UseMana(10);
        }
    }

    public void UseMana(int amount)
    {
        currentMana = Mathf.Max(0, currentMana - amount);
        manaBar.SetHP(currentMana, characterStats.Stats.maxMana);
    }

    public void TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        hpBar.SetHP(currentHP, characterStats.Stats.maxHP);
        var dmgTxt = Instantiate(dmgTxtPrefab, dmgTxtParent);
        dmgTxt.SetText(dmg.ToString());

        dmgTxt.transform.DOMoveY(10, 3).OnComplete(() =>
        {
            Destroy(dmgTxt.gameObject);
        });

        if (currentHP == 0)
        {
            OnDeathEvent?.Invoke(characterID, dmg, currentHP);
            EventManager.Trigger<OnCharacterDeath>(new OnCharacterDeath());
        }
        else
        {
            OnDamageEvent?.Invoke(characterID, dmg, currentHP);
        }
    }
}

public class OnCharacterDeath : GameEvent
{
    public OnCharacterDeath()
    {

    }
}
