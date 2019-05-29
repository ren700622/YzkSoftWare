using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// decimal数据类型的精度定义
    /// </summary>
    public interface IDecimalDefine
    {

        /// <summary>
        /// 数据长度
        /// </summary>
        int DecimalLength { get; }

        /// <summary>
        /// 小数点长度
        /// </summary>
        int DotLength { get; }

    }

    public class DecimalDefine : IDecimalDefine
    {
        /// <summary>
        /// 数据长度
        /// </summary>
        public int DecimalLength { get; set; }

        /// <summary>
        /// 小数点长度
        /// </summary>
        public int DotLength { get; set; }
    }
}
