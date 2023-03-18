using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines;
using Game.Service;
using Game.Events;
using Game.Logger;

public class PlayerTurn : IState
{
    private CombatStateMachine combatStateMachine;
    private CharacterDetails characterDetails;
    private EventManager eventManager => ServiceRegistry.Get<EventManager>();

    public PlayerTurn(CombatStateMachine combatStateMachine, CharacterDetails characterDetails)
    {
        this.combatStateMachine = combatStateMachine;
        this.characterDetails = characterDetails;
    }

    public void OnAttack(AttackButtonClickEvent @event)
    {
        characterDetails.GameplayCanvas.OpenAll();

        Log.Print("On Player Attack", FilterLog.Game);
    }

    public void OnStart()
    {
        characterDetails.VirtualCamera.Priority = 100;
        characterDetails.GameplayCanvas.OpenAll();
        eventManager.AddListener<AttackButtonClickEvent>(OnAttack);
    }

    public void OnUpdate(float deltaTime)
    {
        
    }

    public void OnEnd()
    {
        characterDetails.VirtualCamera.Priority = 50;
    }
}
