using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private CombatStateMachine combatStateMachine = new CombatStateMachine();

    [SerializeField]
    private GridManager gridManager;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        combatStateMachine.OnUpdate(Time.deltaTime);
    }
}
