using System;

namespace PluginContract
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute
    {
        public PluginAttribute(string name) : this(name, "1.0.0.0") { }

        public PluginAttribute(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; private set; }

        public string Version { get; private set; }
    }
}