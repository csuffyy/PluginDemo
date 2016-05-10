using System.ComponentModel.Composition;

namespace PluginContract
{
    /// <summary>
    /// 插件接口
    /// </summary>
    [InheritedExport]
    public interface IPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 运算
        /// </summary>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <returns>运算结果</returns>
        int Invoke(int arg1, int arg2);
    }
}