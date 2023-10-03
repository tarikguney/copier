using CommandLine;

namespace Copier.Client;

internal static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandOptions>(args)
            .WithParsed(StartWatching)
            .WithNotParsed(_ => { Environment.Exit(1); });

        Console.WriteLine("Please press any key to exit.");
        Console.ReadLine();
    }

    private static void StartWatching(CommandOptions options)
    {
        ILogger logger = new ConsoleLogger();
            
        logger.LogInfo("Watching has started...");
            
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