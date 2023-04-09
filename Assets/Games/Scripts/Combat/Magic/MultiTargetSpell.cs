using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Multi Target")]
public class MultiTargetSpell : Spell
{
    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        int attack = characterDetails.Stats.Stats.attack;
        bool oneMore = false;

        foreach (var target in targets)
        {
            int dmg = target.Stats.CalculateDamage(attack, baseDmg);
            bool isCrit = IsCrit();
            bool isWeak = target.weaknesses.Contains(damageType);

            if (isWeak)
            {
                dmg *= 2;
            }

            if (isCrit)
            {
                dmg *= 2;
            }

            oneMore = oneMore && (isCrit || isWeak);
            target.TakeDamage(dmg);
        }
        
        characterDetails.OnTriggerHitAnimation = () =>
        {
            callback?.Invoke(oneMore);
        };

        characterDetails.Animator.Play(animationState);
    }

    public override void Update()
    {
        
    }
}
