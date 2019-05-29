using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 数据模型错误
    /// </summary>
    public sealed class DataModelException : Exception
    {

        public DataModelException(string err) : base(err) { }

    }
}
