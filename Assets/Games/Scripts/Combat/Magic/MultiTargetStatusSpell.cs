using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Multi Status Target")]
public class MultiTargetStatusSpell : Spell
{
    public CombatStatus status;
    public float successRate;

    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        int attack = characterDetails.Stats.Stats.attack;
        bool oneMore = false;

        foreach (var target in targets)
        {
            float percent = UnityEngine.Random.Range(0, 100);
            if (percent < successRate)
            {
                target.AddStatusEffect(status);
            }
            callback?.Invoke(false);
        }

        callback?.Invoke(oneMore);

        characterDetails.Animator.Play(animationState);
    }

    public override void Update()
    {

    }
}
