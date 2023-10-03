using System;
using CopierPluginBase;

namespace SamplePlugin;

public class SamplePostCopyEventListener: IPostCopyEventListener
{
    public void OnPostCopy(string filePath)
    {
        Console.WriteLine("SamplePostCopyEventListener is executed.");
    }
}