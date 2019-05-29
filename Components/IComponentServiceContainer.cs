using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YzkSoftWare.Data;
using YzkSoftWare.Database;
using YzkSoftWare.DataService;

namespace YzkSoftWare.Components
{
    /// <summary>
    /// 服务组件容器接口
    /// </summary>
    public interface IComponentServiceContainer : IDisposable
    {

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        IsolationLevel? TransactionLevel { get; set; }

        /// <summary>
        /// 添加服务组件到容器中
        /// </summary>
        /// <param name="service"></param>
        void AddComponentService(IComponentService service);

        /// <summary>
        /// 保存所有组件中的修改
        /// </summary>
        void SaveChanged();

        /// <summary>
        /// 添加锁
        /// </summary>
        /// <param name="lockKeys"></param>
        void AddLockKeys(IEnumerable<string> readLockKeys, IEnumerable<string> writeLockKeys);

        /// <summary>
        /// 添加组件方法的锁定键
        /// </summary>
        /// <param name="service">组件服务</param>
        /// <param name="methodName">方法</param>
        void AddComponentServiceMethodLockKey(IComponentService service, string methodName);

        /// <summary>
        /// 开始锁
        /// <param name="timeout">超时值,超过该时间引发异常</param>
        /// <param name="cancel">取消信号</param>
        /// </summary>
        /// <returns></returns>
        IDisposable BeginLockKeys(TimeSpan timeout, CancellationToken cancel);
    }

    public class ComponentServiceContainer : IComponentServiceContainer
    {

        public ComponentServiceContainer()
        {
            p_ComponentServices = new List<IComponentService>();
            p_ComponentDatabaseContainer = DataServiceProvider.CreateService<IComponentDatabaseContainer>();
            p_Checkers = new Dictionary<Type, IUpdateChecker>();
            p_ReadLockKeys = new List<string>();
            p_WriteLockKeys = new List<string>();
        }

        private IList<string> p_ReadLockKeys;

        private IList<string> p_WriteLockKeys;

        private IList<IComponentService> p_ComponentServices;

        private IComponentDatabaseContainer p_ComponentDatabaseContainer;

        private IDictionary<Type, IUpdateChecker> p_Checkers;

        private IDisposable p_CurrentLockDispose = null;

        private IUpdateChecker GetChecker(Type type)
        {
            foreach (var m in p_Checkers)
            {
                if (m.Key == type)
                    return m.Value;
            }
            var n = type.GetTypeUpdateChecker();
            p_Checkers.Add(type, n);
            return n;
        }

        private void CheckError(IUpdateContext upd, IEnumerable<IUpdateContext> grouptocommit, IDataSelector dataselector)
        {
            if (upd is IDataUpdateContext)
            {
                IDataUpdateContext upd2 = upd as IDataUpdateContext;
                var checker = GetChecker(upd2.ModalType);
                if (checker != null)
                {
                    var e = checker.CheckErrors(upd2, grouptocommit, dataselector);
                    if (e != null)
                        throw e;
                }
            }
        }

        private void UpdateToDatabase(IUpdateContext upd,IDatabaseAdptor db,IEnumerable<IUpdateContext> grouptocommit, IDataSelector dataselector)
        {
            if (!upd.SuccessUpdate)
            {
                var sql = upd.Update(db.DbTypeName);
                if (sql != null)
                {
                    if (upd.UpdateDataNoty != null)
                        upd.UpdateDataNoty.BeforeChange(upd, grouptocommit, dataselector);
                    upd.SuccessCommitToDatabase(db.CommitToDatabase(sql));
                    if (upd.UpdateDataNoty != null)
                        upd.UpdateDataNoty.OnChanged(upd, grouptocommit, dataselector);
                }
            }
        }

        private void SaveChanged(IComponentService service)
        {
            var a = service.UpdateContexts;
            if (a != null)
            {
                foreach (var b in a)
                {
                    CheckError(b, a, service.DataSelector);
                    UpdateToDatabase(b, p_ComponentDatabaseContainer.GetDatabaseAdptor(service), a, service.DataSelector);
                }
            }
        }

        public void AddComponentService(IComponentService service)
        {
            if (!p_ComponentServices.Contains(service))
            {
                p_ComponentServices.Add(service);
                service.DataSelector = p_ComponentDatabaseContainer.GetDataSelector(service);
            }
        }

        public void SaveChanged()
        {
            //生成数据库适配器
            foreach (var x in p_ComponentServices)
                p_ComponentDatabaseContainer.GetDatabaseAdptor(x);

            //开始事务
            var xxx = p_ComponentDatabaseContainer.GeneratesAdptor;
            if (xxx != null)
            {
                IsolationLevel? al = TransactionLevel;
                foreach (var g in xxx)
                {
                    if (al != null && al.HasValue)
                        g.BeginTransaction(al.Value);
                    else
                        g.BeginTransaction();
                }  
            }

            try
            {
                //保存更新
                foreach (var x in p_ComponentServices)
                {
                    SaveChanged(x);
                    x.SuccessUpdate();
                }
                //提交事务
                if (xxx != null)
                {
                    foreach (var g in xxx)
                        g.CommitTran();
                }
            }
            catch (Exception e)
            {
                //回滚事务
                if (xxx != null)
                {
                    foreach (var g in xxx)
                        g.RollbackTran();
                }
                throw new Exception(e.Message, e);
            }
        }

        public void AddLockKeys(IEnumerable<string> readLockKeys, IEnumerable<string> writeLockKeys)
        {
            if (readLockKeys != null)
            {
                foreach (var a in readLockKeys)
                {
                    if (!p_ReadLockKeys.Contains(a))
                        p_ReadLockKeys.Add(a);
                }
            }
            if (writeLockKeys != null)
            {
                foreach (var a in writeLockKeys)
                {
                    if (!p_WriteLockKeys.Contains(a))
                        p_WriteLockKeys.Add(a);
                }
            }
        }

        public void AddComponentServiceMethodLockKey(IComponentService service, string methodName)
        {
            IEnumerable<string> rd;
            IEnumerable<string> wd = service.GetMethodLockKeys(methodName, out rd);
            AddLockKeys(rd, wd);
        }

        public IDisposable BeginLockKeys(TimeSpan timeout, CancellationToken cancel)
        {
            if (p_ReadLockKeys.Count > 0 || p_WriteLockKeys.Count > 0)
            {
                IDisposable d;
                using (ICSLocks service = DataServiceProvider.CreateService<ICSLocks>())
                {
                    d = service.Locks(p_ReadLockKeys, p_WriteLockKeys, timeout, cancel);
                }
                p_ReadLockKeys.Clear();
                p_WriteLockKeys.Clear();
                return d;
            }
            else
                return ObjectExtendDescription.EmptyDispose;

        }

        public void Dispose()
        {
            p_ReadLockKeys.Clear();
            p_WriteLockKeys.Clear();
            p_Checkers.Clear();
            foreach (var x in p_ComponentServices)
                x.Dispose();
            p_ComponentServices.Clear();
            p_ComponentDatabaseContainer.Dispose();
            if (p_CurrentLockDispose != null)
                p_CurrentLockDispose.Dispose();
            p_CurrentLockDispose = null;
        }

        public IsolationLevel? TransactionLevel { get; set; }
    }

}
