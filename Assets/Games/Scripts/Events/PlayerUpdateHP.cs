using System;
using System.Collections.Generic;

namespace Game.Events
{
    public class CreatePlayerHUD : GameEvent
    {
        public CharacterDetails characterDetails;

        public CreatePlayerHUD(CharacterDetails characterDetails)
        {
            this.characterDetails = characterDetails;
        }
    }

    public class CritEvent : GameEvent
    {
        public CritEvent()
        {
        }
    }

    public class OneMoreEvent : GameEvent
    {
        public OneMoreEvent()
        {
        }
    }

    public class IsShowingTextEvent : GameEvent
    {
        public Action<bool> callback;

        public IsShowingTextEvent(Action<bool> callback)
        {
            this.callback = callback;
        }
    }

    public class PlayerUpdateHP : GameEvent
    {
        public string playerID;
        public float amount;

        public PlayerUpdateHP(string playerID, float amount)
        {
            this.playerID = playerID;
            this.amount = amount;
        }
    }

    public class PlayerUpdateMana : GameEvent
    {
        public string playerID;
        public float amount;

        public PlayerUpdateMana(string playerID, float amount)
        {
            this.playerID = playerID;
            this.amount = amount;
        }
    }

    public class PlayerUpdateCharge : GameEvent
    {
        public string playerID;
        public float amount;

        public PlayerUpdateCharge(string playerID, float amount)
        {
            this.playerID = playerID;
            this.amount = amount;
        }
    }

    public class ResetPlayerCharge : GameEvent
    {
        public string playerID;

        public ResetPlayerCharge(string playerID)
        {
            this.playerID = playerID;
        }
    }

    public class ChargeMax : GameEvent
    {
        public string attackerID;
        public List<string> targetID;
        public int amount;

        public ChargeMax(string attackerID, List<string> targetID, int amount)
        {
            this.attackerID = attackerID;
            this.targetID = targetID;
            this.amount = amount;
        }
    }
}
