using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace Copier.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<CommandOptions, ConfigFileCommandOptions>(args)
                .MapResult(
                    (CommandOptions o) => StartWatchingAndReturnExitCode(o),
                    (ConfigFileCommandOptions co) => StartWatchingWithConfigurationFile(co),
                    err => 1);
            
//                .WithParsed<CommandOptions>(StartWatching)
//                .WithParsed<ConfigFileCommandOptions>(StartWatchingWithConfigurationFile)
//                .WithNotParsed(a => { Environment.Exit(1); });

            Console.WriteLine("Please press any key to exit.");
            Console.ReadLine();
        }

        private static int StartWatchingWithConfigurationFile(ConfigFileCommandOptions options)
        {
            ILogger logger = new ConsoleLogger();
            
            if (File.Exists(options.ConfigFilePath))
            {
                var configContent = File.ReadAllLines(options.ConfigFilePath);
                Parser.Default.ParseArguments<CommandOptions>(configContent)
                    .WithParsed(StartWatching)
                    .WithNotParsed(a => { Environment.Exit(1); });
            }
            else
            {
                logger.LogError($"Cannot find {options.ConfigFilePath}! Please make sure the file exists in the given location.");
            }

            return 0;
        }
        
        private static int StartWatchingAndReturnExitCode(CommandOptions options)
        {
            StartWatching(options);
            return 0;
        }

        private static void StartWatching(CommandOptions options)
        {
            ILogger logger = new ConsoleLogger();
            
            logger.LogInfo("Watching has started...");

            options.SourceDirectoryPath = string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
                ? Directory.GetCurrentDirectory()
                : options.SourceDirectoryPath;
            
            IPluginLoader loader = new PluginLoader(logger, options.Debug);
            
            var fileCopier = new FileCopier(logger, options);
            IFileCopier copier = fileCopier;
            
            if (options.Delay > 0)
            {
                copier = new QueuedFileCopier(fileCopier, logger, options);
            }

            IFileWatcher fileWatcher = new FileWatcher(copier, logger);
            
            loader.Subscribe((IPreCopyEventBroadcaster) copier, (IPostCopyEventBroadcaster) copier);
            
            fileWatcher.Watch(options);
        }
    }
}