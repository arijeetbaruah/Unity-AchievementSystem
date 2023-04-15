using Game.Events;
using Game.Logger;
using Game.Service;
using Game.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : BaseState
{
    private List<CharacterDetails> targetingCharacter;
    private CharacterDetails targetCharacter;

    private float waitTimer = 5;
    private bool attacked = false;
    private bool isCrit = false;

    public AITurn(CombatStateMachine combatStateMachine, CharacterDetails characterDetails) : base(combatStateMachine, characterDetails)
    {
    }

    public override void OnStart()
    {
        characterDetails.VirtualCamera.Priority = 100;
        targetingCharacter = new List<CharacterDetails>(combatStateMachine.activePlayerCharacter);
        targetCharacter = targetingCharacter[Random.Range(0, targetingCharacter.Count - 1)];

        StartStatusEffect();

        Log.Print("On AI Attack", FilterLog.Game);
    }

    public void StartStatusEffect()
    {
        characterDetails.StatusEffect.ForEach(status =>
        {
            StartStatusEffect(status);
        });
    }

    public void StartStatusEffect(CombatStatus status)
    {
        switch (status)
        {
            case CombatStatus.Poisoned:
            case CombatStatus.Bleeding:
            case CombatStatus.Burning:
                characterDetails.TakeDamage(5);
                break;
            case CombatStatus.Down:
                characterDetails.RemoveStatusEffect(status);
                break;
            case CombatStatus.Confused:
                var targets = combatStateMachine.activeOrderedCharacter;
                int index = UnityEngine.Random.Range(0, targets.Count);

                characterDetails.normalAttack.Execute(targets[index], characterDetails, hasCrit =>
                {
                    isCrit = hasCrit;
                    EventManager.Trigger(new ChargeMax(
                        characterDetails.characterID,
                        new List<string>() { targets[index].characterID }, 5)
                    );
                });
                GoToNextTurn();

                break;
            case CombatStatus.Paralyzed:
                GoToNextTurn();
                break;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        waitTimer -= deltaTime;
        if (waitTimer <= 0)
        {
            EventManager.Trigger(new IsShowingTextEvent(showing =>
            {
                if (!showing)
                {
                    if (isCrit)
                    {
                        EventManager.Trigger(new OneMoreEvent());
                        combatStateMachine.SetState(new AITurn(combatStateMachine, characterDetails));
                    }
                    else
                    {
                        GoToNextTurn();
                    }
                }

            }));
        }
        else if (waitTimer <= 2 && !attacked)
        {
            attacked = true;
            characterDetails.VirtualCamera.Priority = 50;
            targetCharacter.VirtualCamera.Priority = 100;

            
            characterDetails.normalAttack.Execute(targetCharacter, characterDetails, isCrit =>
            {
                this.isCrit = isCrit;
                EventManager.Trigger(new ChargeMax(characterDetails.characterID, new List<string>() { targetCharacter.characterID }, 2));
            });
        }
    }

    public override void OnEnd()
    {
        characterDetails.VirtualCamera.Priority = 50;
        Log.Print("On AI Attack end", FilterLog.Game);
    }
}
