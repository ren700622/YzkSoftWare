using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    public class NotSupportException : CodeExceptionBase
    {

        public NotSupportException(int errcode, string msg)
            : base(errcode,msg)
        {

        }

    }
}
