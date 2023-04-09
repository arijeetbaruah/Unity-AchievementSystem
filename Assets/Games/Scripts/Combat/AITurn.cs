using Game.Events;
using Game.Logger;
using Game.Service;
using Game.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class AITurn : BaseState
{
    private List<CharacterDetails> targetingCharacter;
    private CharacterDetails targetCharacter;

    private float waitTimer = 3;
    private bool attacked = false;

    public AITurn(CombatStateMachine combatStateMachine, CharacterDetails characterDetails) : base(combatStateMachine, characterDetails)
    {
    }

    public override void OnStart()
    {
        characterDetails.VirtualCamera.Priority = 100;
        targetingCharacter = new List<CharacterDetails>(combatStateMachine.activePlayerCharacter);
        targetCharacter = targetingCharacter[Random.Range(0, targetingCharacter.Count - 1)];

        Log.Print("On AI Attack", FilterLog.Game);
    }

    public override void OnUpdate(float deltaTime)
    {
        waitTimer -= deltaTime;
        if (waitTimer <= 0)
        {
            GoToNextTurn();
        }
        else if (waitTimer <= 2 && !attacked)
        {
            attacked = true;
            characterDetails.VirtualCamera.Priority = 50;
            targetCharacter.VirtualCamera.Priority = 100;

            int attack = characterDetails.Stats.Stats.attack;
            int dmg = targetCharacter.Stats.CalculateDamage(attack, 5);

            EventManager.Trigger(new ChargeMax(characterDetails.characterID, new List<string>() { targetCharacter.characterID }, 2));

            targetCharacter.TakeDamage(dmg);
        }
    }

    public override void OnEnd()
    {
        characterDetails.VirtualCamera.Priority = 50;
        Log.Print("On AI Attack end", FilterLog.Game);
    }
}
