using System;
using System.IO;

namespace Copier.Client
{
    class FileCopier : IFileCopier
    {
        public Action<string> PreCopy = delegate { };
        public Action<string> PostCopy = delegate { };

        public void CopyFile(string sourceDirectoryPath, string fileName, string targetDirectoryPath,
            bool overwriteTargetFile)
        {
            var absoluteSourceFilePath = Path.Combine(sourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(targetDirectoryPath, fileName);

            if (File.Exists(absoluteTargetFilePath) && !overwriteTargetFile) return;

            PreCopy(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, overwriteTargetFile);
            PostCopy(absoluteSourceFilePath);
        }
    }
}