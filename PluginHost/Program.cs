using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PluginContract;

namespace PluginHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new PluginHost();

            host.PluginsChanged += (sender, eventArgs) =>
            {
                Console.WriteLine("插件有更新……");
                Console.WriteLine();

                ShowPlugins(eventArgs.AvailablePlugins);
                Console.WriteLine();
            };

            ShowPlugins(host.Plugins.Select(x => x.Value));

            Console.ReadLine();
        }

        private static void ShowPlugins(IEnumerable<IPlugin> plugins)
        {
            Console.WriteLine("插件总数：{0}", plugins.Count());
            Console.WriteLine("---------------------------------------------");

            var orderedPlugins = plugins.AsParallel().OrderBy(x => x.Name);
            foreach (var plugin in orderedPlugins)
            {
                var att = plugin.GetType().GetCustomAttribute(typeof(PluginAttribute)) as PluginAttribute;

                Console.WriteLine(plugin.Name);
                if (att != null)
                {
                    Console.WriteLine("Version: {0}", att.Version);
                }

                Console.Write("12 VS 4 = ");
                Console.Write(plugin.Invoke(12, 4));

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
