using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 未发现主键错误
    /// </summary>
    public sealed class NotFoundPrimaryKeyException : Exception
    {

        public NotFoundPrimaryKeyException(Type type)
            : base(
                string.Format(
                LocalResource.NotFoundPrimaryKeyException, 
                type.GetDataModel().Name, 
                LocalResource.Update)
            )
        {

        }

    }
}
