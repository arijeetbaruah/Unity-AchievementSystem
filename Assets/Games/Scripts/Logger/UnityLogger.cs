using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Logger
{
    public class UnityLogger : ILogger
    {
        private Dictionary<FilterLog, string> colorMap = new Dictionary<FilterLog, string>()
        {
            { FilterLog.Default, "#FFFFFF" },
            { FilterLog.Game, "#42ecf5" },
            { FilterLog.Network, "#00FFFF" },
            { FilterLog.GameEvent, "#4287f5" },
            { FilterLog.Error, "#FF0000" },
        };

        public void Print(object message, FilterLog filterLog)
        {
            (string filters, string logColor) = GetColorAndFilter(filterLog);

            Debug.Log($"<color={logColor}>{filters} {message}</color>");
        }

        public void Warning(object message, FilterLog filterLog)
        {
            (string filters, string logColor) = GetColorAndFilter(filterLog);

            Debug.LogWarning($"<color={logColor}>{filters} {message}</color>");
        }

        public void Error(object message, FilterLog filterLog)
        {
            (string filters, string logColor) = GetColorAndFilter(filterLog);

            Debug.LogError($"<color={logColor}>{filters} {message}</color>");
        }

        private static bool IsDistinctValue(Enum value)
        {
            int current = Convert.ToInt32(value) >> 1;
            while (current > 0)
            {
                if ((Convert.ToInt32(value) & current) != 0)
                {
                    return false;
                }
                current >>= 1;
            }

            return true;
        }

        private (string, string) GetColorAndFilter(FilterLog filterLog)
        {
            Enum v = (Enum)Convert.ChangeType(filterLog, typeof(Enum));
            Array array = Enum.GetValues(typeof(FilterLog));
            IEnumerable<FilterLog> setFlags = array
                .Cast<FilterLog>()
                .Where(c => v.HasFlag(c) && IsDistinctValue(c));

            string filters = string.Join("", setFlags.Where(c => c != FilterLog.Default).Select(flag => $"[{flag}]"));

            string logColor = colorMap[ setFlags.Last() ].ToString();

            return (filters, logColor);
        }
    }
}
