using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginContract;

namespace Plugin1
{
    [Plugin("加法", "1.1.1.0")]
    public class AddPlugin : IPlugin
    {
        public AddPlugin()
        {
            Name = "C#加法插件";
        }

        public string Name { get; private set; }

        public int Invoke(int arg1, int arg2)
        {
            var path = Path.Combine(this.GetDirectory(), "plugin1.txt");
            File.WriteAllText(path, Name);

            return arg1 + arg2;
        }
    }
}
