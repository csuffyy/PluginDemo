using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace PluginContract
{
    /// <summary>
    /// 插件帮助类
    /// </summary>
    public static class PluginHelper
    {
        /// <summary>
        /// 获取程序集对应的配置文件信息
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>配置文件信息</returns>
        public static Configuration GetConfiguration(this Assembly assembly)
        {
            var configFile = assembly.Location + ".config";
            var map = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 获取插件对应的配置文件信息
        /// </summary>
        /// <param name="plugin">插件</param>
        /// <returns>配置文件信息</returns>
        public static Configuration GetConfiguration(this IPlugin plugin)
        {
            var configFile = plugin.GetType().Assembly.Location + ".config";
            var map = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 获取插件所在的目录
        /// </summary>
        /// <param name="plugin">插件</param>
        /// <returns>插件目录</returns>
        public static string GetDirectory(this IPlugin plugin)
        {
            return Path.GetDirectoryName(plugin.GetType().Assembly.Location);
        }
    }
}
