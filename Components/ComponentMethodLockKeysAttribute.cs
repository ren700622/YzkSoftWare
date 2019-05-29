using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Components
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ComponentMethodLockKeysAttribute : Attribute
    {

        public ComponentMethodLockKeysAttribute(string lockKey, bool isread) { LockKey = lockKey; IsRead = isread; }

        /// <summary>
        /// 锁定的键
        /// </summary>
        public string LockKey { get; private set; }

        /// <summary>
        /// 是否是读锁
        /// </summary>
        public bool IsRead { get; private set; }
    }
}
