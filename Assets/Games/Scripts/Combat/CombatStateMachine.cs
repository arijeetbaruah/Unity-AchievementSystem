using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines;
using System.Linq;

public class CombatStateMachine : StateMachine
{
    private GridManager gridManager;

    public List<CharacterDetails> registeredCharacters;

    public List<CharacterDetails> activeOrderedCharacter => registeredCharacters
        .Where(c => c.currentHP > 0)
        .OrderBy(c => c.Stats.Stats.speed)
        .ToList();

    public List<CharacterDetails> activeAICharacter => activeOrderedCharacter
        .Where(c => !c.isPlayer)
        .ToList();

    public List<CharacterDetails> activePlayerCharacter => activeOrderedCharacter
        .Where(c => c.isPlayer)
        .ToList();

    public Queue<CharacterDetails> combatOrder;

    public CombatStateMachine(GridManager gridManager)
    {
        this.gridManager = gridManager;
        registeredCharacters = new List<CharacterDetails>(GameObject.FindObjectsOfType<CharacterDetails>());

        SetState(new InitState(this));
    }

    public CharacterDetails GetNextCombatant()
    {
        if (combatOrder == null || combatOrder.Count == 0)
        {
            combatOrder = new Queue<CharacterDetails>(activeOrderedCharacter);
        }

        return combatOrder.Dequeue();
    }
}
