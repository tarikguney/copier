namespace Copier.Client;

public class QueuedFileCopier : IFileCopier, IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
{
    public event Action<string> PreCopyEvent = delegate {};
    public event Action<string> PostCopyEvent = delegate {};
        
    private readonly IFileCopier _fileCopier;
    private readonly ILogger _logger;
    private readonly CommandOptions _options;

    private readonly HashSet<string> _fileNameQueue = new HashSet<string>();
    private Task _copyTask;

    public QueuedFileCopier(IFileCopier fileCopier, ILogger logger, CommandOptions options)
    {
        _fileCopier = fileCopier;
        _logger = logger;
        _options = options;

        if (_options.Debug)
        {
            logger.LogInfo("Delay option has been specified. QueuedFileCopier is chosen as the copier strategy.");
        }
    }

    public void CopyFile(string fileName)
    {
        if (_copyTask == null)
        {
            _copyTask = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(_options.Delay));
                if (_options.Verbose || _options.Debug)
                {
                    _logger.LogInfo($"{_options.Delay} milliseconds have passed. The copy operation has started...");
                }

                PreCopyEvent("");

                foreach (var item in _fileNameQueue)
                {
                    _fileCopier.CopyFile(item);
                }

                PostCopyEvent("");

                _copyTask = null;

                if (_options.Verbose || _options.Debug)
                {
                    _logger.LogInfo($"The copy operation has finished...");
                    _logger.LogInfo("The file queue has been emptied.");
                }
            });
        }

        if (!_fileNameQueue.Contains(fileName))
        {
            if (_options.Verbose || _options.Debug)
            {
                _logger.LogInfo(
                    $"{fileName} has been added to the file queue and will be copied over in {_options.Delay} milliseconds.");
            }

            _fileNameQueue.Add(fileName);
        }
        else if (_options.Debug)
        {
            _logger.LogInfo($"{fileName} exists in the file queue, thereby skipped.");
        }
    }
}