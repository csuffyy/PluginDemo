using System;
using System.Collections.Generic;
using PluginContract;

namespace PluginHost
{
    /// <summary>
    /// Watch for changes to a plugin directory for a specific MEF Import type.
    /// <para>Keeps a list of last seen exports and exposes a change event</para>
    /// </summary>
    /// <typeparam name="T">Plugin type. Plugins should contain classes implementing this type and decorated with [Export(typeof(...))]</typeparam>
    public interface IPluginWatcher<T> : IDisposable where T : IPlugin
    {
        /// <summary>
        /// Available Exports matching type <typeparamref name="T"/> have changed
        /// </summary>
        event EventHandler<PluginsChangedEventArgs<T>> PluginsChanged;

        /// <summary>
        /// Last known Exports matching type <typeparamref name="T"/>.
        /// </summary>
        IEnumerable<T> CurrentlyAvailable { get; }
    }
}