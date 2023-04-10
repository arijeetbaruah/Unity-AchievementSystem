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
using System;
using System.Linq;
using Sirenix.Utilities;

public class CharacterDetails : MonoBehaviour
{
    private readonly int TakeHitAnimationHash = Animator.StringToHash("TakeDamage");
    private readonly int DeathAnimationHash = Animator.StringToHash("Die");

    public string characterID;

    [SerializeField]
    private CharacterStats characterStats;
    [SerializeField]
    private GameplayCanvas gameplayCanvas;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private HPBar AIHPBar;

    public Action OnTriggerHitAnimation;
    public List<DamageType> weaknesses;

    [ValueDropdown("GetSpells", IsUniqueList = true)]
    public List<Spell> knownSpells;

    [SerializeField]
    private CinemachineVirtualCamera vcam;

    public int currentHP;
    public int currentMana;
    public int currentMax;

    public bool isPlayer;

    public UnityEvent<string, int, int> OnDamageEvent;
    public UnityEvent<string, int, int> OnDeathEvent;

    //[ShowInInspector, SerializeReference]
    public AttackCommand normalAttack;

    //[ShowInInspector, SerializeReference]
    public AttackCommand superAttack;

    public Animator Animator => animator;
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

    private void OnEnable()
    {
        ServiceRegistry.Get<EventManager>().AddListener<ChargeMax>(ChargeMax);
        ServiceRegistry.Get<EventManager>().AddListener<ResetPlayerCharge>(ResetPlayerCharge);
    }

    private void OnDisable()
    {
        ServiceRegistry.Get<EventManager>().RemoveListener<ChargeMax>(ChargeMax);
        ServiceRegistry.Get<EventManager>().RemoveListener<ResetPlayerCharge>(ResetPlayerCharge);
    }

    public void TriggerHitAnimation()
    {
        OnTriggerHitAnimation?.Invoke();
        OnTriggerHitAnimation = null;
    }

    public void ResetPlayerCharge(ResetPlayerCharge resetPlayerCharge)
    {
        if (resetPlayerCharge.playerID == characterID)
        {
            currentMax = 0;
            gameplayCanvas.SuperButton.interactable = false;
        }
    }

    public void ChargeMax(ChargeMax chargeMax)
    {
        if (chargeMax.attackerID == characterID || chargeMax.targetID.Contains(characterID))
        {
            currentMax = Mathf.Min(currentMax + chargeMax.amount, Stats.Stats.maxCharge);

            if (gameplayCanvas.SuperButton)
            {
                gameplayCanvas.SuperButton.interactable = currentMax == Stats.Stats.maxCharge;
            }

            EventManager.Trigger(new PlayerUpdateCharge(characterID, currentMax));
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

    private static IEnumerable GetSpells {
        get
        {
            ValueDropdownList<Spell> list = new ValueDropdownList<Spell>();
            SpellRegistry.Instance.Spells.ForEach(pair =>
            {
                list.Add(pair.Key, pair.Value);
            });

            return list;
        }
    }
}

public class OnCharacterDeath : GameEvent
{
    public OnCharacterDeath()
    {

    }
}
