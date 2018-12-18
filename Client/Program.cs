using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Copier.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(StartWatching)
                .WithNotParsed(a =>
                {
                    Environment.Exit(1);
                });
        }

        private static void StartWatching(CommandOptions options)
        {
            var files = GetMatchingFiles(options).ToList();

            foreach (var file in files)
            {
                WatchFile(Path.GetFileName(file.Path), options.SourceDirectoryPath);
            }
            
            Console.WriteLine(files.Select(a => a.Path).Aggregate((a, b) => a + ", " + b));
            Console.ReadLine();
        }

        private static void WatchFile(string file, string directoryPath)
        {
            var watcher = new FileSystemWatcher
            {
                Path = directoryPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = file,
            };

            watcher.Changed += (sender, args) => Console.WriteLine("File has changed.");
            watcher.Renamed += (sender, args) => Console.WriteLine("File has been renamed.");

            // Start watching the file.
            watcher.EnableRaisingEvents = true;
        }
       

        private static IEnumerable<FilePatternMatch> GetMatchingFiles(CommandOptions options)
        {
            var m = new Matcher();
            m.AddInclude(options.FileGlobPattern);

            var directoryInfo = new DirectoryInfo(string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
                ? Directory.GetCurrentDirectory()
                : options.SourceDirectoryPath);
            var dirInfo = new DirectoryInfoWrapper(directoryInfo);

            var files = m.Execute(dirInfo).Files;
            return files;
        }
    }
}