using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.StateMachines;
using Game.Logger;

public class PlayerState : IState
{
    public CombatStateMachine combatStateMachine;
    public CharacterDetails characterDetails;

    public PlayerState(CharacterDetails characterDetails, CombatStateMachine combatStateMachine)
    {
        this.combatStateMachine = combatStateMachine;
        this.characterDetails = characterDetails;
    }

    public void OnStart()
    {
        Log.Print($"{characterDetails.characterID} turn start", FilterLog.Game);
    }

    public void OnUpdate(float deltaTime)
    {

    }

    public void OnEnd()
    {
        Log.Print($"{characterDetails.characterID} turn end", FilterLog.Game);
    }
}
