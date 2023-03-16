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
        GameManager.instance.OrderCharacter();
        
        var character = GameManager.instance.GetCurrentCharacter();

        combatStateMachine.SetState(combatStateMachine.GetCharacterState(character));
    }

    public void OnEnd()
    {
        
    }

    public void OnUpdate(float deltaTime)
    {
        
    }
}
