using Game.Events;
using Game.Service;
using Game.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    protected CombatStateMachine combatStateMachine;
    protected CharacterDetails characterDetails;

    protected EventManager eventManager => ServiceRegistry.Get<EventManager>();

    public BaseState(CombatStateMachine combatStateMachine, CharacterDetails characterDetails)
    {
        this.combatStateMachine = combatStateMachine;
        this.characterDetails = characterDetails;
    }

    public abstract void OnStart();
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnEnd();

    public void GoToNextTurn()
    {
        bool hasPlayerWon = combatStateMachine.activeAICharacter.Count == 0;
        bool hasPlayerLost = combatStateMachine.activePlayerCharacter.Count == 0;
        if (hasPlayerWon || hasPlayerLost)
        {
            combatStateMachine.SetState(new GameOverState(hasPlayerWon));
            return;
        }

        CharacterDetails nextCharacter = combatStateMachine.GetNextCombatant();

        if (nextCharacter.isPlayer)
        {
            combatStateMachine.SetState(new PlayerTurn(combatStateMachine, nextCharacter));
        }
        else
        {
            combatStateMachine.SetState(new AITurn(combatStateMachine, nextCharacter));
        }
    }
}
