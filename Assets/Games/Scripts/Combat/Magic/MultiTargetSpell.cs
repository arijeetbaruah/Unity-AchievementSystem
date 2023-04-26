using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Multi Target")]
public class MultiTargetSpell : Spell
{

    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        Stats characterStats = characterDetails.Stats.Stats + characterDetails.inventory.GetBonus;

        int attack = characterStats.attack;
        bool oneMore = false;

        foreach (var target in targets)
        {
            Stats targetStats = target.Stats.Stats + target.inventory.GetBonus;

            int dmg = targetStats.CalculateMagicDamage(attack, baseDmg);
            bool isCrit = IsCrit();
            bool isWeak = target.weaknesses.Contains(damageType);

            if (isWeak)
            {
                dmg *= 2;
                isWeak = target.AddStatusEffect(CombatStatus.Down);
            }

            if (isCrit)
            {
                dmg *= 2;
                isCrit = target.AddStatusEffect(CombatStatus.Down);
            }

            oneMore = oneMore || (!target.StatusEffect.Contains(CombatStatus.Down) && isCrit || isWeak);
            UpdateStatus(target);
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
