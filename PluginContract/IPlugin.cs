using System.ComponentModel.Composition;

namespace PluginContract
{
    /// <summary>
    /// ����ӿ�
    /// </summary>
    [InheritedExport]
    public interface IPlugin
    {
        /// <summary>
        /// �������
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="arg1">����1</param>
        /// <param name="arg2">����2</param>
        /// <returns>������</returns>
        int Invoke(int arg1, int arg2);
    }
}