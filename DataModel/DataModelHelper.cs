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
    internal static class DataModelHelper
    {

        /// <summary>
        /// 设置扩展属性:字段缺省值(DefaultValue)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_DefaultValue(IExtendProperty ext, PropertyDescriptor pd)
        {
            DefaultValueAttribute a = pd.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
            if (a != null && !string.IsNullOrEmpty(a.DefaultValue))
            {
                ext.SetExtendPropertyValue("DefaultValue", a.DefaultValue);
            }
        }

        /// <summary>
        /// 获取扩展属性:字段缺省值(DefaultValue)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static string GetExtendProperty_DefaultValue(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<string>("DefaultValue", null);
        }


        /// <summary>
        /// 设置扩展属性:数据引用(DataIdRefence)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProerty_DataIdRefence(IExtendProperty ext, IDataIdRefence at)
        {
            ext.SetExtendPropertyValue("DataIdRefence", at);
        }

        /// <summary>
        /// 获取扩展属性:数据引用
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IDataIdRefence GetExtendProerty_DataIdRefence(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<IDataIdRefence>("DataIdRefence", null);
        }


        /// <summary>
        /// 获取指定类型在数据库中的名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetDataModelNameForDatabase(Type type)
        {
            if (Attribute.IsDefined(type, typeof(DatabaseObjectNameAttribute), false))
            {
                DatabaseObjectNameAttribute d = type.GetCustomAttributes(typeof(DatabaseObjectNameAttribute), false)[0] as DatabaseObjectNameAttribute;
                if (!string.IsNullOrEmpty(d.Name))
                    return d.Name;
            }
            return type.Name;
        }

        /// <summary>
        /// 获取指定类型在数据库中的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static string GetDataModelNameForDatabase<T>()
            where T : class
        {
            return GetDataModelNameForDatabase(typeof(T));
        }

        /// <summary>
        /// 获取指定属性在数据库中的名称
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        internal static string GetDataFieldModelNameForDatabase(PropertyDescriptor pd)
        {
            string fn = pd.Name;
            DatabaseObjectNameAttribute a = pd.Attributes[typeof(DatabaseObjectNameAttribute)] as DatabaseObjectNameAttribute;
            if (a != null && !string.IsNullOrEmpty(a.Name))
                fn = a.Name;
            return fn;
        }

        /// <summary>
        /// 设置扩展属性:数据库字段值转换接口的类型(DbFieldValueConvertType)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProerty_DbFieldValueConvertType(IExtendProperty ext, PropertyDescriptor pd)
        {
            DbFieldValueConvertAttribute a = pd.Attributes[typeof(DbFieldValueConvertAttribute)] as DbFieldValueConvertAttribute;
            if (a != null && a.TypeValue != null && a.TypeValue.FindInterface(typeof(IDbValueConvertor)))
            {
                ext.SetExtendPropertyValue("DbFieldValueConvertType", a.TypeValue);
            }
        }

        /// <summary>
        /// 获取扩展属性:数据库字段值转换接口的类型(DbFieldValueConvertType)
        /// </summary>
        /// <param name="ext"></param>
        internal static Type GetExtendProerty_DbFieldValueConvertType(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<Type>("DbFieldValueConvertType", null); 
        }

        /// <summary>
        /// 将type转换为数据库支持的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static DbType ConvertToDbType(Type type)
        {
            if (type == typeof(string))
                return DbType.String;
            if (type == typeof(char[]))
                return DbType.AnsiString;
            if (type == typeof(byte[]))
                return DbType.Binary;
            if (type == typeof(byte))
                return DbType.Byte;
            if (type == typeof(bool))
                return DbType.Boolean;
            if (type == typeof(DateTime))
                return DbType.DateTime;
            if (type == typeof(decimal))
                return DbType.Decimal;
            if (type == typeof(double))
                return DbType.Double;
            if (type == typeof(Guid))
                return DbType.Guid;
            if (type == typeof(short))
                return DbType.Int16;
            if (type == typeof(int))
                return DbType.Int32;
            if (type == typeof(long))
                return DbType.Int64;
            if (type == typeof(ushort))
                return DbType.UInt16;
            if (type == typeof(uint))
                return DbType.UInt32;
            if (type == typeof(ulong))
                return DbType.UInt64;
            if (type == typeof(sbyte))
                return DbType.SByte;
            if (type == typeof(float))
                return DbType.Single;
            if (type == typeof(TimeSpan))
                return DbType.Time;
            if (type.IsEnum)
                return DbType.Int32;
            return DbType.Object;
        }

        /// <summary>
        /// 设置扩展属性:是否是软删除字段(IsSoftDeleteField)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProerty_IsSoftDeleteField(IExtendProperty ext, PropertyDescriptor pd)
        {
            SoftDeleteFieldAttribute a = pd.Attributes[typeof(SoftDeleteFieldAttribute)] as SoftDeleteFieldAttribute;
            if (a != null && a.IsSoftDelete)
                ext.SetExtendPropertyValue("IsSoftDeleteField", true);
        }

        /// <summary>
        /// 获取扩展属性:是否是软删除字段(IsSoftDeleteField)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static bool GetExtendProerty_IsSoftDeleteField(IExtendProperty ext)
        {
            bool? a = ext.GetExtendPropertyValue<bool?>("IsSoftDeleteField", null);
            if (a != null)
                return a.Value;
            return false;
        }

        /// <summary>
        /// 设置扩展属性:是否是行版本字段(IsRowVersion)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProerty_RowVersion(IExtendProperty ext, PropertyDescriptor pd)
        {
            RowVersionAttribute a = pd.Attributes[typeof(RowVersionAttribute)] as RowVersionAttribute;
            if (a != null)
                ext.SetExtendPropertyValue("IsRowVersion", true);
        }

        /// <summary>
        /// 获取扩展属性:是否是行版本字段(IsRowVersion)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static bool GetExtendProerty_RowVersion(IExtendProperty ext)
        {
            bool? a = ext.GetExtendPropertyValue<bool?>("IsRowVersion", null);
            if (a != null)
                return a.Value;
            return false;
        }

        /// <summary>
        /// 设置扩展属性:属性描述符(PropertyDescriptor)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProerty_PropertyDescription(IExtendProperty ext, PropertyDescriptor pd)
        {
            ext.SetExtendPropertyValue("PropertyDescriptor", pd);
        }

        /// <summary>
        /// 获取扩展属性:属性描述符(PropertyDescriptor)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static PropertyDescriptor GetExtendProerty_PropertyDescription(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<PropertyDescriptor>("PropertyDescriptor", null);
        }

        /// <summary>
        /// 设置扩展属性:Clr数据类型(ClrType)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="type"></param>
        internal static void SetExtendProerty_ClrType(IExtendProperty ext, Type type)
        {
            ext.SetExtendPropertyValue("ClrType", type);
        }

        /// <summary>
        /// 获取扩展属性:Clr数据类型(ClrType)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static Type GetExtendProerty_ClrType(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<Type>("ClrType", null);
        }

        /// <summary>
        /// 设置扩展属性:字段是否可为NULL(CanbeNull)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        /// <param name="dbtype"></param>
        internal static void SetExtendProperty_CanbeNull(IExtendProperty ext, PropertyDescriptor pd)
        {
            if (pd.PropertyType.IsNullableType())
                ext.SetExtendPropertyValue("CanbeNull", true);
            else
            {
                switch (pd.PropertyType.GetTypeIfIsNullable().GetDbType())
                {
                    case DbType.String:
                    case DbType.Object:
                        {
                            MustInputAttribute a = pd.Attributes[typeof(MustInputAttribute)] as MustInputAttribute;
                            if (a != null)
                                ext.SetExtendPropertyValue("CanbeNull", false);
                            else
                                ext.SetExtendPropertyValue("CanbeNull", true);
                            break;
                        }
                    default:
                        {
                            ext.SetExtendPropertyValue("CanbeNull", false);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 获取扩展属性:字段是否可为NULL(CanbeNull)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static bool GetExtendProperty_CanbeNull(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<bool>("CanbeNull", false);
        }

        /// <summary>
        /// 设置扩展属性:字段是否主键(PrimaryKey)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_PrimaryKey(IExtendProperty ext, PropertyDescriptor pd)
        {
            PrimaryKeyAttribute a = pd.Attributes[typeof(PrimaryKeyAttribute)] as PrimaryKeyAttribute;
            if (a != null && a.IsPrimaryKey)
            {
                DataModelFieldIndex d = new DataModelFieldIndex() { Name = pd.GetNameForDatabase(), Order = a.Order };
                ext.SetExtendPropertyValue("PrimaryKey", d);
            }
        }

        /// <summary>
        /// 获取扩展属性:字段是否主键(PrimaryKey)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IDataModelFieldIndex GetExtendProperty_PrimaryKey(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<DataModelFieldIndex>("PrimaryKey", null);
        }

        /// <summary>
        /// 设置扩展属性:字段是否索引(IndexKey)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_IndexKey(IExtendProperty ext, PropertyDescriptor pd)
        {
            IndexKeyAttribute a = pd.Attributes[typeof(IndexKeyAttribute)] as IndexKeyAttribute;
            if (a != null && a.IsIndexKey)
            {
                PrimaryKeyAttribute b = pd.Attributes[typeof(PrimaryKeyAttribute)] as PrimaryKeyAttribute;
                if (b != null && b.IsPrimaryKey)
                    return;
                DataModelFieldIndex d = new DataModelFieldIndex() { Name = pd.GetNameForDatabase(), Order = a.Order };
                ext.SetExtendPropertyValue("IndexKey", d);
            }
        }

        /// <summary>
        /// 获取扩展属性:字段是否索引(IndexKey)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IDataModelFieldIndex GetExtendProperty_IndexKey(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<DataModelFieldIndex>("IndexKey", null);
        }

        /// <summary>
        /// 设置扩展属性:字段最大数据长度(MaxLength)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_MaxLength(IExtendProperty ext, PropertyDescriptor pd,int? defaultvalue)
        {
            MaxValueLengthAttribute a = pd.Attributes[typeof(MaxValueLengthAttribute)] as MaxValueLengthAttribute;
            if (a != null)
            {
                ext.SetExtendPropertyValue("MaxLength", a.MaxValue);
            }
            else
            {
                if (defaultvalue != null && defaultvalue.HasValue)
                    ext.SetExtendPropertyValue("MaxLength", defaultvalue.Value);
            }
        }

        /// <summary>
        /// 获取扩展属性:字段最大数据长度(MaxLength)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static int GetExtendProperty_MaxLength(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<int>("MaxLength", 0);
        }


        /// <summary>
        /// 设置扩展属性:数字类型字段值的最大和最小限制表达式(MinMaxExpress)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_MinMaxExpress(IExtendProperty ext, PropertyDescriptor pd)
        {
            MinMaxValueAttribute a = pd.Attributes[typeof(MinMaxValueAttribute)] as MinMaxValueAttribute;
            if (a != null)
            {
                ext.SetExtendPropertyValue("MinMaxExpress", a.ValueExpress);
            }
        }

        /// <summary>
        /// 获取扩展属性:数字类型字段值的最大和最小限制表达式(MinMaxExpress)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static string GetExtendProperty_MinMaxExpress(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<string>("MinMaxExpress", null);
        }


        /// <summary>
        /// 设置扩展属性:Decimal类型的字段精度定义(DecimalDefine)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_DecimalDefine(IExtendProperty ext, PropertyDescriptor pd,DecimalDefine defaultvalue)
        {
            DecimalDbDescriptionAttribute a = pd.Attributes[typeof(DecimalDbDescriptionAttribute)] as DecimalDbDescriptionAttribute;
            if (a != null)
            {
                DecimalDefine d = new DecimalDefine() { DecimalLength = a.DecimalLength, DotLength = a.DotLength };
                ext.SetExtendPropertyValue("DecimalDefine", d);
            }
            else
            {
                if (defaultvalue != null)
                    ext.SetExtendPropertyValue("DecimalDefine", defaultvalue);
            }

        }

        /// <summary>
        /// 获取扩展属性:Decimal类型的字段精度定义(DecimalDefine)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IDecimalDefine GetExtendProperty_DecimalDefine(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<DecimalDefine>("DecimalDefine", null);
        }

        /// <summary>
        /// 设置扩展属性:数据库字段的自动增加属性(DbGeneratedDefine)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="pd"></param>
        internal static void SetExtendProperty_DbGeneratedDefine(IExtendProperty ext, PropertyDescriptor pd)
        {
            DbGeneratedAttribute a = pd.Attributes[typeof(DbGeneratedAttribute)] as DbGeneratedAttribute;
            if (a != null)
            {
                DbGeneratedDefine d = new DbGeneratedDefine() { StartValue = a.StartValue, Step = a.Step };
                ext.SetExtendPropertyValue("DbGeneratedDefine", d);
            }

        }

        /// <summary>
        /// 获取扩展属性:数据库字段的自动增加属性(DbGeneratedDefine)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IDbGeneratedDefine GetExtendProperty_DbGeneratedDefine(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<DbGeneratedDefine>("DbGeneratedDefine", null);
        }

        /// <summary>
        /// 获取指定的Clr类型在数据库中的对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static DbTypeForDataModel GetDbTypeForDatabaseObject(Type type)
        {
            if (type.IsClass && !type.IsAbstract)
            {
                if (Attribute.IsDefined(type, typeof(DatabaseObjectAttribute), false))
                {
                    DatabaseObjectAttribute d = type.GetCustomAttributes(typeof(DatabaseObjectAttribute), false)[0] as DatabaseObjectAttribute;
                    return d.DbType;
                }
                if (Attribute.IsDefined(type, typeof(DatabaseObjectAttribute), true))
                {
                    DatabaseObjectAttribute d = type.GetCustomAttributes(typeof(DatabaseObjectAttribute), true)[0] as DatabaseObjectAttribute;
                    return d.DbType;
                }
            }
            return DbTypeForDataModel.Unkown;
        }

        /// <summary>
        /// 是否该在数据库对象中定义pd属性指定的字段
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool FieldIsDefineOnDb(PropertyDescriptor pd, DataModelFieldCustomFilterAttribute b)
        {
            if (b != null)
                return b.IsDefineFeild(pd);
            NoneDatabaseFieldAttribute a = pd.Attributes[typeof(NoneDatabaseFieldAttribute)] as NoneDatabaseFieldAttribute;
            if (a != null)
                return a.IsDefineOnDb;
            return true;
        }

        /// <summary>
        /// 设置扩展属性:字段集合(Fields,PrimaryKeys,IndexKey,SoftDeleteFieldName)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="type"></param>
        internal static void SetExtendProperty_Fields(IExtendProperty ext,Type type)
        {
            //自定义字段筛选特性
            DataModelFieldCustomFilterAttribute a = null;
            if (Attribute.IsDefined(type, typeof(DataModelFieldCustomFilterAttribute), true))
                a = type.GetCustomAttributes(typeof(DataModelFieldCustomFilterAttribute), true)[0] as DataModelFieldCustomFilterAttribute;
            List<IDataModelFieldIndex> primk = new List<IDataModelFieldIndex>();
            List<IDataModelFieldIndex> indexk = new List<IDataModelFieldIndex>();
            DataFieldModelCollection fields = new DataFieldModelCollection();
            PropertyDescriptorCollection pdcols = TypeDescriptor.GetProperties(type);
            string softDeleteFieldName = null;
            foreach (PropertyDescriptor p in pdcols)
            {
                if (FieldIsDefineOnDb(p, a))
                {
                    IDataFieldModel field = new PropertyDescriptorDataModelField(p);
                    fields.Add(field);
                    var pk = field.IsPrimaryKey();
                    if (pk != null)
                        primk.Add(pk);
                    var ink = field.IsIndexKey();
                    if (ink != null)
                        indexk.Add(ink);
                    if(field.IsSoftDeleteField())
                    {
                        if (!string.IsNullOrEmpty(softDeleteFieldName))
                            throw new DataModelException(string.Format(LocalResource.DataModelException_ExtSFD, softDeleteFieldName, field.Name));
                        softDeleteFieldName = field.Name;
                        ext.SetExtendPropertyValue("SoftDeleteFieldName", softDeleteFieldName);
                    }
                }
            }
            ext.SetExtendPropertyValue("Fields", fields);
            ext.SetExtendPropertyValue("PrimaryKeys", primk.ToArray());
            ext.SetExtendPropertyValue("IndexKeys", indexk.ToArray());

            object[] o = type.GetCustomAttributes(typeof(DataIdRefenceAttribute), true);
            if (o != null)
            {
                foreach (object item in o)
                {
                    DataIdRefenceAttribute da = item as DataIdRefenceAttribute;
                    var sfield = fields.FirstOrDefault(f => string.Equals(f.Name, da.SourceFieldName, StringComparison.OrdinalIgnoreCase));
                    if (sfield != null)
                    {
                        DataIdRefence df = new DataIdRefence() { RefenceIsParent = da.RefenceIsParent, RefenceModalType = da.RefenceModalType, SourceFieldName = da.SourceFieldName };
                        SetExtendProerty_DataIdRefence(sfield.InnerExtend, df);
                    }
                }
            }
        }

        /// <summary>
        /// 获取扩展属性:软删除字段名称(SoftDeleteFieldName)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static string GetExtendProperty_SoftDeleteFieldName(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<string>("SoftDeleteFieldName", null);
        }

        /// <summary>
        /// 获取扩展属性:字段集合(Fields)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IDataFieldModelCollection GetExtendProperty_Fields(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<IDataFieldModelCollection>("Fields", null);
        }

        /// <summary>
        /// 获取扩展属性:字段主键集合(PrimaryKeys)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IEnumerable<IDataModelFieldIndex> GetExtendProperty_PrimaryKeys(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<IEnumerable<IDataModelFieldIndex>>("PrimaryKeys", null);
        }

        /// <summary>
        /// 获取扩展属性:字段索引集合(IndexKey)
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal static IEnumerable<IDataModelFieldIndex> GetExtendProperty_IndexKeys(IExtendProperty ext)
        {
            return ext.GetExtendPropertyValue<IEnumerable<IDataModelFieldIndex>>("IndexKeys", null);
        }
    }
}
