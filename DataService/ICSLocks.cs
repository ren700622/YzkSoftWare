using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YzkSoftWare.DataService
{
    /// <summary>
    /// 锁定服务
    /// </summary>
    public interface ICSLocks : IAppService
    {
        /// <summary>
        /// 锁定指定的锁
        /// </summary>
        /// <param name="readlocks">要锁定的读锁数组</param>
        /// <param name="writelocks">要锁定的写锁数组</param>
        /// <param name="timeout">超时值</param>
        /// <param name="canceltoken">取消信号</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable Locks(IEnumerable<string> readlocks, IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken);

        /// <summary>
        /// 读锁定
        /// </summary>
        /// <param name="readlocks">要锁定的读锁数组</param>
        /// <param name="timeout">超时值</param>
        /// <param name="canceltoken">取消信号</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable LockReads(IEnumerable<string> readlocks, TimeSpan timeout, CancellationToken canceltoken);

        /// <summary>
        /// 读锁定
        /// </summary>
        /// <param name="readlocks">要锁定的读锁数组</param>
        /// <param name="timeout">超时值</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable LockReads(IEnumerable<string> readlocks, TimeSpan timeout);

        /// <summary>
        /// 读锁定
        /// </summary>
        /// <param name="readlocks">要锁定的读锁数组</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable LockReads(IEnumerable<string> readlocks);

        /// <summary>
        /// 尝试读锁定
        /// </summary>
        /// <param name="readlocks">要锁定的读锁数组</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable TryLockReads(IEnumerable<string> readlocks);

        /// <summary>
        /// 写锁定
        /// </summary>
        /// <param name="writelocks">要锁定的写锁数组</param>
        /// <param name="timeout">超时值</param>
        /// <param name="canceltoken">取消信号</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable LockWrites(IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken);

        /// <summary>
        /// 写锁定
        /// </summary>
        /// <param name="writelocks">要锁定的写锁数组</param>
        /// <param name="timeout">超时值</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable LockWrites(IEnumerable<string> writelocks, TimeSpan timeout);

        /// <summary>
        /// 写锁定
        /// </summary>
        /// <param name="writelocks">要锁定的写锁数组</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable LockWrites(IEnumerable<string> writelocks);

        /// <summary>
        /// 尝试写锁定
        /// </summary>
        /// <param name="writelocks">要锁定的写锁数组</param>
        /// <returns>返回一个IDisposable对象,当调用该对象的Dispose方法时解锁</returns>
        IDisposable TryLockWrites(IEnumerable<string> writelocks);
    }

    public abstract class CSLocksBase : AppServiceBase,ICSLocks
    {

        /// <summary>
        /// 构造指定最大和最小等待时间的构造函数
        /// 轮值等待的线程时间将在随机选取这两个数之间的数(含最大和最小数)
        /// </summary>
        /// <param name="minwaitmillseconds">最小等待毫秒数</param>
        /// <param name="maxwaitmillsecondes">最大等待毫秒数</param>
        protected CSLocksBase(int minwaitmillseconds, int maxwaitmillsecondes)
        {
            g_WaitTimeDom = new Random();
            SafeSetWaitMillSeconds(minwaitmillseconds, maxwaitmillsecondes);
        }

        /// <summary>
        /// 使用DefaultMinWaitTimeMillSeconds和DefaultMaxWaitTimeMillSeconds构造函数
        /// 轮值等待的线程时间将在随机选取这两个数之间的数(含最大和最小数)
        /// </summary>
        protected CSLocksBase()
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

        private void AbortWaitThread(object th)
        {
            try
            {
                ((System.Threading.Thread)th).Abort();
            }
            catch { }
        }

        protected abstract IDisposable TryLockFor(IEnumerable<string> readlocks, IEnumerable<string> writelocks);

        private IDisposable LocksFor(IEnumerable<string> readlocks, IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken, bool fireexpwhentimeout)
        {
            bool iserror = false;
            bool success = false;
            IDisposable result = null;
            try
            {
                if (canceltoken != CancellationToken.None)
                    canceltoken.Register(AbortWaitThread, System.Threading.Thread.CurrentThread);
                TimeSpan tmout = timeout;
                DateTime n = DateTime.Now;
                bool istimeout = false;
                result = TryLockFor(readlocks, writelocks);
                success = result != null;
                //不等待的情况
                if (timeout == TimeSpan.Zero)
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

                    result = TryLockFor(readlocks, writelocks);
                    success = result != null;
                }

                if (success)
                    return result;

                if (fireexpwhentimeout)
                    throw new TimeoutException();

                return null;
            }
            catch (ThreadAbortException) { iserror = true; return null; }
            catch (Exception e) { iserror = true; throw e; }
            finally
            {
                if (iserror && result != null)
                {
                    result.Dispose();
                }
            }
        }

        public IDisposable Locks(IEnumerable<string> readlocks, IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken)
        {
            return LocksFor(readlocks, writelocks, timeout, canceltoken, true);
        }

        public IDisposable LockReads(IEnumerable<string> readlocks, TimeSpan timeout, CancellationToken canceltoken)
        {
            return LocksFor(readlocks, null, timeout, canceltoken, true);
        }

        public IDisposable LockReads(IEnumerable<string> readlocks, TimeSpan timeout)
        {
            return LocksFor(readlocks, null, timeout, CancellationToken.None, true);
        }

        public IDisposable LockReads(IEnumerable<string> readlocks)
        {
            return LocksFor(readlocks, null, TimeSpan.MaxValue, CancellationToken.None, true);
        }

        public IDisposable TryLockReads(IEnumerable<string> readlocks)
        {
            return LocksFor(readlocks, null, TimeSpan.Zero, CancellationToken.None, false);
        }

        public IDisposable LockWrites(IEnumerable<string> writelocks, TimeSpan timeout, CancellationToken canceltoken)
        {
            return LocksFor(null, writelocks, timeout, canceltoken, true);
        }

        public IDisposable LockWrites(IEnumerable<string> writelocks, TimeSpan timeout)
        {
            return LocksFor(null, writelocks, timeout, CancellationToken.None, true);
        }

        public IDisposable LockWrites(IEnumerable<string> writelocks)
        {
            return LocksFor(null, writelocks, TimeSpan.MaxValue, CancellationToken.None, true);
        }

        public IDisposable TryLockWrites(IEnumerable<string> writelocks)
        {
            return LocksFor(null, writelocks, TimeSpan.Zero, CancellationToken.None, false);
        }

    }
}
