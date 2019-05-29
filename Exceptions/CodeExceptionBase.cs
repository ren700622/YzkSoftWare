using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    public abstract class CodeExceptionBase : Exception
    {
        protected CodeExceptionBase(int errorcode, string errmsg)
            : base(errmsg)
        {
            p_ErrorCode = errorcode;
        }

        private int p_ErrorCode;

        public int ErrorCode
        {
            get { return p_ErrorCode; }
        }
    }

    public class UnkownCodeException : CodeExceptionBase
    {
        public UnkownCodeException(int errorcode, string errmsg)
            : base(errorcode, errmsg)
        {

        }
    }
}
