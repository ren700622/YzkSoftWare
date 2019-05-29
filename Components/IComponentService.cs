using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;
using YzkSoftWare.Database;

namespace YzkSoftWare.Components
{

    /// <summary>
    /// 组件服务接口
    /// </summary>
    public interface IComponentService : IComponent, IComponentUpdate, IDisposable
    {

        /// <summary>
        /// 当前可使用的数据获取接口
        /// </summary>
        IDataSelector DataSelector { get; set; }

        /// <summary>
        /// 获取指定方法的锁定键值
        /// </summary>
        /// <param name="methodNames">指定方法名称数组</param>
        /// <param name="readLockKeys">返回读锁定的值数组</param>
        /// <returns>返回写锁定的值数组</returns>
        IEnumerable<string> GetMethodLockKeys(IEnumerable<string> methodNames,out IEnumerable<string> readLockKeys);

        /// <summary>
        /// 获取指定方法的锁定键值
        /// </summary>
        /// <param name="methodName">指定方法名称</param>
        /// <param name="readLockKeys">返回读锁定的值数组</param>
        /// <returns>返回写锁定的值数组</returns>
        IEnumerable<string> GetMethodLockKeys(string methodName, out IEnumerable<string> readLockKeys);
    }

    /// <summary>
    /// 组件服务基础类
    /// </summary>
    public abstract class ComponentServiceBase : IComponentService
    {

        protected ComponentServiceBase()
        {
            ComponentId = this.GetType().FullName;
            p_UpdateContexts = new List<IUpdateContext>();
        }

        #region 私有成员

        /// <summary>
        /// 待更新的数据
        /// </summary>
        private IList<IUpdateContext> p_UpdateContexts;

        #endregion

        #region 受保护的成员


        /// <summary>
        /// 当有数据容器存储器更新了待更新的数据时调用
        /// 基类实现:清空待更新的数据内容
        /// </summary>
        protected virtual void OnSuccessUpdateComponent()
        {
            p_UpdateContexts.Clear();
        }

        /// <summary>
        /// IDispose接口被调用
        /// 基类实现:清空待更新的数据内容
        /// </summary>
        protected virtual void OnDispose()
        {
            p_UpdateContexts.Clear();
        }

        /// <summary>
        /// 检查指定的类型是否可更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void CheckDataModelIsCanUpdate<T>()
        {
            var dbtype = typeof(T).GetDataModel().DbType;
            switch (dbtype)
            {
                case DataModel.DbTypeForDataModel.Table: break;
                default:
                    throw new NotSupportException(ErrorCodes.NotSupportDatabaseObjectForUpdate, string.Format(LocalResource.NotSupportDatabaseObjectForUpdate, dbtype.GetEnumDisplayName()));
            }
        }

        /// <summary>
        /// 添加一个数据更新对象
        /// </summary>
        /// <param name="context"></param>
        protected void AddUpdateContext(IUpdateContext context)
        {
            p_UpdateContexts.Add(context);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T">要新增的数据类型</typeparam>
        /// <param name="newitem">要新增的数据</param>
        /// <param name="noty">新增数据的通知</param>
        /// <returns>返回新增数据的上下文</returns>
        protected DataUpdateContext AddNewData<T>(T newitem,IUpdateDataNoty noty)
        {
            CheckDataModelIsCanUpdate<T>();
            DataUpdateContext dc = new DataUpdateContext()
            {
                CheckRangle = FieldSelectRange.All,
                UpdateRangle = FieldSelectRange.All,
                Data = newitem,
                ModalState = ModalState.AddNew,
                ModalType = typeof(T),
                UpdateDataNoty = noty
            };
            AddUpdateContext(dc);
            return dc;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">要修改的数据类型</typeparam>
        /// <param name="updateitem">要修改的数据</param>
        /// <param name="updaterangle">要修改的数据的字段范围</param>
        /// <param name="updatefieldnames">updaterangle指定的范围的字段数组</param>
        /// <param name="noty">修改数据的通知</param>
        /// <returns>返回修改数据的上下文</returns>
        protected DataUpdateContext UpdateData<T>(T updateitem, FieldSelectRange updaterangle, IEnumerable<string> updatefieldnames, IUpdateDataNoty noty)
        {
            CheckDataModelIsCanUpdate<T>();
            DataUpdateContext dc = new DataUpdateContext()
            {
                CheckRangle = updaterangle,
                CheckFieldNames = updatefieldnames,
                UpdateRangle = updaterangle,
                UpdateFieldNames = updatefieldnames,
                Data = updateitem,
                ModalState = ModalState.Update,
                ModalType = typeof(T),
                UpdateDataNoty = noty
            };
            string softdelKey = typeof(T).GetDataModel().GetSoftDeleteFieldName();
            if (!string.IsNullOrEmpty(softdelKey))
            {
                switch (updaterangle)
                {
                    case FieldSelectRange.All:
                        {
                            dc.CheckRangle = FieldSelectRange.IgnoreFields;
                            dc.CheckFieldNames = new string[] { softdelKey };
                            dc.UpdateRangle = FieldSelectRange.IgnoreFields;
                            dc.UpdateFieldNames = new string[] { softdelKey };
                            break;
                        }
                    case FieldSelectRange.IgnoreFields:
                        {
                            IEnumerable<string> x = new string[] { softdelKey };
                            if (updatefieldnames != null)
                            {
                                if (updatefieldnames.FirstOrDefault(f => string.Equals(f, softdelKey, StringComparison.OrdinalIgnoreCase)) == null)
                                    x = x.Union(updatefieldnames);
                                else
                                    x = updatefieldnames;
                            }
                            dc.CheckFieldNames = x;
                            dc.UpdateFieldNames = x;
                            break;
                        }
                    case FieldSelectRange.OnlyFields:
                        {
                            if (updatefieldnames != null)
                            {
                                IEnumerable<string> x = updatefieldnames.Where(f => !string.Equals(f, softdelKey, StringComparison.OrdinalIgnoreCase)).ToArray();
                                dc.CheckFieldNames = x;
                                dc.UpdateFieldNames = x;
                            }
                            break;
                        }
                }
            }
            AddUpdateContext(dc);
            return dc;
        }

        /// <summary>
        /// 完整的修改数据
        /// </summary>
        /// <typeparam name="T">要修改的数据类型</typeparam>
        /// <param name="updateitem">要修改的数据</param>
        /// <returns>返回修改数据的上下文</returns>
        protected DataUpdateContext UpdateData<T>(T updateitem)
        {
            return UpdateData(updateitem, null);
        }

        /// <summary>
        /// 完整的修改数据
        /// </summary>
        /// <typeparam name="T">要修改的数据类型</typeparam>
        /// <param name="updateitem">要修改的数据</param>
        /// <param name="noty">修改数据的通知</param>
        /// <returns>返回修改数据的上下文</returns>
        protected DataUpdateContext UpdateData<T>(T updateitem, IUpdateDataNoty noty)
        {
            return UpdateData(updateitem, FieldSelectRange.All, null, noty);
        }

        /// <summary>
        /// 通过主键值删除数据
        /// </summary>
        /// <typeparam name="T">要删除的数据类型</typeparam>
        /// <param name="id">要删除的数据的主键值</param>
        /// <param name="noty">删除数据的通知</param>
        /// <returns>返回删除数据的上下文</returns>
        protected DataUpdateContext DeleteDataById<T>(long id, IUpdateDataNoty noty)
            where T : class,IDataId, new()
        {
            CheckDataModelIsCanUpdate<T>();
            T delitem = new T();
            delitem.Id = id;
            DataUpdateContext dc = new DataUpdateContext()
            {
                Data = delitem,
                ModalState = ModalState.Delete,
                ModalType = typeof(T),
                UpdateDataNoty = noty
            };
            AddUpdateContext(dc);
            return dc;
        }


        #endregion

        /// <summary>
        /// 获取数据接口
        /// </summary>
        public IDataSelector DataSelector { get; set; }

        /// <summary>
        /// 获取指定方法的锁定键值
        /// </summary>
        /// <param name="methodNames">指定方法名称数组</param>
        /// <param name="readLockKeys">返回读锁定的值数组</param>
        /// <returns></returns>
        public IEnumerable<string> GetMethodLockKeys(IEnumerable<string> methodNames, out IEnumerable<string> readLockKeys)
        {
            readLockKeys = null;
            if (methodNames == null) return ObjectExtendDescription.EmptyString;
            Type p = this.GetType();
            List<string> rds = new List<string>();
            List<string> wds = new List<string>();
            foreach (string n in methodNames)
            {
                MethodInfo m = p.GetMethod(n);
                if (m != null)
                {
                    var keys = m.GetCustomAttributes<ComponentMethodLockKeysAttribute>(true);
                    if (keys != null)
                    {
                        foreach (var k in keys)
                        {
                            if (k.IsRead)
                                rds.Add(k.LockKey);
                            else
                                wds.Add(k.LockKey);
                        }
                    }
                }
            }
            if (rds.Count > 0)
                readLockKeys = rds.ToArray();
            return wds.ToArray();
        }

        /// <summary>
        /// 获取指定方法的锁定键值
        /// </summary>
        /// <param name="methodNames">指定方法名称数组</param>
        /// <param name="readLockKeys">返回读锁定的值数组</param>
        /// <returns></returns>
        public IEnumerable<string> GetMethodLockKeys(string methodName, out IEnumerable<string> readLockKeys)
        {
            readLockKeys = null;
            if (!string.IsNullOrEmpty(methodName))
            {
                Type p = this.GetType();
                MethodInfo m = p.GetMethod(methodName);
                if (m != null)
                {
                    var keys = m.GetCustomAttributes<ComponentMethodLockKeysAttribute>(true);
                    if (keys != null)
                    {
                        List<string> rds = new List<string>();
                        List<string> wds = new List<string>();
                        foreach (var k in keys)
                        {
                            if (k.IsRead)
                                rds.Add(k.LockKey);
                            else
                                wds.Add(k.LockKey);
                        }
                        if (rds.Count > 0)
                            readLockKeys = rds.ToArray();
                        return wds.ToArray();
                    }
                }
            }
            return ObjectExtendDescription.EmptyString;
        }

        /// <summary>
        /// 获取或设置组件Id
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// 当前应用名称
        /// </summary>
        public string CurrentAppName { get; set; }

        /// <summary>
        /// 待更新的数据
        /// </summary>
        public IEnumerable<IUpdateContext> UpdateContexts
        {
            get { return p_UpdateContexts; }
        }

        public void SuccessUpdate()
        {
            OnSuccessUpdateComponent();
        }

        public void Dispose()
        {
            OnDispose();
        }
    }
}
