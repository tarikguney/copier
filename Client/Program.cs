using System;
using System.IO;
using CommandLine;

namespace Copier.Client
{
    class Program
    {
        public static event Action ApplicationStarted = delegate { };

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(StartWatching)
                .WithNotParsed(a => { Environment.Exit(1); });

            Console.WriteLine("Please press any key to exit.");
            Console.ReadLine();
        }

        private static void StartWatching(CommandOptions options)
        {
            ApplicationStarted();

            Console.WriteLine("Watching has started...");

            options.SourceDirectoryPath = string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
                ? Directory.GetCurrentDirectory()
                : options.SourceDirectoryPath;

            IFileCopier copier = new FileCopier();
            ILogger logger = new ConsoleLogger();
            IFileWatcher fileWatcher = new FileWatcher(copier, logger);
            fileWatcher.Watch(options);
        }
    }
}