using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Single Target")]
public class SingleTargetSpell : Spell
{

    [HideInInspector]
    public bool isPlayer;
    public Action OnMoveLeftEvent;
    public Action OnMoveRightEvent;
    public Action OnAttackEvent;

    public override void Execute(List<CharacterDetails> target, CharacterDetails characterDetails, Action<bool> callback)
    {
        int attack = characterDetails.Stats.Stats.attack;
        int dmg = target[0].Stats.CalculateDamage(attack, baseDmg);
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
            callback?.Invoke(isCrit || isWeak);
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
