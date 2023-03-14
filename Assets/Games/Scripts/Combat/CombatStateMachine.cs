using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        SetState(new InitState(this));
    }
}
