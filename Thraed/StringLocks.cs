using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YzkSoftWare.Thraed
{
    public class StringLocks
    {

        /// <summary>
        /// 构造指定最大和最小等待时间的构造函数
        /// 轮值等待的线程时间将在随机选取这两个数之间的数(含最大和最小数)
        /// </summary>
        /// <param name="minwaitmillseconds">最小等待毫秒数</param>
        /// <param name="maxwaitmillsecondes">最大等待毫秒数</param>
        public StringLocks(int minwaitmillseconds, int maxwaitmillsecondes)
        {
            p_Lks = new StringLockInfoCollection();
            g_WaitTimeDom = new Random();
            SafeSetWaitMillSeconds(minwaitmillseconds, maxwaitmillsecondes);
        }

        /// <summary>
        /// 使用DefaultMinWaitTimeMillSeconds和DefaultMaxWaitTimeMillSeconds构造函数
        /// 轮值等待的线程时间将在随机选取这两个数之间的数(含最大和最小数)
        /// </summary>
        public StringLocks()
            : this(DefaultMinWaitTimeMillSeconds,
                DefaultMaxWaitTimeMillSeconds)
        {
        }

        #region 公有常数
        /// <summary>
        /// 缺省最大等待毫秒数=100
        /// </summary>
        public const int DefaultMaxWaitTimeMillSeconds = 100;
        /// <summary>
        /// 缺省最小等待毫秒数=10
        /// </summary>
        public const int DefaultMinWaitTimeMillSeconds = 10;

        #endregion

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random g_WaitTimeDom = null;
        /// <summary>
        /// 最大等待毫秒数
        /// </summary>
        private int g_MaxWaitMillSeconds = 0;
        /// <summary>
        /// 最小等待毫秒数
        /// </summary>
        private int g_MinWaitMillSeconds = 0;

        private void SafeSetWaitMillSeconds(int minwaitmillseconds, int maxwaitmillsecondes)
        {
            int min = minwaitmillseconds;
            if (min < 0)
                min = DefaultMinWaitTimeMillSeconds;
            int max = maxwaitmillsecondes;
            if (max < 0)
                max = DefaultMaxWaitTimeMillSeconds;
            if (min > max)
            {
                int x = min;
                min = max;
                max = x;
            }
            if (min == max)
            {
                min = DefaultMinWaitTimeMillSeconds;
                max = DefaultMaxWaitTimeMillSeconds;
            }
            g_MaxWaitMillSeconds = max;
            g_MinWaitMillSeconds = min;
        }

        private StringLockInfoCollection p_Lks;

        private void AbortWaitThread(object th)
        {
            try
            {
                ((System.Threading.Thread)th).Abort();
            }
            catch { }
        }

        private IDisposable GetStringLocks(IEnumerable<string> readlocks, IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken,bool fireexpwhentimeout)
        {
            bool iserror = false;
            bool success = false;
            List<StringLockInfo> locks = new List<StringLockInfo>();
            try
            {
                if (canceltoken != CancellationToken.None)
                    canceltoken.Register(AbortWaitThread, System.Threading.Thread.CurrentThread);
                TimeSpan tmout = timeout;
                DateTime n = DateTime.Now;
                bool istimeout = false;
                success = p_Lks.TryGetKeys(readlocks, writelocks, locks);
                //不等待的情况
                if (timeout == TimeSpan.MinValue ||
                    timeout == TimeSpan.Zero)
                {
                    istimeout = true;
                }
                while (!success && !istimeout)
                {
                    //睡眠随机毫秒片段
                    System.Threading.Thread.Sleep(g_WaitTimeDom.Next(g_MinWaitMillSeconds, g_MaxWaitMillSeconds));

                    if (timeout != TimeSpan.MaxValue)
                    {
                        //减去睡眠时间
                        DateTime sn = DateTime.Now;
                        tmout -= sn.Subtract(n);
                        n = sn;
                    }

                    //是否超时
                    if (timeout != TimeSpan.MaxValue
                        && tmout.Ticks <= 0)
                    {
                        istimeout = true;
                        break;
                    }

                    success = p_Lks.TryGetKeys(readlocks, writelocks, locks);
                }

                if (success)
                    return new StringArrayKeyLocks(locks.ToArray());

                if (fireexpwhentimeout)
                    throw new TimeoutException();

                return null;
            }
            catch (ThreadAbortException) { iserror = true; return null; }
            catch (Exception e) { iserror = true; throw e; }
            finally
            {
                if (iserror && success)
                {
                    foreach (var s in locks)
                        s.Dispose();
                }
            }
        }

        public IDisposable GetReadStringLocks(IEnumerable<string> readlocks, TimeSpan timeout, CancellationToken canceltoken)
        {
            return GetStringLocks(readlocks, null, timeout, canceltoken, true);
        }

        public IDisposable GetReadStringLocks(IEnumerable<string> readlocks, TimeSpan timeout)
        {
            return GetStringLocks(readlocks, null, timeout, CancellationToken.None, true);
        }

        public IDisposable GetReadStringLocks(IEnumerable<string> readlocks)
        {
            return GetStringLocks(readlocks, null, TimeSpan.MaxValue, CancellationToken.None, true);
        }

        public IDisposable TryGetReadStringLocks(IEnumerable<string> readlocks)
        {
            return GetStringLocks(readlocks, null, TimeSpan.MinValue, CancellationToken.None, false);
        }

        public IDisposable GetWriteStringLocks(IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken)
        {
            return GetStringLocks(null, writelocks, timeout, canceltoken, true);
        }

        public IDisposable GetWriteStringLocks(IEnumerable<string> writelocks, TimeSpan timeout)
        {
            return GetStringLocks(null, writelocks, timeout, CancellationToken.None, true);
        }

        public IDisposable GetWriteStringLocks(IEnumerable<string> writelocks)
        {
            return GetStringLocks(null, writelocks, TimeSpan.MaxValue, CancellationToken.None, true);
        }

        public IDisposable TryGetWriteStringLocks(IEnumerable<string> writelocks)
        {
            return GetStringLocks(null, writelocks, TimeSpan.MinValue, CancellationToken.None, false);
        }

        public IDisposable GetStringLocks(IEnumerable<string> readlocks, IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken)
        {
            return GetStringLocks(readlocks, writelocks, timeout, canceltoken, true);
        }

        public IDisposable GetStringLocks(IEnumerable<string> readlocks, IEnumerable<string> writelocks, TimeSpan timeout)
        {
            return GetStringLocks(readlocks, writelocks, timeout, CancellationToken.None, true);
        }

        public IDisposable GetStringLocks(IEnumerable<string> readlocks, IEnumerable<string> writelocks)
        {
            return GetStringLocks(readlocks, writelocks, TimeSpan.MaxValue, CancellationToken.None, true);
        }

        public IDisposable TryGetStringLocks(IEnumerable<string> readlocks, IEnumerable<string> writelocks)
        {
            return GetStringLocks(readlocks, writelocks, TimeSpan.MinValue, CancellationToken.None, false);
        }
    }

    internal sealed class StringArrayKeyLocks : IDisposable
    {

        internal StringArrayKeyLocks(IEnumerable<StringLockInfo> ks)
        {
            p_Ks = ks;
        }

        private IEnumerable<StringLockInfo> p_Ks;


        void IDisposable.Dispose()
        {
            if (p_Ks != null)
            {
                foreach (var x in p_Ks)
                    x.Dispose();
            }
        }
    }
}
