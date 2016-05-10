using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginContract;

namespace PluginHost
{
    public class PluginHost
    {
        /// <summary>
        /// ���Ŀ¼
        /// </summary>
        public const string PluginPath = "Plugins";

        private readonly AggregateCatalog aggregate = new AggregateCatalog();

        private readonly IPluginWatcher<IPlugin> watcher = new PluginWatcher<IPlugin>(PluginPath);

        /// <summary>
        /// �������
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<Lazy<IPlugin>> Plugins;

        public PluginHost()
        {
            if (!Directory.Exists(PluginPath))
            {
                Directory.CreateDirectory(PluginPath);
            }

            watcher.PluginsChanged += watcher_PluginsChanged;

            aggregate.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            aggregate.Catalogs.Add(new AssemblyCatalog(typeof(PluginAttribute).Assembly));
            aggregate.Catalogs.Add(new DirectoryCatalog(PluginPath));

            //��Ŀ¼�µĲ��ҲҪ����
            var childDirs = Directory.GetDirectories(PluginPath);
            foreach (var dir in childDirs)
            {
                aggregate.Catalogs.Add(new DirectoryCatalog(dir));
            }

            var container = new CompositionContainer(aggregate);
            container.ComposeParts(this);
        }

        /// <summary>
        /// ˢ��
        /// </summary>
        private void Refresh()
        {
            foreach (var catalog in aggregate.Catalogs.OfType<DirectoryCatalog>())
            {
                catalog.Refresh();
            }
        }

        void watcher_PluginsChanged(object sender, PluginsChangedEventArgs<IPlugin> e)
        {
            Refresh();
            OnPluginsChanged();
        }

        #region PluginsChanged

        /// <summary>
        /// ����и���
        /// </summary>
        public event EventHandler<PluginsChangedEventArgs<IPlugin>> PluginsChanged;

        protected virtual void OnPluginsChanged()
        {
            var handler = PluginsChanged;
            if (handler != null)
            {
                var e = new PluginsChangedEventArgs<IPlugin> { AvailablePlugins = Plugins.Select(x => x.Value) };
                handler(this, e);
            }
        }

        #endregion
    }
}