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
            var files = GetMatchingFiles(options);
            
            Console.WriteLine(files.Select(a => a.Path).Aggregate((a, b) => a + ", " + b));
        }

        private static IEnumerable<FilePatternMatch> GetMatchingFiles(CommandOptions options)
        {
            var m = new Matcher();
            m.AddInclude(options.SearchPattern);

            var directoryInfo = new DirectoryInfo(string.IsNullOrWhiteSpace(options.BasePath)
                ? Directory.GetCurrentDirectory()
                : options.BasePath);
            var dirInfo = new DirectoryInfoWrapper(directoryInfo);

            var files = m.Execute(dirInfo).Files;
            return files;
        }
    }
}