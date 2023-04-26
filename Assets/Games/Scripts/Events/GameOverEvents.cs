using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvents : GameEvent
{
    public string txt;

    public GameOverEvents(string txt)
    {
        this.txt = txt;
    }
}
