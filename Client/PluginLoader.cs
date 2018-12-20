using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CopierPluginBase;

namespace Copier.Client
{
    public class PluginLoader : IPluginLoader
    {
        private readonly ILogger _debugLogger;
        private readonly List<Type> _preCopyListeners = new List<Type>();
        private readonly List<Type> _postCopyListeners = new List<Type>();

        private bool ShowDebugMessages { get; }


        public PluginLoader(ILogger debugLogger, bool showDebugMessages = false) : this()
        {
            _debugLogger = debugLogger;
            ShowDebugMessages = showDebugMessages && debugLogger != null;
        }

        public PluginLoader()
        {
            var pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
            var assemblyFiles = Directory.GetFiles(pluginDirectory, "*.dll");

            foreach (var assemblyName in assemblyFiles)
            {
                var pluginAssembly = Assembly.LoadFile(assemblyName);

                if (ShowDebugMessages)
                {
                    _debugLogger.Write($"Loaded {assemblyName}");
                }

                var existingTypes = pluginAssembly.GetTypes();

                var postCopyListenerTypes =
                    existingTypes.Where(a => a.IsClass && a.IsSubclassOf(typeof(IPostCopyEventListener))).ToList();
                _postCopyListeners.AddRange(postCopyListenerTypes);

                var preCopyListenerTypes =
                    existingTypes.Where(a => a.IsClass && a.IsSubclassOf(typeof(IPreCopyEventListener))).ToList();
                _preCopyListeners.AddRange(preCopyListenerTypes);

                // If enabled, logging debug messages for the found types in the iterated assembly.
                if (ShowDebugMessages)
                {
                    _debugLogger.Write($"Found the following PostCopy types from plugin {assemblyName}:");
                    _debugLogger.Write(string.Join("\n", postCopyListenerTypes.Select(a => a.Name).ToArray()));

                    _debugLogger.Write($"Found the following PreCopy types from plugin {assemblyName}:");

                    // Used LINQ for fun.
                    var preCopyTypeNames = (from a in preCopyListenerTypes
                        select a.Name).ToArray();
                    _debugLogger.Write(string.Join("\n", preCopyTypeNames));
                }
            }
        }

        public void Subscribe(IPreCopyEventBroadcaster pre, IPostCopyEventBroadcaster post)
        {
            _preCopyListeners.ForEach(a =>
            {
                var listenerObject = (IPreCopyEventListener) Activator.CreateInstance(a);
                pre.PreCopyEvent += listenerObject.OnPreCopy;
            });

            _postCopyListeners.ForEach(a =>
            {
                var listenerObject = (IPostCopyEventListener) Activator.CreateInstance(a);
                post.PostCopy += listenerObject.OnPostCopy;
            });
        }
    }
}