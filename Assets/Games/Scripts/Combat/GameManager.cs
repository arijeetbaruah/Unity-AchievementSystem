using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private CombatStateMachine combatStateMachine;

    [SerializeField]
    private GridManager gridManager;

    [SerializeField]
    private List<CharacterDetails> characterDetails = new List<CharacterDetails>();

    private int currentTurn = 0;

    private void Start()
    {
        instance = this;
        combatStateMachine = new CombatStateMachine();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        combatStateMachine?.OnUpdate(Time.deltaTime);
    }

    public CharacterDetails GetCurrentCharacter()
    {
        return characterDetails[currentTurn];
    }

    public CharacterDetails NextTurn()
    {
        currentTurn++;
        if (currentTurn >= characterDetails.Count)
        {
            currentTurn = 0;
        }

        return characterDetails[currentTurn];
    }

    public void AddCharacterToList(CharacterDetails characterDetail)
    {
        characterDetails.Add(characterDetail);
        OrderCharacter();
    }

    public void OrderCharacter()
    {
        characterDetails = characterDetails.OrderBy(cd => cd.Stats.Stats.speed).ToList();
    }
}
