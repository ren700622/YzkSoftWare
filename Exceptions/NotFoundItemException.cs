using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 未找到元素的错误
    /// </summary>
    public class NotFoundItemException : Exception
    {

        public NotFoundItemException(string description)
            : base(string.Format("{0}[{1}]", LocalResource.NotFound, description))
        {

        }

    }
}
