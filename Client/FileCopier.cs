using System;
using System.IO;
using CopierPluginBase;

namespace Copier.Client
{
    class FileCopier : IFileCopier, IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
    {
        public event Action<string> PreCopyEvent = delegate {  };
        public event Action<string> PostCopy = delegate {  };
        
        private readonly ILogger _logger;

        public FileCopier(ILogger logger)
        {
            _logger = logger;
        }

        public void CopyFile(CommandOptions options, string fileName)
        {
            var absoluteSourceFilePath = Path.Combine(options.SourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(options.DestinationDirectoryPath, fileName);

            if (File.Exists(absoluteTargetFilePath) && !options.OverwriteTargetFile)
            {
                _logger.LogInfo($"{fileName} exists. Skipped the copy operation because OverwriteTargetFile is set to false.");
                return;
            }

            PreCopyEvent(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, options.OverwriteTargetFile);
            PostCopy(absoluteSourceFilePath);
        }
    }

    public interface IPostCopyEventBroadcaster
    {
        event Action<string> PostCopy;
    }

    public interface IPreCopyEventBroadcaster
    {
       event Action<string> PreCopyEvent;
    }
}