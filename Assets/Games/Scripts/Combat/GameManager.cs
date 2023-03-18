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

    private int currentTurn = 0;

    private void Start()
    {
        instance = this;
        combatStateMachine = new CombatStateMachine(gridManager);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        combatStateMachine?.OnUpdate(Time.deltaTime);
    }
}
