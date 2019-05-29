using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Exceptions
{
    /// <summary>
    /// 锁定失败的错误
    /// </summary>
    public class LockFailException : Exception
    {

        public LockFailException(bool isread, string key)
            : base(string.Format(LocalResource.LockFailException, isread ? LocalResource.ReadLock : LocalResource.WriteLock, key))
        {
            LockKey = key;
        }

        public string LockKey { get; private set; }
    }
}
