using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 对象为null错误
    /// </summary>
    public class ObjectIsNullException : Exception
    {

        public ObjectIsNullException(string objdescription)
            : base(string.Format(LocalResource.ObjectIsNullException, objdescription))
        {

        }

    }
}
