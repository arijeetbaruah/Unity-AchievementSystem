using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines;
using System.Linq;
using Game.Service;
using Game.Events;

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
        registeredCharacters = new List<CharacterDetails>();
        GameManager.instance.playerInitCompleted = false;

        EventManager.Trigger<AsyncEvent>(new AsyncEvent(InitantiateCharacters()));

        SetState(new InitState(this));
    }

    private IEnumerator InitantiateCharacters()
    {
        yield return ServiceRegistry.Get<CharacterSpawnerService>().InstantiateCharacter(
            "Player",
            gridManager.GetCell(CalculatePositionX(activePlayerCharacter.Count), 4),
            playerGO =>
            {
                registeredCharacters.Add(playerGO.GetComponent<CharacterDetails>());
            });

        yield return ServiceRegistry.Get<CharacterSpawnerService>().InstantiateCharacter(
            "Boss 1",
            gridManager.GetCell(CalculatePositionX(activeAICharacter.Count), 6),
            playerGO =>
            {
                registeredCharacters.Add(playerGO.GetComponent<CharacterDetails>());
            });

        for (int i = 0; i < 2; i++)
        {
            yield return ServiceRegistry.Get<CharacterSpawnerService>().InstantiateCharacter(
                "Enemy 1",
                gridManager.GetCell(CalculatePositionX(activeAICharacter.Count), 6),
                playerGO =>
                {
                    CharacterDetails character = playerGO.GetComponent<CharacterDetails>();
                    character.characterID = $"{character.characterID} {i + 1}";
                    registeredCharacters.Add(character);
                });
        }

        GameManager.instance.playerInitCompleted = true;
    }

    private int CalculatePositionX(int count)
    {
        int pos = Mathf.CeilToInt((float)count / 2f);

        return (pos * (count % 2 == 0 ? -1 : 1)) + (int) gridManager.GetSize.x / 2;
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
