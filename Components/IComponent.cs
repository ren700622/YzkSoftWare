using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Components
{
    /// <summary>
    /// 数据组件接口
    /// </summary>
    public interface IComponent
    {

        /// <summary>
        /// 数据组件Id
        /// </summary>
        string ComponentId { get; }

        /// <summary>
        /// 当前应用名称
        /// </summary>
        string CurrentAppName { get; set; }

    }
}
