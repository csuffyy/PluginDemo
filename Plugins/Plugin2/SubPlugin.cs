using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PluginContract;

namespace Plugin2
{
    [Plugin("减法", "2.2.2.0")]
    public class SubPlugin : IPlugin
    {
        public SubPlugin()
        {
            Name = "C#减法插件（自带参数）";
        }

        public string Name { get; private set; }

        public int Invoke(int arg1, int arg2)
        {
            string p = this.GetConfiguration().AppSettings.Settings["Parameter"].Value;
            int parameter;
            int.TryParse(p, out parameter);

            return (arg1 - arg2) * parameter;
        }
    }
}