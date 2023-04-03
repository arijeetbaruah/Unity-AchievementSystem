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

    public class ChargeMax : GameEvent
    {
        public string attackerID;
        public List<string> targetID;
        public float amount;

        public ChargeMax(string attackerID, List<string> targetID, float amount)
        {
            this.attackerID = attackerID;
            this.targetID = targetID;
            this.amount = amount;
        }
    }
}
