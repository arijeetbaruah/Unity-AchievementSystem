using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachines;
using Game.Service;
using Game.Events;
using Game.Logger;
using System;

public class PlayerTurn : BaseState
{
    private List<CharacterDetails> targetingCharacter;
    private int currentTargetIndex = 0;
    private int previousTargetIndex = -1;

    private bool targeting = false;
    private bool superAttack = false;
    private bool attacking = false;
    private bool hasCrit = false;
    private float waitingTimer = 1;

    private ICombatCommand selectedAttack = null;

    public PlayerTurn(CombatStateMachine combatStateMachine, CharacterDetails characterDetails) : base(combatStateMachine, characterDetails)
    {
        
    }

    public void OnCharacterDeath(OnCharacterDeath @event)
    {
        previousTargetIndex = -1;
    }

    public void OnAttack(AttackButtonClickEvent @event)
    {
        characterDetails.GameplayCanvas.CloseAll();
        targetingCharacter = new List<CharacterDetails>(combatStateMachine.activeAICharacter);
        currentTargetIndex = 0;
        targeting = true;
        selectedAttack = characterDetails.normalAttack;
        ShowTarget();

        Log.Print("On Player Attack", FilterLog.Game);
    }

    public void OnSuperAttack(SuperAttackButtonClickEvent @event)
    {
        characterDetails.GameplayCanvas.CloseAll();
        targetingCharacter = new List<CharacterDetails>(combatStateMachine.activeAICharacter);
        currentTargetIndex = 0;
        superAttack = true;
        targeting = true;
        selectedAttack = characterDetails.superAttack;
        ShowTarget();

        Log.Print("On Player Super Attack", FilterLog.Game);
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

    public override void OnStart()
    {
        characterDetails.VirtualCamera.Priority = 100;
        attacking = false;
        superAttack = false;
        targeting = false;
        characterDetails.GameplayCanvas.OpenAll();

        characterDetails.GameplayCanvas.SuperButton.interactable = characterDetails.currentMax == characterDetails.Stats.Stats.maxCharge;

        eventManager.AddListener<AttackButtonClickEvent>(OnAttack);
        eventManager.AddListener<SuperAttackButtonClickEvent>(OnSuperAttack);
        eventManager.AddListener<OnCharacterDeath>(OnCharacterDeath);
    }

    public override void OnUpdate(float deltaTime)
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
                selectedAttack.Execute(targetingCharacter[currentTargetIndex], characterDetails, hasCrit =>
                {
                    targeting = false;
                    attacking = true;
                    this.hasCrit = hasCrit;
                    EventManager.Trigger(new ChargeMax(characterDetails.characterID, new List<string>() { targetingCharacter[currentTargetIndex].characterID }, 5));
                });

                if (superAttack)
                {
                    EventManager.Trigger(new ResetPlayerCharge(characterDetails.characterID));
                }
            }
        }

        if (attacking)
        {
            waitingTimer -= deltaTime;
            if (waitingTimer < 0)
            {
                if (hasCrit)
                {
                    combatStateMachine.SetState(new PlayerTurn(combatStateMachine, characterDetails));
                }
                else
                {
                    GoToNextTurn();
                }
            }
        }

    }

    public override void OnEnd()
    {
        characterDetails.VirtualCamera.Priority = 50;
        
        eventManager.RemoveListener<AttackButtonClickEvent>(OnAttack);
        eventManager.RemoveListener<OnCharacterDeath>(OnCharacterDeath);
    }
}
