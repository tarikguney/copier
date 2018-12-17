using CommandLine;

namespace Copier.Client
{
    public class CommandOptions
    {
        [Option('b', "basePath", HelpText="Parent directory where the files will be searched. If skipped, the current directory will be searched.", Required = false)]
        public string BasePath { get; set; }
        
        [Option('f',"filePattern", Required = true, HelpText = "Files to be searched. Accepts glob patter for pattern matching.")]
        public string SearchPattern { get; set; }
        
        [Option('d', "destinationPath",Required = true, HelpText = "Destination directory path")]
        public string DestionationPath { get; set; }
       
    }
}