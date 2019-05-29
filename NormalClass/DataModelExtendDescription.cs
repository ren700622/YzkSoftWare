using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;
using YzkSoftWare.DataModel;

namespace YzkSoftWare
{
    public static class DataModelExtendDescription
    {

        /// <summary>
        /// 获取字段的缺省值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetDefaultValue(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_DefaultValue(field.InnerExtend);
        }

        /// <summary>
        /// 获取字段对Id的引用信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IDataIdRefence GetDataIdRefence(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProerty_DataIdRefence(field.InnerExtend);
        }

        /// <summary>
        /// 获取字段模型的Clr数据类型
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Type GetClrType(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProerty_ClrType(field.InnerExtend);
        }

        /// <summary>
        /// 获取字段模型的值是否可为NULL
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool CanbeNull(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_CanbeNull(field.InnerExtend);
        }

        /// <summary>
        /// 指示该字段是否是行版本
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsRowVersion(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProerty_RowVersion(field.InnerExtend);
        }

        /// <summary>
        /// 指示该字段是否是软删除字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsSoftDeleteField(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProerty_IsSoftDeleteField(field.InnerExtend);
        }

        /// <summary>
        /// 获取字段的数据大小:返回0标识没有设置
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int GetFieldSize(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_MaxLength(field.InnerExtend);
        }

        /// <summary>
        /// 判断字段模型是否是主键
        /// </summary>
        /// <param name="field"></param>
        /// <returns>如果该字段模型是主键则返回主键索引信息,否则返回null</returns>
        public static IDataModelFieldIndex IsPrimaryKey(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_PrimaryKey(field.InnerExtend);
        }

        /// <summary>
        /// 判断字段模型是否是索引
        /// </summary>
        /// <param name="field"></param>
        /// <returns>如果该字段模型是索引则返回索引信息,否则返回null</returns>
        public static IDataModelFieldIndex IsIndexKey(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_IndexKey(field.InnerExtend);
        }

        /// <summary>
        /// 获取decimal类型的字段的精度定义
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IDecimalDefine GetDecimalDefine(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_DecimalDefine(field.InnerExtend);
        }

        /// <summary>
        /// 获取字段标识的自动增长属性
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IDbGeneratedDefine GetDbGeneratedDefine(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_DbGeneratedDefine(field.InnerExtend);
        }

        /// <summary>
        /// 获取字段最大值和最小值限制表达式
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetMinMaxExpress(this IDataFieldModel field)
        {
            return DataModelHelper.GetExtendProperty_MinMaxExpress(field.InnerExtend);
        }

        /// <summary>
        /// 设置数据data的字段值为fieldvalue
        /// <para>如果data实现接口IDataRowSetter,则调用IDataRowSetter.SetFieldValue</para>
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data">要设置值的数据</param>
        /// <param name="fieldvalue">要设置为的字段值</param>
        public static void SetModelFieldValue(this IDataFieldModel field, object data, object fieldvalue)
        {
            if (data is IDataRowSetter)
            {
                IDataRowSetter s = data as IDataRowSetter;
                s.SetFieldValue(field.Name, fieldvalue);
                return;
            }
            PropertyDescriptor pd = DataModelHelper.GetExtendProerty_PropertyDescription(field.InnerExtend);
            if (pd != null)
            {
                pd.SetValue(data, fieldvalue);
            }
            else
            {
                throw new ObjectIsNullException(LocalResource.FieldPropertyDescriptor);
            }
        }

        /// <summary>
        /// 获取数据data的字段值
        /// <para>如果数据data实现接口IDataRowReader,则返回Item[name]</para>
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data">要获取字段值的数据</param>
        /// <returns></returns>
        public static object GetModelFieldValue(this IDataFieldModel field, object data)
        {
            if (data is IDataRowReader)
            {
                IDataRowReader r = data as IDataRowReader;
                return r[field.Name];
            }
            PropertyDescriptor pd = DataModelHelper.GetExtendProerty_PropertyDescription(field.InnerExtend);
            if (pd != null)
            {
                return pd.GetValue(data);
            }
            throw new ObjectIsNullException(LocalResource.FieldPropertyDescriptor);
        }

        /// <summary>
        /// 获取数据模型的软删除字段名称
        /// <para>如果该模型不是软删除模型,则返回null</para>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetSoftDeleteFieldName(this IDataModel model)
        {
            return DataModelHelper.GetExtendProperty_SoftDeleteFieldName(model.InnerExtend);
        }

        /// <summary>
        /// 获取数据库值转换接口
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IDbValueConvertor GetDbValueConvertor(this IDataFieldModel field)
        {
            Type v = DataModelHelper.GetExtendProerty_DbFieldValueConvertType(field.InnerExtend);
            if (v != null)
                return v.CreateObject() as IDbValueConvertor;
            return null;
        }
    }
}
