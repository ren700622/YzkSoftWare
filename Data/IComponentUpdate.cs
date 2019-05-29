using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 组件数据更新接口
    /// </summary>
    public interface IComponentUpdate
    {

        /// <summary>
        /// 待更新的数据
        /// </summary>
        IEnumerable<IUpdateContext> UpdateContexts { get; }

        /// <summary>
        /// 已成功更新数据
        /// </summary>
        void SuccessUpdate();
    }
}
