using System;

namespace Game.Logger
{
    public interface ILogger
    {
        public void Print(object message, FilterLog filterLog);
        public void Warning(object message);
        public void Error(object message);
    }

    public static class Log
    {
        private static ILogger logger;

        public static void Initialization(ILogger logger)
        {
            Log.logger = logger;
        }

        public static void Print(object message, FilterLog filterLog = FilterLog.Default) => logger.Print(message, filterLog);
        public static void Warning(object message) => logger.Warning(message);
        public static void Error(object message) => logger.Error(message);
    }

    [Flags]
    public enum FilterLog
    {
        Default = 0,
        GameEvent = 1,
        Game = 1 << 1,
        Network = 1 << 2,
        Error = 1 << 3,
    }
}
