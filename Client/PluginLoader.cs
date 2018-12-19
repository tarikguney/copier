using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using CopierPluginBase;

namespace Copier.Client
{
    public class PluginLoader : IPluginLoader
    {
        private readonly ILogger _logger;
        private readonly bool _showDebugMessages;
        private readonly List<Type> _preCopyListeners = new List<Type>();
        private readonly List<Type> _postCopyListeners = new List<Type>();


        public PluginLoader(ILogger logger, bool showDebugMessages = false) : this()
        {
            _logger = logger;
            _showDebugMessages = showDebugMessages;
        }

        public PluginLoader()
        {
            var pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
            var assemblyFiles = Directory.GetFiles(pluginDirectory, "*.dll");

            foreach (var assemblyName in assemblyFiles)
            {
                var pluginAssembly = Assembly.LoadFile(assemblyName);

                if (_logger != null && _showDebugMessages)
                {
                    _logger.Write($"Loaded {assemblyName}");
                }

                var postCopyListenerTypes = pluginAssembly
                    .GetTypes().Where(a => a.IsClass && a.IsSubclassOf(typeof(IPostCopyEventListener)));
                _postCopyListeners.AddRange(postCopyListenerTypes);

                var preCopyListenerTypes = pluginAssembly
                    .GetTypes().Where(a => a.IsClass && a.IsSubclassOf(typeof(IPreCopyEventListener)));
                _preCopyListeners.AddRange(preCopyListenerTypes);
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