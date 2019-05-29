using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据字段模型接口
    /// </summary>
    public interface IDataFieldModel : IExtendProperty, IInnerExtendProperty
    {

        /// <summary>
        /// 字段名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 字段标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 字段说明
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 字段数据库数据类型
        /// </summary>
        DbType FieldDbType { get; }

    }

    public interface IInnerExtendProperty
    {

        IExtendProperty InnerExtend { get; }

    }

    internal sealed class PropertyDescriptorDataModelField : ExtendProperty, IDataFieldModel
    {

        public PropertyDescriptorDataModelField(PropertyDescriptor pd)
        {

            #region 基础属性

            p_InnerExtend.SetExtendPropertyValue("Name", pd.GetNameForDatabase());

            p_InnerExtend.SetExtendPropertyValue("Title", pd.GetDisplayName());

            p_InnerExtend.SetExtendPropertyValue("Description", pd.GetDescription());

            Type clrType = pd.PropertyType.GetTypeIfIsNullable();

            p_InnerExtend.SetExtendPropertyValue("FieldDbType", clrType.GetDbType());

            #endregion

            #region 扩展属性

            DataModelHelper.SetExtendProerty_IsSoftDeleteField(p_InnerExtend, pd);

            DataModelHelper.SetExtendProerty_RowVersion(p_InnerExtend, pd);

            DataModelHelper.SetExtendProerty_PropertyDescription(p_InnerExtend, pd);

            DataModelHelper.SetExtendProerty_ClrType(p_InnerExtend, clrType);

            DataModelHelper.SetExtendProperty_CanbeNull(p_InnerExtend, pd);

            DataModelHelper.SetExtendProperty_PrimaryKey(p_InnerExtend, pd);

            DataModelHelper.SetExtendProperty_IndexKey(p_InnerExtend, pd);

            DataModelHelper.SetExtendProerty_DbFieldValueConvertType(p_InnerExtend, pd);

            DataModelHelper.SetExtendProperty_DefaultValue(p_InnerExtend, pd);
            switch (clrType.GetDbType())
            {
                case DbType.String:
                    {
                        DataModelHelper.SetExtendProperty_MaxLength(p_InnerExtend, pd, 50);
                        break;
                    }
                case DbType.Decimal:
                    {
                        DataModelHelper.SetExtendProperty_DecimalDefine(p_InnerExtend, pd, new DecimalDefine() { DecimalLength = 18, DotLength = 2 });
                        break;
                    }
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                    {
                        DataModelHelper.SetExtendProperty_DbGeneratedDefine(p_InnerExtend, pd);
                        break;
                    }
            }
            //数字是否超出范围
            NumberType nt = DataValueParase.IsNumberType(clrType);
            if ((nt != NumberType.Unkown) || (clrType == typeof(DateTime) || clrType == typeof(TimeSpan)))
            {
                DataModelHelper.SetExtendProperty_MinMaxExpress(p_InnerExtend, pd);
            }
            #endregion

        }

        private IExtendProperty p_InnerExtend = new ExtendProperty();

        string IDataFieldModel.Name
        {
            get { return p_InnerExtend.GetExtendPropertyValue<string>("Name", null); }
        }

        string IDataFieldModel.Title
        {
            get { return p_InnerExtend.GetExtendPropertyValue<string>("Title", null); }
        }

        string IDataFieldModel.Description
        {
            get { return p_InnerExtend.GetExtendPropertyValue<string>("Description", null); }
        }

        DbType IDataFieldModel.FieldDbType
        {
            get { return p_InnerExtend.GetExtendPropertyValue<DbType>("FieldDbType", DbType.Object); }
        }

        IExtendProperty IInnerExtendProperty.InnerExtend
        {
            get { return p_InnerExtend; }
        }
    }
}
