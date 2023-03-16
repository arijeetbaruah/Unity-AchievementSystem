using Game.StateMachines;
using Game.Logger;

public class AIState : IState
{
    public CombatStateMachine combatStateMachine;
    public CharacterDetails characterDetails;

    public AIState(CharacterDetails characterDetails, CombatStateMachine combatStateMachine)
    {
        this.combatStateMachine = combatStateMachine;
        this.characterDetails = characterDetails;
    }

    public void OnStart()
    {
        Log.Print($"AI {characterDetails.characterID} turn start", FilterLog.Game);
    }

    public void OnUpdate(float deltaTime)
    {

    }

    public void OnEnd()
    {
        Log.Print($"AI {characterDetails.characterID} turn end", FilterLog.Game);
    }
}
