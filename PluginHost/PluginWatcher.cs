using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using PluginContract;

namespace PluginHost
{
    /// <summary>
    /// Watch for changes to a plugin directory for a specific MEF Import type.
    /// <para>Keeps a list of last seen exports and exposes a change event</para>
    /// </summary>
    /// <typeparam name="T">Plugin type. Plugins should contain classes implementing this type and decorated with [Export(typeof(...))]</typeparam>
    public class PluginWatcher<T> : IPluginWatcher<T> where T : IPlugin
    {
        private readonly object compositionLock = new object();
        private AggregateCatalog catalog;

        private CompositionContainer container;
        private FileSystemWatcher fsw;
        private AssemblyCatalog localCatalog;
        private DirectoryCatalog pluginCatalog;

        public PluginWatcher(string pluginDirectory)
        {
            if (!Directory.Exists(pluginDirectory))
            {
                throw new Exception("Can't watch \"" + pluginDirectory + "\", might not exist or not enough permissions");
            }

            CurrentlyAvailable = new T[0];
            fsw = new FileSystemWatcher(pluginDirectory, "*.dll");
            SetupFileWatcher();

            try
            {
                pluginCatalog = new DirectoryCatalog(pluginDirectory);
                localCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
                catalog = new AggregateCatalog();
                catalog.Catalogs.Add(localCatalog);
                catalog.Catalogs.Add(pluginCatalog);
                container = new CompositionContainer(catalog, false);
                container.ExportsChanged += ExportsChanged;
            }
            catch
            {
                Dispose(true);
                throw;
            }

            ReadLoadedPlugins();
        }

        ~PluginWatcher()
        {
            Dispose(true);
        }

        public event EventHandler<PluginsChangedEventArgs<T>> PluginsChanged;
        public IEnumerable<T> CurrentlyAvailable { get; protected set; }

        protected virtual void OnPluginsChanged()
        {
            var handler = PluginsChanged;
            if (handler != null)
            {
                handler(this, new PluginsChangedEventArgs<T> { AvailablePlugins = CurrentlyAvailable });
            }
        }

        private void SetupFileWatcher()
        {
            fsw.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName |
                                NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Security;

            fsw.Changed += FileAddedOrRemoved;
            fsw.Created += FileAddedOrRemoved;
            fsw.Deleted += FileAddedOrRemoved;
            fsw.Renamed += FileRenamed;

            fsw.EnableRaisingEvents = true;
        }

        private void ExportsChanged(object sender, ExportsChangeEventArgs e)
        {
            lock (compositionLock)
            {
                if (e.AddedExports.Any() || e.RemovedExports.Any())
                {
                    ReadLoadedPlugins();
                }
            }
        }

        private void ReadLoadedPlugins()
        {
            CurrentlyAvailable = container.GetExports<T>().Select(y => y.Value);
            OnPluginsChanged();
        }

        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            RefreshPlugins();
        }

        void FileAddedOrRemoved(object sender, FileSystemEventArgs e)
        {
            RefreshPlugins();
        }

        private void RefreshPlugins()
        {
            try
            {
                var cat = pluginCatalog;
                if (cat == null) { return; }
                lock (compositionLock)
                {
                    cat.Refresh();
                }
            }
            catch (ChangeRejectedException rejex)
            {
                Console.WriteLine("Could not update plugins: " + rejex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (!disposing) return;

            var fsw = Interlocked.Exchange(ref this.fsw, null);
            if (fsw != null) fsw.Dispose();

            var plg = Interlocked.Exchange(ref pluginCatalog, null);
            if (plg != null) plg.Dispose();

            var con = Interlocked.Exchange(ref container, null);
            if (con != null) con.Dispose();

            var loc = Interlocked.Exchange(ref localCatalog, null);
            if (loc != null) loc.Dispose();

            var cat = Interlocked.Exchange(ref catalog, null);
            if (cat != null) cat.Dispose();
        }
    }

}