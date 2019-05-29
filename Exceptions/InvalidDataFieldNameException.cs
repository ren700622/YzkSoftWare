using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 无效的字段或成员名称
    /// </summary>
    public class InvalidDataFieldNameException : CodeExceptionBase
    {

        public InvalidDataFieldNameException(string fieldname)
            : base(ErrorCodes.InvalidFieldName, string.Format(LocalResource.InvalidFieldName,fieldname))
        {

        }

    }
}
