using Game.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Multi Status Target")]
public class MultiTargetStatusSpell : Spell
{
    public override void Execute(List<CharacterDetails> targets, CharacterDetails characterDetails, Action<bool> callback)
    {
        int attack = characterDetails.Stats.Stats.attack;
        bool oneMore = false;

        foreach (var target in targets)
        {
            UpdateStatus(target);
        }

        callback?.Invoke(oneMore);

        characterDetails.Animator.Play(animationState);
    }

    public override void Update()
    {

    }
}
