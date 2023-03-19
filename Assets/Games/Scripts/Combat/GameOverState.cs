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
        string txt = playerWon ? "Won" : "Lost";
        Log.Print($"Game Over - You {txt}");
    }

    public void OnUpdate(float deltaTime)
    {
    }

    public void OnEnd()
    {
    }

}
