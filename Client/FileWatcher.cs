namespace Copier.Client;

internal class FileWatcher : IFileWatcher
{
    private readonly IFileCopier _fileCopier;
    private readonly ILogger _logger;

    public FileWatcher(IFileCopier fileCopier, ILogger logger)
    {
        _fileCopier = fileCopier;
        _logger = logger;
    }
        
    public void Watch(CommandOptions options)
    {
        var watcher = new FileSystemWatcher
        {
            Path = options.SourceDirectoryPath,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            Filter = options.FileGlobPattern
        };

        watcher.Changed += (_, args) =>
        {
            if (args.ChangeType != WatcherChangeTypes.Changed || string.IsNullOrWhiteSpace(args.Name)) 
                return;
                
            if (options.Verbose)
            {
                _logger.LogInfo($"{args.Name} file has changed.");
            }

            _fileCopier.CopyFile(args.Name);
        };

        watcher.Renamed += (_, args) =>
        {
            if (string.IsNullOrWhiteSpace(args.Name))
                return;
            
            if (options.Verbose)
            {
                _logger.LogInfo($"{args.OldName} has been renamed to {args.Name}.");
            }

            _fileCopier.CopyFile(args.Name);
        };

        watcher.EnableRaisingEvents = true;
    }
}