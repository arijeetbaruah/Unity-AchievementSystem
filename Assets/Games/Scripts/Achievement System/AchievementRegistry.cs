using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Achievements
{
    [CreateAssetMenu(menuName = "Achievement Registry")]
    public class AchievementRegistry : ScriptableObject
    {
        public List<AchievementInfo> registries;
    }
}
