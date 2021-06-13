// A simple logger class that uses Console.WriteLine by default.
// Can also do Logger.LogMethod = Debug.Log for Unity etc.
// (this way we don't have to depend on UnityEngine.DLL and don't need a
//  different version for every UnityEngine version here)
using System;
using System.Collections.Generic;

namespace Telepathy
{
    public class Logger
    {
        private static Action<string> log = Console.WriteLine;
        private static Action<string> logWarning = Console.WriteLine;
        private static Action<string> logError = Console.Error.WriteLine;

        private static Dictionary<string, Action<string>> loggerDependence = new Dictionary<string, Action<string>>();

        public static void Log(string value)
        {
            log?.Invoke(value);                  
        }

        public static void LogWarning(string value)
        {
            logWarning?.Invoke(value);
        }

        public static void LogError(string value)
        {
            logError?.Invoke(value);
        }

        public static void Log(string value, string loggerKey)
        {
            if (loggerDependence.ContainsKey(loggerKey))
                loggerDependence[loggerKey]?.Invoke(value);
        }

        public static void AddLogger(string loggerKey, Action<string> action)
        {
            if(action!=null)
            {
                if (loggerDependence.ContainsKey(loggerKey))
                {
                    loggerDependence.Remove(loggerKey);
                }

                loggerDependence.Add(loggerKey, action);
            }
        }
        
        public static void Initialize()
        {
            Logger.AddLogger("ClientConnected", (x) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Client connected {x}");
                Console.ResetColor();
            });

            Logger.AddLogger("ClientDisconnected", (x) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Client disconnected {x}");
                Console.ResetColor();
            });

            Logger.AddLogger("Message", (x) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(x);
                Console.ResetColor();
            });
        }
    }
}