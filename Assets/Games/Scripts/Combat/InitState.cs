using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.StateMachines;

public class InitState : IState
{
    public CombatStateMachine combatStateMachine;

    public InitState(CombatStateMachine combatStateMachine)
    {
        this.combatStateMachine = combatStateMachine;
    }

    public void OnStart()
    {

    }

    public void OnEnd()
    {
        
    }

    public void OnUpdate(float deltaTime)
    {
        
    }
}
