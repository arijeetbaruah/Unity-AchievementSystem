using System;

namespace Game.Logger
{
    public interface ILogger
    {
        public void Print(object message, FilterLog filterLog);
        public void Warning(object message, FilterLog filterLog);
        public void Error(object message, FilterLog filterLog);
    }

    public static class Log
    {
        private static ILogger logger;

        public static void Initialization(ILogger logger)
        {
            Log.logger = logger;
        }

        public static void Print(object message, FilterLog filterLog = FilterLog.Default) => logger.Print(message, filterLog);
        public static void Warning(object message, FilterLog filterLog) => logger.Warning(message, filterLog);
        public static void Error(object message, FilterLog filterLog) => logger.Error(message, filterLog);
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
