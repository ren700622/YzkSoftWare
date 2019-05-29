using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据库标识字段的增长定义
    /// </summary>
    public interface IDbGeneratedDefine
    {
        /// <summary>
        /// 起始值
        /// </summary>
        int StartValue { get; }

        /// <summary>
        /// 步长
        /// </summary>
        int Step { get; }
    }

    public sealed class DbGeneratedDefine : IDbGeneratedDefine
    {
        /// <summary>
        /// 起始值
        /// </summary>
        public int StartValue { get; set; }

        /// <summary>
        /// 步长
        /// </summary>
        public int Step { get; set; }
    }
}
