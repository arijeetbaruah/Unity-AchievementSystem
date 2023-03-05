using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Achievements
{
    [CreateAssetMenu(menuName = "Achievement Info")]
    public class AchievementInfo : ScriptableObject
    {
        public string achievementID;
        public string achievementName;
        [TextArea]
        public string achievementDescription;
        public AssetReferenceSprite icon;

        public AchievementType achievementType;

        [HideIfGroup("IsAchievementIncreament")]
        public int incrementalMinCount;
        public bool IsAchievementIncreament => achievementType != AchievementType.Incremental;
    }

    public enum AchievementType
    {
        Challenge,
        Incremental
    }
}
