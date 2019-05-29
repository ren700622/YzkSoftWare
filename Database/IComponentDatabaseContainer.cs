using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Components;
using YzkSoftWare.Configuration;
using YzkSoftWare.Data;
using YzkSoftWare.Encrypt;
using YzkSoftWare.Exceptions;

namespace YzkSoftWare.Database
{

    /// <summary>
    /// 组件数据库适配器器容器接口
    /// </summary>
    public interface IComponentDatabaseContainer : IDisposable
    {

        /// <summary>
        /// 获取数据组件的数据库适配器器
        /// </summary>
        /// <param name="component">要获取数据库适配器器的组件</param>
        /// <param name="appname">该组件所在的应用名称</param>
        /// <returns>数据库适配器器</returns>
        IDatabaseAdptor GetDatabaseAdptor(IComponent component);

        /// <summary>
        /// 获取数据组件数据获取接口
        /// </summary>
        /// <param name="component">要获取数据获取接口的组件</param>
        /// <param name="appname">该组件所在的应用名称</param>
        /// <returns>数据获取接口</returns>
        IDataSelector GetDataSelector(IComponent component);

        /// <summary>
        /// 已生成的数据库适配器
        /// </summary>
        IEnumerable<IDatabaseAdptor> GeneratesAdptor { get; }

    }

    /// <summary>
    /// 组件数据库适配器器容器
    /// </summary>
    public class ComponentDatabaseContainer : IComponentDatabaseContainer
    {

        public ComponentDatabaseContainer()
        {
            p_DatabaseAdptors = new List<DatabaseAdptorItem>();
        }

        public static readonly string AppComponentDbSettingSectionName = "appComponentSetting";

        private static readonly string pAk = "ComponentDatabaseContainer";

        public static string HelperEncryptConnectionString(string cnnt)
        {
            if (!string.IsNullOrEmpty(cnnt))
            {
                var s = NormalServiceProvider.CreateSimpleEncrypt();
                s.EncryptKey = pAk;
                return s.Encrypt(cnnt);
            }
            return null;
        }

        private IList<DatabaseAdptorItem> p_DatabaseAdptors;

        private string DecryptConnectionString(string cnnt)
        {
            if (!string.IsNullOrEmpty(cnnt))
            {
                var s = NormalServiceProvider.CreateSimpleEncrypt();
                s.EncryptKey = pAk;
                return s.Decrypt(cnnt);
            }
            return null;
        }

        private DatabaseAdptorItem SelectFromBuffer(string dbtype,string cnnt)
        {
            return p_DatabaseAdptors.FirstOrDefault(f => string.Equals(f.Adptor.DbTypeName, dbtype, StringComparison.OrdinalIgnoreCase)
                && string.Equals(f.CnntString, cnnt, StringComparison.OrdinalIgnoreCase));
        }

        private DatabaseAdptorItem GetDatabaseAdptorItemForComponent(IComponent component, string appname,bool ismaster)
        {
            lock (p_DatabaseAdptors)
            {
                string dbtype;
                string connectstring = GetConnectStringForComponent(component, appname, ismaster, out dbtype);

                DatabaseAdptorItem a = SelectFromBuffer(dbtype, connectstring);
                if (a == null)
                {
                    IDatabaseAdptor b = CreateNewDatabaseAdptorForComponent(dbtype, connectstring);
                    if (b == null)
                        throw new NotFoundDatabaseAdptorException(component.ComponentId);
                    a = new DatabaseAdptorItem()
                    {
                        CnntString = connectstring,
                        Adptor = b,
                        DataSelector = new DataSelector(b)
                    };
                    p_DatabaseAdptors.Add(a);
                }
                return a;
            }
        }

        private ComponentDbConnectStringSelectionElement GetElementForComponent(IComponent component,string appname)
        {
            ComponentDbConnectStringSelectionElement select = null;
            AppComponentDatabaseAdptorConfiguration appdbs = ConfigurationManager.GetSection(AppComponentDbSettingSectionName) as AppComponentDatabaseAdptorConfiguration;
            if (appdbs != null)
            {
                select = appdbs.DefaultAppConnectString;
                if (appdbs.Dbs != null)
                {
                    foreach (var item in appdbs.Dbs)
                    {
                        AppComponentDatabaseAdptor adptor = item as AppComponentDatabaseAdptor;
                        //adptor.AppName==空(表示所有app都可用)或adptor.AppName==appname
                        if ((string.IsNullOrEmpty(adptor.AppName) || string.Equals(adptor.AppName, appname, StringComparison.OrdinalIgnoreCase))
                            && adptor.ComponentDatabaeAdptors != null)
                        {
                            foreach (var item2 in adptor.ComponentDatabaeAdptors)
                            {
                                ComponentDbConnectStringSelectionElement db = item2 as ComponentDbConnectStringSelectionElement;
                                if (string.IsNullOrEmpty(db.ComponentId))
                                    select = db;
                                else
                                {
                                    if (string.Equals(db.ComponentId, component.ComponentId, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (string.IsNullOrEmpty(adptor.AppName))
                                            select = db;
                                        else
                                            return db; //app名称相同且组件id相同
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return select;
        }

        /// <summary>
        /// 给指定的组件获取数据库连接串
        /// </summary>
        /// <param name="component"></param>
        /// <param name="appname"></param>
        /// <param name="ismaster"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected virtual string GetConnectStringForComponent(IComponent component, string appname, bool ismaster,out string dbType)
        {
            dbType = null;
            ComponentDbConnectStringSelectionElement a = GetElementForComponent(component, appname);
            if (a != null)
            {
                dbType = a.DabaseType;
                string str = a.ConnectString;
                if (ismaster)
                    str = a.MasterConnectString;
                if (!string.IsNullOrEmpty(str))
                {
                    return DecryptConnectionString(str);
                }
            }
            return null;
        }

        /// <summary>
        /// 给指定的连接串创建的数据库适配器
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="connectstring">数据库连接串</param>
        /// <returns>为连接串创建的数据库适配器</returns>
        protected virtual IDatabaseAdptor CreateNewDatabaseAdptorForComponent(string dbtype, string connectstring)
        {
            if (!string.IsNullOrEmpty(dbtype))
            {
                if (!string.IsNullOrEmpty(connectstring))
                {
                    if (string.Equals(dbtype, DatabaseTypeNames.MS_Sql, StringComparison.OrdinalIgnoreCase))
                    {
                        return new MsSqlDatabaseAdptor(connectstring);
                    }
                    else
                        throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtype));
                }
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ConnectStringIsNull, dbtype));
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtype));
        }

        /// <summary>
        /// 获取指定组件的数据库适配器
        /// </summary>
        /// <param name="component">要获取数据库适配器的组件</param>
        /// <returns>指定组件的数据库适配器</returns>
        protected IDatabaseAdptor GetDatabaseAdptorForComponent(IComponent component, string appname)
        {
            return GetDatabaseAdptorItemForComponent(component, appname, false).Adptor;
        }

        /// <summary>
        /// 获取指定组件的数据获取接口
        /// </summary>
        /// <param name="component">要获取数据获取接口的组件</param>
        /// <returns>数据获取接口</returns>
        protected IDataSelector GetDataSelectorForComponent(IComponent component, string appname)
        {
            return GetDatabaseAdptorItemForComponent(component, appname, false).DataSelector;
        }

        /// <summary>
        /// IDispose接口被调用
        /// 基类实现:释放所有已生成的IDatabaseAdptor
        /// </summary>
        protected virtual void OnDispose()
        {
            lock (p_DatabaseAdptors)
            {
                foreach (var p in p_DatabaseAdptors)
                    p.Adptor.Dispose();
                p_DatabaseAdptors.Clear();
            }
        }

        IDatabaseAdptor IComponentDatabaseContainer.GetDatabaseAdptor(IComponent component)
        {
            return GetDatabaseAdptorForComponent(component, component.CurrentAppName);
        }

        void IDisposable.Dispose()
        {
            OnDispose();
        }

        IDataSelector IComponentDatabaseContainer.GetDataSelector(IComponent component)
        {
            return GetDataSelectorForComponent(component, component.CurrentAppName);
        }

        IEnumerable<IDatabaseAdptor> IComponentDatabaseContainer.GeneratesAdptor
        {
            get
            {
                lock (p_DatabaseAdptors)
                {
                    return p_DatabaseAdptors.Select<DatabaseAdptorItem, IDatabaseAdptor>(f => f.Adptor).ToArray();
                }
            }
        }

    }

    internal class DatabaseAdptorItem
    {

        internal string CnntString { get; set; }

        internal IDatabaseAdptor Adptor { get; set; }

        internal IDataSelector DataSelector { get; set; }
    }
}
