using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Single Target")]
public class SingleTargetSpell : Spell, SingleTarget
{

    [HideInInspector]
    public bool isPlayer;
    public Action onMoveLeftEvent;
    public Action onMoveRightEvent;
    public Action onAttackEvent;

    public bool IsPlayer {
        get => isPlayer;
        set => isPlayer = value;
    }

    public Action OnMoveLeftEvent
    {
        get => onMoveLeftEvent;
        set => onMoveLeftEvent = value;
    }

    public Action OnMoveRightEvent
    {
        get => onMoveRightEvent;
        set => onMoveRightEvent = value;
    }

    public Action OnAttackEvent
    {
        get => onAttackEvent;
        set => onAttackEvent = value;
    }

    public override void Execute(List<CharacterDetails> target, CharacterDetails characterDetails, Action<bool> callback)
    {
        Stats characterStats = characterDetails.Stats.Stats + characterDetails.inventory.GetBonus;
        Stats targetStats = target[0].Stats.Stats + target[0].inventory.GetBonus;


        int attack = characterStats.attack;
        int dmg = targetStats.CalculateDamage(attack, baseDmg);
        bool isCrit = IsCrit();
        bool isWeak = target[0].weaknesses.Contains(damageType);

        if (isWeak)
        {
            dmg *= 2;
        }

        if (isCrit)
        {
            dmg *= 2;
        }

        characterDetails.OnTriggerHitAnimation = () =>
        {
            target[0].TakeDamage(dmg);
            callback?.Invoke(target[0].StatusEffect.Contains(CombatStatus.Down) && (isCrit || isWeak));
        };
        characterDetails.Animator.Play(animationState);
    }

    public override void Update()
    {
        if (isPlayer)
        {
            PlayerUpdate();
        }
    }

    public void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnMoveRightEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnMoveLeftEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnAttackEvent?.Invoke();
        }
    }
}

public interface SingleTarget
{
    bool IsPlayer { get; set; }
    Action OnMoveLeftEvent { get; set; }
    Action OnMoveRightEvent { get; set; }
    Action OnAttackEvent { get; set; }
}
