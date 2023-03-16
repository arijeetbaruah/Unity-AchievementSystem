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

    public IState GetCharacterState(CharacterDetails characterDetail)
    {
        if (characterDetail == null)
        {
            return null;
        }

        return characterDetail.isAIControlled ? new AIState(characterDetail, this) : new PlayerState(characterDetail, this);
    }
}
