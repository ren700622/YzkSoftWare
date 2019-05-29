using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data.Sql.MsSql;
using YzkSoftWare.Database;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 更新上下文接口
    /// </summary>
    public interface IUpdateContext
    {
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        ISqlExpress Update(string dbtypeName);

        /// <summary>
        /// 更新通知
        /// </summary>
        IUpdateDataNoty UpdateDataNoty { get; }

        /// <summary>
        /// 是否已成功提交到数据库
        /// </summary>
        bool SuccessUpdate { get; }

        /// <summary>
        /// 提交到数据库时的返回值
        /// </summary>
        object SuccessReturn { get; }

        /// <summary>
        /// 成功提交到数据库
        /// </summary>
        /// <param name="returnvalue"></param>
        void SuccessCommitToDatabase(object returnvalue);
    }

    /// <summary>
    /// 数据更新上下文接口
    /// </summary>
    public interface IDataUpdateContext : IUpdateContext
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        Type ModalType { get; }
        /// <summary>
        /// 数据
        /// </summary>
        object Data { get; }
        /// <summary>
        /// Modal数据的状态
        /// </summary>
        ModalState ModalState { get; }
        /// <summary>
        /// 字段检查范围
        /// </summary>
        FieldSelectRange CheckRangle { get; }
        /// <summary>
        /// 如果CheckRangle是IgnoreFields或OnlyFields则指定字段
        /// </summary>
        IEnumerable<string> CheckFieldNames { get; }
        /// <summary>
        /// 更新字段范围
        /// </summary>
        FieldSelectRange UpdateRangle { get;}
        /// <summary>
        /// 如果UpdateRangle是IgnoreFields或OnlyFields则指定字段
        /// </summary>
        IEnumerable<string> UpdateFieldNames { get; }

    }

    /// <summary>
    /// 数据更新通知接口
    /// </summary>
    public interface IUpdateDataNoty
    {

        /// <summary>
        /// 单个数据保存前调用
        /// </summary>
        /// <param name="context">将要更新的数据</param>
        /// <returns>返回false不保存</returns>
        bool BeforeChange(IUpdateContext context,IEnumerable<IUpdateContext> grouptocommit, IDataSelector dataSelector);

        /// <summary>
        /// 单个数据已保存后调用
        /// </summary>
        /// <param name="context">已更新的数据</param>
        void OnChanged(IUpdateContext context, IEnumerable<IUpdateContext> grouptocommit, IDataSelector dataSelector);

    }

    /// <summary>
    /// 数据状态
    /// </summary>
    [Flags()]
    [ResourceDisplayName(typeof(LocalResource), "ModalState")]
    [ResourceDescription(typeof(LocalResource), "Flags_Des")]
    public enum ModalState
    {
        /// <summary>
        /// 未知
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Unkown")]
        Unkown = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Normal")]
        Normal = 1,
        /// <summary>
        /// 新行
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "AddNew")]
        AddNew = 2,
        /// <summary>
        /// 修改
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Update")]
        Update = 4,
        /// <summary>
        /// 删除
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Delete")]
        Delete = 8
    }

    /// <summary>
    /// 数据更新上下文
    /// </summary>
    public class DataUpdateContext : IDataUpdateContext
    {

        public DataUpdateContext()
        {
            CheckRangle = FieldSelectRange.All;
            UpdateRangle = FieldSelectRange.All;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type ModalType { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Modal数据的状态
        /// </summary>
        public ModalState ModalState { get; set; }
        /// <summary>
        /// 字段检查范围
        /// </summary>
        public FieldSelectRange CheckRangle { get; set; }
        /// <summary>
        /// 如果CheckRangle是IgnoreFields或OnlyFields则指定字段
        /// </summary>
        public IEnumerable<string> CheckFieldNames { get; set; }
        /// <summary>
        /// 更新字段范围
        /// </summary>
        public FieldSelectRange UpdateRangle { get; set; }
        /// <summary>
        /// 如果UpdateRangle是IgnoreFields或OnlyFields则指定字段
        /// </summary>
        public IEnumerable<string> UpdateFieldNames { get; set; }
        /// <summary>
        /// 更新通知
        /// </summary>
        public IUpdateDataNoty UpdateDataNoty { get; set; }
        /// <summary>
        /// 生成更新操作的表达式
        /// </summary>
        /// <returns></returns>
        public ISqlExpress Update(string dbtypeName)
        {
            if (string.Equals(dbtypeName, DatabaseTypeNames.MS_Sql, StringComparison.OrdinalIgnoreCase))
            {
                switch (ModalState)
                {
                    case ModalState.AddNew:
                        {
                            Type addnew = typeof(AddNewMsSqlExpress<>);
                            object o = addnew.GenerateGenericDefaultObject(ModalType);
                            o.SetPropertyValue("NewItem", Data);
                            return o as ISqlExpress;
                        }
                    case ModalState.Update:
                        {
                            Type update = typeof(UpdateMsSqlExpress<>);
                            object o = update.GenerateGenericDefaultObject(ModalType);
                            o.SetPropertyValue("UpdateItem", Data);
                            o.InvokeMethod("SetUpdateItem",
                                new Type[] { ModalType, typeof(FieldSelectRange), typeof(IEnumerable<string>) },
                                new object[] { Data, UpdateRangle, UpdateFieldNames });
                            return o as ISqlExpress;
                        }
                    case ModalState.Delete:
                        {
                            Type del = typeof(DeleteMsSqlExpress<>);
                            object o = del.GenerateGenericDefaultObject(ModalType);
                            o.SetPropertyValue("DeleteItem", Data);
                            return o as ISqlExpress;
                        }
                    default: return null;
                }
            }
            throw new NotSupportedException(string.Format(LocalResource.NotSupportDbType, dbtypeName));
        }

        private bool p_SuccessUpdate = false;

        public bool SuccessUpdate
        {
            get { return p_SuccessUpdate; }
        }

        private object p_SuccessReturn = null;

        public object SuccessReturn
        {
            get { return p_SuccessReturn; }
        }

        public void SuccessCommitToDatabase(object returnvalue)
        {
            p_SuccessReturn = returnvalue;
            p_SuccessUpdate = true;
        }
    }


}
