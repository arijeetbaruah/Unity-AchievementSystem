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
    private GameplayMagicMenu gameplayMagicMenu;
    [SerializeField]
    private ItemMenuUI itemMenuUI;
    [SerializeField]
    private Animator animator;

    public Action OnTriggerHitAnimation;
    public List<DamageType> weaknesses;

    [ValueDropdown("GetSpells", IsUniqueList = true)]
    public List<Spell> knownSpells;

    [SerializeField]
    private List<CombatStatus> statusEffect = new List<CombatStatus>();
    public List<CombatStatus> StatusEffect  => statusEffect;

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

    public CharacterInventory inventory;

    public Animator Animator => animator;
    public CharacterStats Stats => characterStats;
    public GameplayCanvas GameplayCanvas => gameplayCanvas;
    public GameplayMagicMenu GameplayMagicCanvas => gameplayMagicMenu;
    public ItemMenuUI ItemMenuUI => itemMenuUI;
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

            if (gameplayCanvas && gameplayCanvas.SuperButton)
            {
                gameplayCanvas.SuperButton.interactable = currentMax == Stats.Stats.maxCharge;
            }

            EventManager.Trigger(new PlayerUpdateCharge(characterID, currentMax));
        }
    }

    public void UseMana(int amount)
    {
        currentMana = Mathf.Max(0, currentMana - amount);

        EventManager.Trigger(new PlayerUpdateMana(characterID, currentMana));
    }

    public void TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        EventManager.Trigger<PlayerUpdateHP>(new PlayerUpdateHP(characterID, currentHP));

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

    public void AddStatusEffect(CombatStatus combatStatus)
    {
        statusEffect.Add(combatStatus);
        EventManager.Trigger(new UpdateStatusEffect(characterID));
    }

    public void RemoveStatusEffect(CombatStatus combatStatus)
    {
        statusEffect.Remove(combatStatus);
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

public class UpdateStatusEffect : GameEvent
{
    public string characterID;

    public UpdateStatusEffect(string characterID)
    {
        this.characterID = characterID;
    }
}
