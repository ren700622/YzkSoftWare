using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 具有长整型数据类型的主键的数据记录接口
    /// </summary>
    public interface IDataId
    {

        /// <summary>
        /// 数据主键的值
        /// </summary>
        long Id { get; set; }

    }
}
