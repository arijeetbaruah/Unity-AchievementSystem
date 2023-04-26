using Game.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "RPG/Magic/Single Status Target")]
public class SingleStatusSpell : Spell, SingleTarget
{
    public CombatStatus status;
    public float successRate;

    [HideInInspector]
    public bool isPlayer;
    public Action onMoveLeftEvent;
    public Action onMoveRightEvent;
    public Action onAttackEvent;

    public bool IsPlayer
    {
        get => isPlayer;
        set => isPlayer = value;
    }

    public Action OnMoveLeftEvent
    {
        get => onMoveLeftEvent;
        set => onMoveLeftEvent = value;
    }

    public Action OnMoveRightEvent
    {
        get => onMoveRightEvent;
        set => onMoveRightEvent = value;
    }

    public Action OnAttackEvent
    {
        get => onAttackEvent;
        set => onAttackEvent = value;
    }

    public override void Execute(List<CharacterDetails> target, CharacterDetails characterDetails, Action<bool> callback)
    {
        float percent = UnityEngine.Random.Range(0, 100);
        if (percent < successRate)
        {
            target[0].AddStatusEffect(status);
            UpdateStatus(target[0]);
            EventManager.Trigger(new ReportStatusEvent(target[0].characterID, status));
        }
        callback?.Invoke(false);
    }

    public override void Update()
    {
        if (isPlayer)
        {
            PlayerUpdate();
        }
    }

    public void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnMoveRightEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnMoveLeftEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnAttackEvent?.Invoke();
        }
    }
}
