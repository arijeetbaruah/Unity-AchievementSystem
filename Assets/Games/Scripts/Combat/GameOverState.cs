using Game.Events;
using Game.Logger;
using Game.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IState
{
    public bool playerWon;

    public GameOverState(bool playerWon)
    {
        this.playerWon = playerWon;
    }

    public void OnStart()
    {
        string txt = $"Game Over - You {(playerWon ? "Won" : "Lost")}";
        Log.Print(txt, FilterLog.GameEvent);
        EventManager.Trigger(new GameOverEvents(txt));
    }

    public void OnUpdate(float deltaTime)
    {
    }

    public void OnEnd()
    {
    }

}
