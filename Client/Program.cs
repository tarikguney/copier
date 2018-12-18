using System;
using System.IO;
using CommandLine;

namespace Copier.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(StartWatching)
                .WithNotParsed(a =>
                {
                    Environment.Exit(1);
                });
            
            Console.WriteLine("Please press any key to exit.");
            Console.ReadLine();
        }

        private static void StartWatching(CommandOptions options)
        {
            Console.WriteLine("Watching has started...");
            WatchFile(options.FileGlobPattern, options.SourceDirectoryPath);
        }

        private static void WatchFile(string filePattern, string sourceDirectoryPath)
        {
            var watcher = new FileSystemWatcher
            {
                Path = sourceDirectoryPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = filePattern,
            };

            watcher.Changed += (sender, args) =>
            {
                if (args.ChangeType == WatcherChangeTypes.Changed)
                {
                    Console.WriteLine($"{args.Name} file has changed.");
                }
            };
            
            watcher.Renamed += (sender, args) => Console.WriteLine($"{args.OldName} has been renamed to {args.Name}.");

            // Start watching the file.
            watcher.EnableRaisingEvents = true;
        }
    }
}