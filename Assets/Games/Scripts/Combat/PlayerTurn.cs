using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines;
using Game.Service;
using Game.Events;
using Game.Logger;

public class PlayerTurn : IState
{
    private CombatStateMachine combatStateMachine;
    private CharacterDetails characterDetails;
    private EventManager eventManager => ServiceRegistry.Get<EventManager>();

    private List<CharacterDetails> targetingCharacter;
    private int currentTargetIndex = 0;
    private int previousTargetIndex = -1;

    private bool targeting = false;
    private bool attacking = false;
    private float waitingTimer = 1;

    public PlayerTurn(CombatStateMachine combatStateMachine, CharacterDetails characterDetails)
    {
        this.combatStateMachine = combatStateMachine;
        this.characterDetails = characterDetails;
    }

    public void OnAttack(AttackButtonClickEvent @event)
    {
        characterDetails.GameplayCanvas.OpenAll();
        targetingCharacter = new List<CharacterDetails>(combatStateMachine.activeAICharacter);
        currentTargetIndex = 0;
        targeting = true;
        ShowTarget();

        Log.Print("On Player Attack", FilterLog.Game);
    }

    public void ShowTarget()
    {
        characterDetails.VirtualCamera.Priority = 50;
        if (previousTargetIndex != -1)
        {
            targetingCharacter[previousTargetIndex].VirtualCamera.Priority = 50;
        }
        targetingCharacter[currentTargetIndex].VirtualCamera.Priority = 100;
        previousTargetIndex = currentTargetIndex;
    }

    public void OnStart()
    {
        characterDetails.VirtualCamera.Priority = 100;
        attacking = false;
        targeting = false;
        characterDetails.GameplayCanvas.OpenAll();
        eventManager.AddListener<AttackButtonClickEvent>(OnAttack);
    }

    public void OnUpdate(float deltaTime)
    {
        if (targeting)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentTargetIndex++;
                if (currentTargetIndex>=targetingCharacter.Count)
                {
                    currentTargetIndex = 0;
                }
                ShowTarget();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentTargetIndex--;
                if (currentTargetIndex < 0)
                {
                    currentTargetIndex = targetingCharacter.Count - 1;
                }
                ShowTarget();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var target = targetingCharacter[currentTargetIndex];
                int attack = characterDetails.Stats.Stats.attack;
                int dmg = target.Stats.CalculateDamage(attack, 5);
                target.TakeDamage(dmg);
                targeting = false;
                attacking = true;
            }
        }

        if (attacking)
        {
            waitingTimer -= deltaTime;
            if (waitingTimer < 0)
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

    public void OnEnd()
    {
        characterDetails.VirtualCamera.Priority = 50;
    }
}
