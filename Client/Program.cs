using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.VisualBasic.CompilerServices;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Client.dll <BasePath> <File in GlobPattern> <Destination>");
            }
            else
            {
                var m = new Matcher();
                m.AddInclude(args[1]);
                var directoryInfo = new DirectoryInfo(args[0]);
                var dirInfo = new DirectoryInfoWrapper(directoryInfo);

                var files = m.Execute(dirInfo).Files;
                Console.WriteLine(files.Select(a => a.Path).Aggregate((a, b) => a + ", " + b));
            }
        }
    }
}