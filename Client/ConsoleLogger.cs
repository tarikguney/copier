using System;
using System.Drawing;

namespace Copier.Client
{
    class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + message);
            Console.ResetColor();
        }

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: " + message);
            Console.ResetColor();
        }
        
        public void LogDebug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DEBUG: " + message);
            Console.ResetColor();
        }
    }
}