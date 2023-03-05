using Game.Achievements;
using Game.Achievements.Events;
using Game.Events;
using Game.Logger;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Diagnostics;

namespace Game.Service
{
    public class AchievementSystem : IService
    {
        private Dictionary<string, AchievementInfo> achivementRegistry;

        private Dictionary<string, int> achievementProgression;
        private Dictionary<string, bool> achievementCompleted;

        private const string AchievementProgressionKey = "Achievement Progression";
        private const string AchievementCompletedKey = "Achievement Completed";

        public AchievementSystem()
        {
            Load();

            achivementRegistry = new Dictionary<string, AchievementInfo>();

            var operationHandle = Addressables.LoadAssetAsync<AchievementRegistry>("achievement");
            operationHandle.Completed += _ =>
            {
                AchievementRegistry registry = operationHandle.Result;
                foreach (var item in registry.registries)
                {
                    achivementRegistry.Add(item.achievementID, item);
                }
            };
        }

        public IEnumerable<AchievementInfo> GetAllAchievement()
        {
            return achivementRegistry.Values;
        }

        public void UpdateAchievementPrograssion(string achievementID, int increament = 1)
        {
            if (achivementRegistry.TryGetValue(achievementID, out var achievement))
            {
                switch (achievement.achievementType)
                {
                    case AchievementType.Challenge:
                        if (!achievementCompleted.ContainsKey(achievementID))
                        {
                            achievementCompleted.Add(achievementID, true);
                            ServiceRegistry.Get<EventManager>().TriggerEvent(new AchievementUnlockedEvent(achievementID));
                        }
                        break;
                    case AchievementType.Incremental:
                        if (!achievementCompleted.ContainsKey(achievementID))
                        {
                            if (achievementProgression.ContainsKey(achievementID) && achievementProgression[achievementID] < achivementRegistry[achievementID].incrementalMinCount)
                            {
                                achievementProgression[achievementID] += increament;
                            }

                            if (achievementProgression[achievementID] < achivementRegistry[achievementID].incrementalMinCount)
                            {
                                achievementCompleted.Add(achievementID, true);
                                ServiceRegistry.Get<EventManager>().TriggerEvent(new AchievementUnlockedEvent(achievementID));
                            }
                        }
                        break;
                }
            }

            Save();
        }

        public bool GetAchievementStatus(string achievementID)
        {
            if (achievementCompleted.TryGetValue(achievementID, out bool status))
            {
                return status;
            }

            return false;
        }

        public int GetAchievementPProgression(string achievementID)
        {
            if (achievementProgression.TryGetValue(achievementID, out int progress))
            {
                return progress;
            }

            return 0;
        }

        private void Load()
        {
            string completeData = PlayerPrefs.GetString(AchievementCompletedKey);

            if (string.IsNullOrEmpty(completeData))
            {
                achievementCompleted = new Dictionary<string, bool>();
            }
            else
            {
                achievementCompleted = JsonConvert.DeserializeObject<Dictionary<string, bool>>(completeData);
            }

            string progress = PlayerPrefs.GetString(AchievementProgressionKey);
            if (string.IsNullOrEmpty(progress))
            {
                achievementProgression = new Dictionary<string, int>();
            }
            else
            {
                achievementProgression = JsonConvert.DeserializeObject<Dictionary<string, int>>(progress);
            }
        }

        private void Save()
        {
            PlayerPrefs.SetString(AchievementProgressionKey, JsonConvert.SerializeObject(achievementProgression));
            PlayerPrefs.SetString(AchievementCompletedKey, JsonConvert.SerializeObject(achievementCompleted));
        }
    }
}
