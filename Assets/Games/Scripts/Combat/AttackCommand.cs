using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "RPG/Combat/Attack")]
public class AttackCommand : ICombatCommand
{
    public override void Execute(CharacterDetails target, CharacterDetails characterDetails, Action<bool> callback)
    {
        int attack = characterDetails.Stats.Stats.attack;
        int dmg = target.Stats.CalculateDamage(attack, baseDmg);
        bool isCrit = IsCrit();

        if (isCrit)
        {
            dmg *= 2;
        }

        characterDetails.OnTriggerHitAnimation = () =>
        {
            target.TakeDamage(dmg);
            callback?.Invoke(isCrit);
        };
        characterDetails.Animator.Play(animationState);
    }
}
