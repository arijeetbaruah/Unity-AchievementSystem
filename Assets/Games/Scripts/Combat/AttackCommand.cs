using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "RPG/Combat/Attack")]
public class AttackCommand : ICombatCommand
{
    [HideInInspector]
    public bool isPlayer;
    public Action OnMoveLeftEvent;
    public Action OnMoveRightEvent;
    public Action OnAttackEvent;

    public override void Execute(CharacterDetails target, CharacterDetails characterDetails, Action<bool> callback)
    {
        Stats characterStats = characterDetails.Stats.Stats + characterDetails.inventory.GetBonus;
        Stats targetStats = target.Stats.Stats + target.inventory.GetBonus;

        int attack = characterStats.attack;
        int dmg = targetStats.CalculateDamage(attack, baseDmg);
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

        characterDetails.OnTriggerHitAnimation = () =>
        {
            target.TakeDamage(dmg);
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
