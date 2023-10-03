using System.IO;

namespace Copier.Client
{
    class FileWatcher : IFileWatcher
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
                Filter = options.FileGlobPattern,
                IncludeSubdirectories = options.Recursive
            };

            watcher.Changed += (sender, args) =>
            {
                if (args.ChangeType != WatcherChangeTypes.Changed) return;
                
                if (options.Verbose)
                {
                    _logger.LogInfo($"{args.Name} file has changed.");
                }

                _fileCopier.CopyFile(args.Name);
            };

            watcher.Renamed += (sender, args) =>
            {
                if (options.Verbose)
                {
                    _logger.LogInfo($"{args.OldName} has been renamed to {args.Name}.");
                }

                _fileCopier.CopyFile(args.Name);
            };

            watcher.EnableRaisingEvents = true;
        }
    }
}