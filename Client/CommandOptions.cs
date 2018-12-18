using CommandLine;

namespace Copier.Client
{
    public class CommandOptions
    {
        [Option('s', "sourceDirectoryPath", HelpText="Parent directory where the files will be searched. If skipped, the current directory will be searched.", Required = false)]
        public string SourceDirectoryPath { get; set; }
        
        [Option('f',"fileGlobPattern", Required = true, HelpText = "Files to be searched. Accepts glob patter for pattern matching.")]
        public string FileGlobPattern { get; set; }
        
        [Option('d', "destinationDirectoryPath",Required = true, HelpText = "Destination directory path")]
        public string DestionationDirectoryPath { get; set; }
    }
}