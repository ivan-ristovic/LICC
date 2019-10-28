using System;
using System.Linq;

namespace AST.Common
{
    public static class Logger
    {
        public static LogLevel LogLevel { get; set; } = LogLevel.Debug;


        public static void LogMany(string module, params string[] messages)
            => LogMany(LogLevel.Debug, module, messages);

        public static void LogMany(LogLevel level, string module, params string[] messages)
        {
            if (level > LogLevel)
                return;

            PrintTimestamp(null);
            PrintApplicationInfo(module);
            PrintLevel(level);
            PrintLogMessage();
            foreach (string message in messages.Where(m => !string.IsNullOrWhiteSpace(m)))
                PrintLogMessage(message);
            PrintLogMessage();
        }

        public static void Log(string message, DateTime? timestamp = null)
            => Log(LogLevel.Debug, message, null, timestamp);

        public static void Log(string module, string message, DateTime? timestamp = null)
            => Log(LogLevel.Debug, message, module, timestamp);

        public static void Log(LogLevel level, string message, string module = null, DateTime? timestamp = null)
        {
            if (level > LogLevel)
                return;

            PrintTimestamp(timestamp);
            PrintLevel(level);
            PrintApplicationInfo(module);
            PrintLogMessage();
            PrintLogMessage(message);
            PrintLogMessage();
        }

        private static void PrintLevel(LogLevel level)
        {
            switch (level) {
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }
            Console.Write($"[{level}] ");
            Console.ResetColor();
        }

        private static void PrintTimestamp(DateTime? timestamp = null)
        {
            Console.Write($"[{(timestamp ?? DateTime.Now):yyyy-MM-dd HH:mm:ss zzz}] ");
        }

        private static void PrintApplicationInfo(string module = null)
        {
            if (string.IsNullOrWhiteSpace(module))
                return;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"[{module}]");
            Console.ResetColor();
        }

        private static void PrintLogMessage(string message = "")
        {
            if (!(message is null))
                Console.WriteLine(string.IsNullOrWhiteSpace(message) ? "" : "| " + message.Trim());
        }


    }

    public enum LogLevel
    {
        Debug = 8,
        Info = 4,
        Warning = 2,
        Error = 1,
        Critical = 0
    }
}
