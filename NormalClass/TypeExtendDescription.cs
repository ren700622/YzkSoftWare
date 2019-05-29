using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;
using YzkSoftWare.DataModel;

namespace YzkSoftWare
{
    public static class TypeExtendDescription
    {

        public static object[] EmptyObjects = new object[0];

        public static Type[] EmptyTypes = new Type[0];

        /// <summary>
        /// 判断数据类型是否是Nullable&lt;T&gt;类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns>如果type是一个可实例化的Nullable&lt;T&gt;类型则返回true,否则返回false</returns>
        public static bool IsNullableType(this Type type)
        {
            if (type.IsGenericType
               && !type.IsGenericTypeDefinition)
            {
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            return false;
        }

        /// <summary>
        /// 如果type是Nullable&lt;T&gt;类型则返回内镶的数据类型,否则返回type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeIfIsNullable(this Type type)
        {
            if (type.IsNullableType())
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        /// <summary>
        /// 获取属性说明符上的显示名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Type type)
        {
            if (Attribute.IsDefined(type, typeof(ResourceDisplayNameAttribute)))
            {
                ResourceDisplayNameAttribute att = Attribute.GetCustomAttribute(type, typeof(ResourceDisplayNameAttribute)) as ResourceDisplayNameAttribute;
                return att.ResourceDisplayName;
            }

            if (Attribute.IsDefined(type, typeof(System.ComponentModel.DisplayNameAttribute)))
            {
                System.ComponentModel.DisplayNameAttribute att = Attribute.GetCustomAttribute(type, typeof(System.ComponentModel.DisplayNameAttribute)) as System.ComponentModel.DisplayNameAttribute;
                return att.DisplayName;
            }
            return null;
        }

        /// <summary>
        /// 获取属性说明符上的说明文本
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDescription(this Type type)
        {
            if (Attribute.IsDefined(type, typeof(ResourceDescriptionAttribute)))
            {
                ResourceDescriptionAttribute att = Attribute.GetCustomAttribute(type, typeof(ResourceDescriptionAttribute)) as ResourceDescriptionAttribute;
                return att.ResourceDescription;
            }

            if (Attribute.IsDefined(type, typeof(DescriptionAttribute)))
            {
                DescriptionAttribute att = Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return att.Description;
            }
            return null;
        }

        /// <summary>
        /// 获取类型在数据库中的名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNameForDatabase(this Type type)
        {
            return DataModelHelper.GetDataModelNameForDatabase(type);
        }

        /// <summary>
        /// 将数据类型转换为数据库支持的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbType GetDbType(this Type type)
        {
            return DataModelHelper.ConvertToDbType(type);
        }

        /// <summary>
        /// 获取指定的Clr类型在数据库中的对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbTypeForDataModel GetDatabaseObjectType(this Type type)
        {
            return DataModelHelper.GetDbTypeForDatabaseObject(type);
        }

        /// <summary>
        /// 获取type代表的数据库对象模型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDataModel GetDataModel(this Type type)
        {
            return DataModelBuffer.Current.GetModel(type);
        }

        /// <summary>
        /// 生成指定泛型类型的缺省构造的对象实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public static object GenerateGenericDefaultObject(this Type type, params Type[] typeArguments)
        {
            if (type.IsGenericType)
            {
                Type lt = type.MakeGenericType(typeArguments);
                var cn = lt.GetConstructor(EmptyTypes);
                if (cn != null)
                    return cn.Invoke(EmptyObjects);
            }
            return null;
        }

        /// <summary>
        /// 创建type实例对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateObject(this Type type)
        {
            if (type.IsArray)
            {
                Type itemtype = type.GetElementType();
                if (itemtype == null)
                    itemtype = typeof(object);
                Array a = Array.CreateInstance(itemtype, 0);
                return a;
            }
            if (type.IsClass)
            {
                var cn = type.GetConstructor(EmptyTypes);
                if (cn != null)
                    return cn.Invoke(EmptyObjects);
                return null;
            }
            if (type.IsGenericType
               && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                Type lt = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                var cn = lt.GetConstructor(EmptyTypes);
                if (cn != null)
                    return cn.Invoke(EmptyObjects);
                return null;
            }
            if (type.IsGenericType
               && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type itemtype = type.GetGenericArguments()[0];
                Array a = Array.CreateInstance(itemtype, 0);
                return a;
            }
            if (type == typeof(IList) || type.FindInterface(typeof(IList)))
            {
                return new List<object>();
            }
            if (type == typeof(IEnumerable) || type.FindInterface(typeof(IEnumerable)))
            {
                Type itemtype = type.GetElementType();
                if (itemtype == null)
                    itemtype = typeof(object);
                Array a = Array.CreateInstance(itemtype, 0);
                return a;
            }
            return null;
        }

        /// <summary>
        /// 确定type是否实现接口interfacetype
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfacetype"></param>
        /// <returns></returns>
        public static bool FindInterface(this Type type, Type interfacetype)
        {
            Type[] pt = type.GetInterfaces();
            foreach (Type y in pt)
            {
                if (y == interfacetype)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定类型的数据检查借口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IUpdateChecker GetTypeUpdateChecker(this Type type)
        {
            if (Attribute.IsDefined(type, typeof(DataUpdateCheckersAttribute), true))
            {
                object[] oo = type.GetCustomAttributes(typeof(DataUpdateCheckersAttribute), true);
                if (oo != null)
                {
                    DataUpdateCheckersAttribute dd = oo.FirstOrDefault() as DataUpdateCheckersAttribute;
                    if (dd != null)
                    {
                        return dd.TypeValue.CreateObject() as IUpdateChecker;
                    }
                }
            }
            return new DefaultUpdateChceker();
        }

    }
}
