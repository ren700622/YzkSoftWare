using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 字段选择范围枚举
    /// </summary>
    public enum FieldSelectRange
    {
        /// <summary>
        /// 所有字段
        /// </summary>
        All = 0,
        /// <summary>
        /// 忽略指定字段
        /// </summary>
        IgnoreFields = 1,
        /// <summary>
        /// 仅指定字段
        /// </summary>
        OnlyFields = 2
    }
}
