using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据模型接口
    /// </summary>
    public interface IDataModel : IExtendProperty, IInnerExtendProperty
    {

        /// <summary>
        /// 数据对象的名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 数据对象的显示名称
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 数据库对象的原型:.net类型
        /// </summary>
        Type ClrType { get; }

        /// <summary>
        /// 数据库对象类型
        /// </summary>
        DbTypeForDataModel DbType { get; }

        /// <summary>
        /// 获取该模型的主键
        /// </summary>
        IEnumerable<IDataModelFieldIndex> PrimaryKeys { get; }

        /// <summary>
        /// 获取该模型的索引
        /// </summary>
        IEnumerable<IDataModelFieldIndex> IndexKeys { get; }

        /// <summary>
        /// 字段模型集合
        /// </summary>
        IDataFieldModelCollection Fields { get; }
    }

    /// <summary>
    /// 定义数据类型的数据库对象类型
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "DbTypeForDataModel")]
    [ResourceDescription(typeof(LocalResource), "DbTypeForDataModel_Des")]
    public enum DbTypeForDataModel
    {
        /// <summary>
        /// 未知
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Unkown")]
        Unkown = 0,
        /// <summary>
        /// 基础表
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Table")]
        Table = 1,
        /// <summary>
        /// 视图
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "View")]
        View = 2,
        /// <summary>
        /// 存储过程
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Store")]
        Store = 3,
        /// <summary>
        /// Sql表达式
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Sql")]
        Sql = 4
    }

    internal sealed class TypeDataModel : ExtendProperty, IDataModel
    {

        public TypeDataModel(Type clrtype)
        {
            p_InnerExtend.SetExtendPropertyValue("ClrType", clrtype);
            p_InnerExtend.SetExtendPropertyValue("Name", clrtype.GetNameForDatabase());
            p_InnerExtend.SetExtendPropertyValue("DisplayName", clrtype.GetDisplayName());
            p_InnerExtend.SetExtendPropertyValue("DbType", clrtype.GetDatabaseObjectType());
            DataModelHelper.SetExtendProperty_Fields(p_InnerExtend, clrtype);
        }


        private IExtendProperty p_InnerExtend = new ExtendProperty();

        string IDataModel.Name
        {
            get { return p_InnerExtend.GetExtendPropertyValue<string>("Name", null); }
        }

        string IDataModel.DisplayName
        {
            get { return p_InnerExtend.GetExtendPropertyValue<string>("DisplayName", null); }
        }

        DbTypeForDataModel IDataModel.DbType
        {
            get { return p_InnerExtend.GetExtendPropertyValue<DbTypeForDataModel>("DbType", DbTypeForDataModel.Unkown); }
        }

        Type IDataModel.ClrType
        {
            get { return p_InnerExtend.GetExtendPropertyValue<Type>("ClrType", null); }
        }

        IEnumerable<IDataModelFieldIndex> IDataModel.PrimaryKeys
        {
            get { return DataModelHelper.GetExtendProperty_PrimaryKeys(p_InnerExtend); }
        }

        IEnumerable<IDataModelFieldIndex> IDataModel.IndexKeys
        {
            get { return DataModelHelper.GetExtendProperty_IndexKeys(p_InnerExtend); }
        }

        IExtendProperty IInnerExtendProperty.InnerExtend
        {
            get { return p_InnerExtend; }
        }

        IDataFieldModelCollection IDataModel.Fields
        {
            get { return DataModelHelper.GetExtendProperty_Fields(p_InnerExtend); }
        }
      
    }
}
