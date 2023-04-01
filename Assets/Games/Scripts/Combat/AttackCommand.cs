using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICombatCommand
{
    public void Execute(CharacterDetails target, CharacterStats characterStats, Action callback)
    {
        int attack = characterStats.Stats.attack;
        int dmg = target.Stats.CalculateDamage(attack, 5);
        target.TakeDamage(dmg);
        callback?.Invoke();
    }
}
