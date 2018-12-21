using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Copier.Client
{
    public class QueuedFileCopier: IFileCopier
    {
        private readonly IFileCopier _fileCopier;
        private readonly ILogger _logger;
        private CommandOptions _commandOptions;
        
        private readonly HashSet<string> _fileNameQueue = new HashSet<string>();
        private Timer _copyTimer;

        public QueuedFileCopier(IFileCopier fileCopier, ILogger logger)
        {
            _fileCopier = fileCopier;
            _logger = logger;
        }

        public void CopyFile(CommandOptions options, string fileName)
        {
            if (_copyTimer == null)
            {
                _commandOptions = options;
                _copyTimer = new Timer();
                _copyTimer.Elapsed += TimerOnElapsed;
            }

            if (!_fileNameQueue.Contains(fileName))
            {
                _fileNameQueue.Add(fileName);
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var fileName in _fileNameQueue)
            {
                _fileCopier.CopyFile(_commandOptions, fileName);
            }
            
            _fileNameQueue.Clear();
        }
    }
}