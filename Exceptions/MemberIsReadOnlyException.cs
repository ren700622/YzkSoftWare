using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 只读错误
    /// </summary>
    public class MemberIsReadOnlyException : Exception
    {

        public MemberIsReadOnlyException(string member) : base(string.Format(LocalResource.MemberIsReadOnlyException, member)) { }

    }
}
