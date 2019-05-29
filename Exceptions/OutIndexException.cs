using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 索引超出范围错误
    /// </summary>
    public class OutIndexException : Exception
    {
        public OutIndexException(int index)
            : base(string.Format(LocalResource.OutIndexException_Message, index))
        {

        }
    }
}
