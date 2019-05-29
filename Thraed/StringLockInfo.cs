using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Thraed
{
    internal sealed class StringLockInfo : IDisposable
    {

        internal StringLockInfoCollection Owner { get; set; }

        public bool IsRead { get; set; }

        public bool IsDisposed { get; set; }

        public string Key { get; set; }

        public override string ToString()
        {
            if (IsDisposed)
            {
                if (IsRead)
                    return string.Format(LocalResource.StringLockInfo_R_D, Key);
                else
                    return string.Format(LocalResource.StringLockInfo_W_D, Key);
            }
            else
            {
                if (IsRead)
                    return string.Format(LocalResource.StringLockInfo_R, Key);
                else
                    return string.Format(LocalResource.StringLockInfo_W, Key);
            }

        }

        public void Dispose()
        {
            if (!IsDisposed && Owner != null)
                Owner.Free(this);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }
    }

    [System.Runtime.Remoting.Contexts.Synchronization]
    internal sealed class StringLockInfoCollection : ContextBoundObject
    {

        internal StringLockInfoCollection()
        {
            //p_Frees = new List<StringLockInfo>();
            p_Locks = new List<StringLockInfo>();
        }

        //private IList<StringLockInfo> p_Frees;

        private IList<StringLockInfo> p_Locks;

        private StringLockInfo GetFromFree(string key, bool isread)
        {
            //StringLockInfo a = p_Frees.FirstOrDefault();
            //if (a != null)
            //{
            //    p_Frees.Remove(a);
            //    a.IsDisposed = false;
            //    a.IsRead = isread;
            //    a.Key = key;
            //    a.Owner = this;
            //}
            //else
            //{
            //    a = new StringLockInfo()
            //    {
            //        IsDisposed = false,
            //        IsRead = isread,
            //        Key = key,
            //        Owner = this
            //    };
            //}
            //return a;
            return new StringLockInfo()
            {
                IsDisposed = false,
                IsRead = isread,
                Key = key,
                Owner = this
            };
        }

        private bool TryGet(IEnumerable<string> readlocks, IEnumerable<string> writelocks, IList<StringLockInfo> locks)
        {
            if (readlocks != null)
            {
                foreach (var k in readlocks)
                {
                    if (!StringLockHelper.TryLockRead(p_Locks, k))
                        return false;
                }
            }
            if (writelocks != null)
            {
                foreach (var k in writelocks)
                {
                    if (!StringLockHelper.TryLockWrite(p_Locks, k))
                        return false;
                }
            }
            if (readlocks != null)
            {
                foreach (var k in readlocks)
                {
                    StringLockInfo a = GetFromFree( k, true);
                    p_Locks.Add(a);
                    locks.Add(a);
                }
            }
            if (writelocks != null)
            {
                foreach (var k in writelocks)
                {
                    StringLockInfo a = GetFromFree(k, false);
                    p_Locks.Add(a);
                    locks.Add(a);
                }
            }
            return true;
        }

        internal void Free(StringLockInfo item)
        {
            p_Locks.Remove(item);
            item.IsDisposed = true;
            item.Owner = null;
            //if (!p_Locks.Remove(item))
            //    throw new Exception(LocalResource.StringLockInfoCollection_Error_1);
            //item.IsDisposed = true;
            //item.Owner = null;
            //p_Frees.Add(item);
        }

        internal bool TryGetWriteKeys(IEnumerable<string> keys, IList<StringLockInfo> locks)
        {
            return TryGet(null, keys, locks);
        }

        internal bool TryGetReadKeys(IEnumerable<string> keys, IList<StringLockInfo> locks)
        {
            return TryGet(keys, null, locks);
        }

        internal bool TryGetKeys(IEnumerable<string> readlocks, IEnumerable<string> writelocks, IList<StringLockInfo> locks)
        {
            return TryGet(readlocks, writelocks, locks);
        }
    }

    internal static class StringLockHelper
    {

        /// <summary>
        /// 是否可锁定写
        /// </summary>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="threadid"></param>
        /// <returns></returns>
        public static bool TryLockWrite(IEnumerable<StringLockInfo> values, string key)
        {
            var f = values.FirstOrDefault(v => WriteIsLocked(v, key));
            return f == null;
        }

        /// <summary>
        /// 是否可锁定读
        /// </summary>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool TryLockRead(IEnumerable<StringLockInfo> values, string key)
        {
            var f = values.FirstOrDefault(v => ReadIsLock(v, key));
            return f == null;
        }

        private static bool WriteIsLocked(StringLockInfo value, string key)
        {
            //key已锁定,已锁定
            if (string.Equals(value.Key, key, StringComparison.OrdinalIgnoreCase))
                return true;
            //已有key的子键锁定,已锁定
            if (value.Key.IndexOf(key + ".") == 0)
                return true;
            //key是锁定键的子键,已锁定
            if (key.IndexOf(value.Key + ".") == 0)
                return true;
            return false;
        }

        private static bool ReadIsLock(StringLockInfo value, string key)
        {
            //key已锁定,如果不是读锁则已锁定
            if (string.Equals(value.Key, key, StringComparison.OrdinalIgnoreCase))
                return !value.IsRead;
            //已有key的子键锁定,如果不是读锁则已锁定
            if (value.Key.IndexOf(key + ".") == 0)
                return !value.IsRead;
            //key是锁定键的子键,如果不是读锁则已锁定
            if (key.IndexOf(value.Key + ".") == 0)
                return !value.IsRead;
            return false;


        }
    }

}
