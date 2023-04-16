namespace Game.Events
{
    class OnSignedInEvent : GameEvent
    {
        public string playerId;
        public string accessToken;

        public OnSignedInEvent(string playerId, string accessToken)
        {
            this.playerId = playerId;
            this.accessToken = accessToken;
        }
    }
}
