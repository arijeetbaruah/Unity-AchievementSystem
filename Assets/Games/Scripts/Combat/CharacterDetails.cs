using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

using DG.Tweening;
using Cinemachine;
using Game.Events;
using Game.Service;

public class CharacterDetails : MonoBehaviour
{
    private readonly int TakeHitAnimationHash = Animator.StringToHash("TakeDamage");
    private readonly int DeathAnimationHash = Animator.StringToHash("TakeDamage");

    public string characterID;

    [SerializeField]
    private CharacterStats characterStats;
    [SerializeField]
    private GameplayCanvas gameplayCanvas;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private HPBar AIHPBar;

    [SerializeField]
    private CinemachineVirtualCamera vcam;

    public int currentHP;
    public int currentMana;
    public int currentMax;

    public bool isPlayer;

    public UnityEvent<string, int, int> OnDamageEvent;
    public UnityEvent<string, int, int> OnDeathEvent;

    [ShowInInspector, SerializeReference]
    public ICombatCommand normalAttack;

    public CharacterStats Stats => characterStats;
    public GameplayCanvas GameplayCanvas => gameplayCanvas;
    public CinemachineVirtualCamera VirtualCamera => vcam;

    private IEnumerator Start()
    {
        currentHP = characterStats.Stats.maxHP;
        currentMana = characterStats.Stats.maxMana;
        currentMax = 0;

        yield return null;

        if (isPlayer)
        {
            EventManager.Trigger(new CreatePlayerHUD(this));
        }
        else
        {
            AIHPBar?.SetHP(currentHP, Stats.Stats.maxHP);
        }
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
    }

    public void TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        EventManager.Trigger<PlayerUpdateHP>(new PlayerUpdateHP(characterID, currentHP));
        AIHPBar?.SetHP(currentHP, Stats.Stats.maxHP);


        if (currentHP == 0)
        {
            OnDeathEvent?.Invoke(characterID, dmg, currentHP);
            EventManager.Trigger<OnCharacterDeath>(new OnCharacterDeath());
            animator.Play(DeathAnimationHash);
        }
        else
        {
            OnDamageEvent?.Invoke(characterID, dmg, currentHP);
            animator.Play(TakeHitAnimationHash);
        }
    }
}

public class OnCharacterDeath : GameEvent
{
    public OnCharacterDeath()
    {

    }
}
