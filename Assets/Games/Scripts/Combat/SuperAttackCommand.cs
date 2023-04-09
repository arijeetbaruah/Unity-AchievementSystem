using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperAttackCommand : ICombatCommand
{
    public void Execute(CharacterDetails target, CharacterStats characterStats, Action callback)
    {
        int attack = characterStats.Stats.attack;
        int dmg = target.Stats.CalculateDamage(attack, 15);
        target.TakeDamage(dmg);
        callback?.Invoke();
    }
}
