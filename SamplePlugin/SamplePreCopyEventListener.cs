using System;
using CopierPluginBase;

namespace SamplePlugin
{
    public class SamplePreCopyEventListener: IPreCopyEventListener
    {
        public void OnPreCopy(string filePath)
        {
            Console.WriteLine("SamplePreCopyEventListener is executed.");
        }
    }
}