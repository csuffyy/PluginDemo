using System;
using System.Collections.Generic;
using PluginContract;

namespace PluginHost
{
    /// <summary>
    /// Event arguments relating to a change in available MEF Export types.
    /// </summary>
    public class PluginsChangedEventArgs<T> : EventArgs where T : IPlugin
    {
        /// <summary>
        /// Last known Exports matching type <typeparamref name="T"/>.
        /// </summary>
        public IEnumerable<T> AvailablePlugins { get; set; }
    }
}