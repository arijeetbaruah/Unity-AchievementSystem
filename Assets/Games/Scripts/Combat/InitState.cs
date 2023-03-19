using Game.StateMachines;

public class InitState : IState
{
    public CombatStateMachine combatStateMachine;
    public float waitTimer = 6;

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
        waitTimer -= deltaTime;

        if (waitTimer <= 0)
        {
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
}
