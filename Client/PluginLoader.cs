using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Copier.Client
{
    public class PluginLoader
    {
        public void Load()
        {
            var pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
            var files = Directory.GetFiles(pluginDirectory, "*.dll");
            foreach (var file in files)
            {
                var pluginAssembly = Assembly.LoadFile(file);
                var existingTypes = pluginAssembly.GetTypes();
                
                var preCopyType = existingTypes
                    .FirstOrDefault(a => a.Name == "PreCopyEventListener")?.GetType();
                if (preCopyType != null)
                {
                    var preCopyObject = Activator.CreateInstance(preCopyType);
                }
                
                var postCopyType = existingTypes
                    .FirstOrDefault(a => a.Name == "PostCopyEventListener")?.GetType();

                if (postCopyType != null)
                {
                    var preCopyObject = Activator.CreateInstance(preCopyType);
                }
            }
        }
    }
}