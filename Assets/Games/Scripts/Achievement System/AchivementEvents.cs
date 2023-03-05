using Game.Events;

namespace Game.Achievements.Events
{
    public class AchievementUnlockedEvent : GameEvent
    {
        public string achievementID;

        public AchievementUnlockedEvent(string achievementID)
        {
            this.achievementID = achievementID;
        }
    }
}
