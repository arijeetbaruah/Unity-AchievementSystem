using System;
using UnityEngine;

public abstract class ICombatCommand : ScriptableObject
{
    public string animation;
    public int animationState => Animator.StringToHash(animation);

    public int baseDmg;
    [Range(0f, 100f)]
    public float critRate;

    public abstract void Execute(CharacterDetails target, CharacterDetails characterDetails, Action<bool> callback);

    public bool IsCrit()
    {
        float percent = UnityEngine.Random.Range(0f, 100f);
        return percent < critRate;
    }
}

public abstract class BaseMagicCommand : ICombatCommand
{
}
