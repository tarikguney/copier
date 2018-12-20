using System.Collections.Generic;
using System.Linq.Expressions;
using CommandLine;
using CommandLine.Text;

namespace Copier.Client
{
    public class CommandOptions
    {
        [Option('s', "sourceDirectoryPath",
            HelpText =
                "Parent directory where the files will be searched. If skipped, the current directory will be searched.",
            Required = false)]
        public string SourceDirectoryPath { get; set; }

        [Option('f', "fileGlobPattern", Required = true,
            HelpText = "Files to be searched. Accepts glob patter for pattern matching.")]
        public string FileGlobPattern { get; set; }

        [Option('d', "destinationDirectoryPath", Required = true, HelpText = "Destination directory path")]
        public string DestinationDirectoryPath { get; set; }

        [Option('o', "overwriteTargetFiles", Default = false, Required = false,
            HelpText = "If passed true, copier will overwrite existing files at the target location.")]
        public bool OverwriteTargetFile { get; set; }

        [Option('v', "verbose", Default = false, Required = false,
            HelpText = "If passed true, more information will be outputted to the console.")]
        public bool Verbose { get; set; }

        [Option('e', "debug", Default = false, Required = false, HelpText = "Shows debug information.")]
        public bool Debug { get; set; }

        [Option('t', "delay", Default = 0, Required = false, HelpText = "Delays copy operation for a given time in seconds.")]
        public int Delay { get; set; }
        
        [Usage]
        public static IEnumerable<Example> Examples => new List<Example>()
        {
            new Example("Starts the copier", new UnParserSettings() {PreferShortName = true},
                new CommandOptions
                {
                    SourceDirectoryPath = "C:/Users/MyDocuments/Images",
                    FileGlobPattern = "*.jpg",
                    DestinationDirectoryPath = "C:/Users/MyDocuments/NewImages"
                }),
            new Example("Starts the copier and overwrites the target files.",
                new UnParserSettings() {PreferShortName = true},
                new CommandOptions
                {
                    SourceDirectoryPath = "C:/Users/MyDocuments/Images",
                    FileGlobPattern = "*.jpg",
                    DestinationDirectoryPath = "C:/Users/MyDocuments/NewImages",
                    OverwriteTargetFile = true
                }),
            new Example("Starts the copier and overwrites the target files and outputs verbose messages.",
                new UnParserSettings() {PreferShortName = true},
                new CommandOptions
                {
                    SourceDirectoryPath = "C:/Users/MyDocuments/Images",
                    FileGlobPattern = "*.jpg",
                    DestinationDirectoryPath = "C:/Users/MyDocuments/NewImages",
                    OverwriteTargetFile = true,
                    Verbose = true
                })
        };
    }
}