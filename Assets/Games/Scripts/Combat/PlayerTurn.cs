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

    private AttackCommand selectedAttack = null;
    private Spell selectedSpell = null;

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

        selectedAttack.isPlayer = true;
        selectedAttack.OnMoveLeftEvent = MoveLeft;
        selectedAttack.OnMoveRightEvent = MoveRight;
        selectedAttack.OnAttackEvent = OnAttack;

        Log.Print("On Player Attack", FilterLog.Game);
    }

    public void OnMagicOpen(MagicButtonClickEvent @event)
    {
        characterDetails.GameplayMagicCanvas.gameObject.SetActive(true);
        characterDetails.GameplayCanvas.CloseAll();

        characterDetails.knownSpells.ForEach(spell =>
        {
            characterDetails.GameplayMagicCanvas.SpawnBtn(spell, CastSpell);
        });
    }

    public void CastSpell(Spell spell)
    {
        Log.Print($"Spell is casted: {spell.spellName}");
        characterDetails.GameplayCanvas.CloseAll();
        characterDetails.GameplayMagicCanvas.gameObject.SetActive(false);

        if (spell is SingleTarget)
        {
            targetingCharacter = new List<CharacterDetails>(combatStateMachine.activeAICharacter);
            currentTargetIndex = 0;
            targeting = true;
            selectedSpell = spell;
            ShowTarget();

            SingleTarget stspell = selectedSpell as SingleTarget;

            stspell.IsPlayer = true;
            stspell.OnMoveLeftEvent = MoveLeft;
            stspell.OnMoveRightEvent = MoveRight;

            stspell.OnAttackEvent = () =>
            {
                attacking = true;
                characterDetails.UseMana(spell.spellCost);
                selectedSpell.Execute(new List<CharacterDetails>()
                {
                    targetingCharacter[currentTargetIndex],
                }, 
                characterDetails,
                hasOneMore =>
                {
                    hasCrit = hasOneMore;
                });
            };
        }
        else
        {
            attacking = true;
            characterDetails.UseMana(spell.spellCost);
            spell.Execute(combatStateMachine.activeAICharacter, characterDetails, hasOneMore =>
            {
                hasCrit = hasOneMore;
            });
        }
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

        selectedAttack.isPlayer = true;
        selectedAttack.OnMoveLeftEvent = MoveLeft;
        selectedAttack.OnMoveRightEvent = MoveRight;
        selectedAttack.OnAttackEvent = OnAttack;

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

    private void MoveRight()
    {
        currentTargetIndex++;
        if (currentTargetIndex >= targetingCharacter.Count)
        {
            currentTargetIndex = 0;
        }
        ShowTarget();
    }

    private void MoveLeft()
    {
        currentTargetIndex--;
        if (currentTargetIndex < 0)
        {
            currentTargetIndex = targetingCharacter.Count - 1;
        }
        ShowTarget();
    }

    private void OnAttack()
    {
        selectedAttack.Execute(targetingCharacter[currentTargetIndex], characterDetails, hasCrit =>
                {
                    targeting = false;
                    attacking = true;
                    this.hasCrit = hasCrit;
                    EventManager.Trigger(new ChargeMax(
                        characterDetails.characterID,
                        new List<string>() { targetingCharacter[currentTargetIndex].characterID }, 5)
                    );
                });

        if (superAttack)
        {
            EventManager.Trigger(new ResetPlayerCharge(characterDetails.characterID));
        }
    }

    public override void OnStart()
    {
        characterDetails.VirtualCamera.Priority = 100;
        attacking = false;
        superAttack = false;
        targeting = false;
        characterDetails.GameplayCanvas.OpenAll();

        characterDetails.GameplayCanvas.SuperButton.interactable = characterDetails.currentMax == characterDetails.Stats.Stats.maxCharge;

        StartStatusEffect();

        eventManager.AddListener<AttackButtonClickEvent>(OnAttack);
        eventManager.AddListener<MagicButtonClickEvent>(OnMagicOpen);
        eventManager.AddListener<SuperAttackButtonClickEvent>(OnSuperAttack);
        eventManager.AddListener<OnCharacterDeath>(OnCharacterDeath);
    }

    public void StartStatusEffect()
    {
        characterDetails.StatusEffect.ForEach(status =>
        {
            StartStatusEffect(status);
        });
    }

    public void StartStatusEffect(CombatStatus status)
    {
        switch (status)
        {
            case CombatStatus.Poisoned:
            case CombatStatus.Bleeding:
            case CombatStatus.Burning:
                characterDetails.TakeDamage(5);
                break;
            case CombatStatus.Down:
                characterDetails.RemoveStatusEffect(status);
                break;
            case CombatStatus.Confused:
                var targets = combatStateMachine.activeOrderedCharacter;
                int index = UnityEngine.Random.Range(0, targets.Count);

                selectedAttack.Execute(targets[index], characterDetails, hasCrit =>
                {
                    targeting = false;
                    attacking = true;
                    this.hasCrit = hasCrit;
                    EventManager.Trigger(new ChargeMax(
                        characterDetails.characterID,
                        new List<string>() { targets[index].characterID }, 5)
                    );
                });
                GoToNextTurn();

                break;
            case CombatStatus.Paralyzed:
                GoToNextTurn();
                break;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (targeting)
        {
            selectedAttack?.Update();
            selectedSpell?.Update();
        }

        if (attacking)
        {
            waitingTimer -= deltaTime;
            if (waitingTimer < 0)
            {
                if (hasCrit)
                {
                    EventManager.Trigger(new OneMoreEvent());
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
        eventManager.RemoveListener<MagicButtonClickEvent>(OnMagicOpen);
        eventManager.RemoveListener<OnCharacterDeath>(OnCharacterDeath);
    }
}
