using Game.Events;
using System;
using UnityEngine;

public abstract class ICombatCommand : ScriptableObject
{
    public string animation;
    public int animationState => Animator.StringToHash(animation);

    public int baseDmg;
    [Range(0f, 100f)]
    public float critRate;
    public DamageType damageType;

    public abstract void Execute(CharacterDetails target, CharacterDetails characterDetails, Action<bool> callback);
    public abstract void Update();

    public bool IsCrit()
    {
        float percent = UnityEngine.Random.Range(0f, 100f);
        bool ret = percent < critRate;

        if (ret)
        {
            EventManager.Trigger(new CritEvent());
        }

        return ret;
    }
}

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Wind,
    Lightning,
    Psychic,
    Necrotic,
    Radiant
}
