using System;
using System.IO;

namespace Copier.Client
{
    class FileCopier : IFileCopier
    {
        private readonly ILogger _logger;

        public FileCopier(ILogger logger)
        {
            _logger = logger;
        }

        public Action<string> PreCopy = delegate { };
        public Action<string> PostCopy = delegate { };

        public void CopyFile(CommandOptions options, string fileName)
        {
            var absoluteSourceFilePath = Path.Combine(options.SourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(options.DestinationDirectoryPath, fileName);

            if (File.Exists(absoluteTargetFilePath) && !options.OverwriteTargetFile)
            {
                _logger.Write($"{fileName} exists. Skipped because OverwriteTargetFile is set to false.");
                return;
            }

            PreCopy(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, options.OverwriteTargetFile);
            PostCopy(absoluteSourceFilePath);
        }
    }
}