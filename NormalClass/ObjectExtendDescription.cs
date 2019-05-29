using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;

namespace YzkSoftWare
{
    public static class ObjectExtendDescription
    {

        /// <summary>
        /// 空字符数组
        /// </summary>
        public readonly static string[] EmptyString = new string[0];

        /// <summary>
        /// 空的IDisposable对象
        /// </summary>
        public readonly static IDisposable EmptyDispose = new EmptyDisposed();

        /// <summary>
        /// 空类型数组
        /// </summary>
        public readonly static Type[] EmptyTypes = new Type[0];

        /// <summary>
        /// 使用PropertyDescriptor映射设置属性值
        /// </summary>
        /// <param name="des"></param>
        /// <param name="propertyname"></param>
        /// <param name="propertyvalue"></param>
        public static void SetPropertyValue(this object des, string propertyname, object propertyvalue)
        {
            if (des == null)
                return;
            PropertyDescriptorCollection pdcols = TypeDescriptor.GetProperties(des);
            PropertyDescriptor pd = pdcols[propertyname];
            if (pd != null)
                pd.SetValue(des, propertyvalue);
        }

        /// <summary>
        /// 使用PropertyDescriptor映射获取属性值
        /// </summary>
        /// <param name="des"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object des, string propertyname)
        {
            if (des == null)
                return null;
            PropertyDescriptorCollection pdcols = TypeDescriptor.GetProperties(des);
            PropertyDescriptor pd = pdcols[propertyname];
            if (pd != null)
                return pd.GetValue(des);
            return null;
        }

        /// <summary>
        /// 获取指定属性名称的属性数据类型
        /// </summary>
        /// <param name="des"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static Type GetPropertyType(this object des, string propertyname)
        {
            if (des == null)
                return null;
            PropertyDescriptorCollection pdcols = TypeDescriptor.GetProperties(des);
            PropertyDescriptor pd = pdcols[propertyname];
            if (pd != null)
                return pd.PropertyType;
            return null;
        }


        /// <summary>
        /// 使用映射调用对象des的公共方法
        /// </summary>
        /// <param name="des"></param>
        /// <param name="methodname"></param>
        /// <param name="parametTypes"></param>
        /// <param name="paramets"></param>
        /// <returns></returns>
        public static object InvokeMethod(this object des, string methodname,Type[] parametTypes, params object[] paramets)
        {
            var m = des.GetType().GetMethod(methodname, parametTypes);
            if (m != null)
                return m.Invoke(des, paramets);
            return null;
        }


        /// <summary>
        /// 使用PropertyDescriptor映射(或IDataRowReader接口,如果对象实IDataRowReader接口)获取属性值
        /// </summary>
        /// <param name="des"></param>
        /// <param name="membername"></param>
        /// <returns></returns>
        public static object GetMemberValue(this object des, string membername)
        {
            if (des == null)
                return null;
            if (des is IDataRowReader)
            {
                IDataRowReader r = des as IDataRowReader;
                return r[membername];
            }
            PropertyDescriptorCollection pdcols = TypeDescriptor.GetProperties(des);
            PropertyDescriptor pd = pdcols[membername];
            if (pd != null)
                return pd.GetValue(des);
            return null;
        }

        /// <summary>
        /// 使用PropertyDescriptor映射(或IDataRowSetter接口,如果对象实IDataRowSetter接口)设置属性值
        /// </summary>
        /// <param name="des"></param>
        /// <param name="propertyname"></param>
        /// <param name="propertyvalue"></param>
        public static void SetMemberValue(this object des, string membername, object membervalue)
        {
            if (des == null)
                return;
            try
            {
                if (des is IDataRowSetter)
                {
                    IDataRowSetter s = des as IDataRowSetter;
                    s.SetFieldValue(membername, membervalue);
                    return;
                }
                PropertyDescriptorCollection pdcols = TypeDescriptor.GetProperties(des);
                PropertyDescriptor pd = pdcols[membername];
                if (pd != null)
                {
                    if (membervalue != null && pd.PropertyType.IsNullableType() && pd.PropertyType.GetTypeIfIsNullable().IsEnum)
                    {
                        //防止 从int类型到enum?类型的转换无效错误
                        object v = Enum.ToObject(pd.PropertyType.GetTypeIfIsNullable(), membervalue);
                        pd.SetValue(des, v);
                    }
                    else
                        pd.SetValue(des, membervalue);
                }
                    
            }
            catch (Exception e)
            {
                throw new UnkownCodeException(ErrorCodes.SetMemberValueError, string.Format(LocalResource.SetMemberValueError, des.GetType().FullName, membername, e.Message));
            }
        }

        /// <summary>
        /// 判定值v是否是一个位域值
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsOnlyBitFieldValue(this int v)
        {
            switch (v)
            {
                case 0x01:
                case 0x02:
                case 0x04:
                case 0x08:
                case 0x10:
                case 0x20:
                case 0x40:
                case 0x80:
                case 0x0100:
                case 0x0200:
                case 0x0400:
                case 0x0800:
                case 0x1000:
                case 0x2000:
                case 0x4000:
                case 0x8000:
                case 0x010000:
                case 0x020000:
                case 0x040000:
                case 0x080000:
                case 0x100000:
                case 0x200000:
                case 0x400000:
                case 0x800000:
                case 0x01000000:
                case 0x02000000:
                case 0x04000000:
                case 0x08000000:
                case 0x10000000:
                case 0x20000000:
                case 0x40000000:
                    return true;
            }
            uint uu = (uint)v;
            return uu == 0x80000000;
        }

        private static string GetEnumDisplayName_one(Type enumvalueType, object enumvalue)
        {
            FieldInfo f = enumvalueType.GetField(enumvalue.ToString());
            if (f != null)
            {
                string d = f.GetDisplayName();
                if (!string.IsNullOrEmpty(d))
                    return d;
            }
            return enumvalue.ToString();
        }

        /// <summary>
        /// 获取枚举值的显示名称
        /// </summary>
        /// <param name="enumvalue">要获取显示名称的枚举值</param>
        /// <returns>枚举值的显示名称</returns>
        private static string GetEnumDisplayName_core(object enumvalue)
        {
            if (enumvalue == null)
                return null;
            Type enumtype = enumvalue.GetType();
            if (!enumtype.IsEnum) return null;
            bool isflag = Attribute.IsDefined(enumtype, typeof(FlagsAttribute));
            if (isflag)
            {
                int iv = (int)enumvalue;
                if (iv.IsOnlyBitFieldValue())
                {
                    return GetEnumDisplayName_one(enumtype, enumvalue);
                }
                else
                {
                    var vv = Enum.GetValues(enumtype);
                    if (vv != null)
                    {
                        StringBuilder xb = new StringBuilder();
                        foreach (object o in vv)
                        {
                            int sv = (int)o;
                            if (sv.IsOnlyBitFieldValue())
                            {
                                if ((iv & sv) == sv)
                                {
                                    string d = GetEnumDisplayName_one(enumtype, o);
                                    if (xb.Length == 0)
                                        xb.Append(d);
                                    else
                                        xb.AppendFormat(",{0}", d);
                                }
                            }
                        }
                        return xb.ToString();
                    }
                }
            }
            else
            {
                return GetEnumDisplayName_one(enumtype, enumvalue);
            }
            return null;
        }

        /// <summary>
        /// 获取枚举值的显示名称
        /// </summary>
        /// <param name="enumvalue">要获取显示名称的枚举值</param>
        /// <returns>枚举值的显示名称</returns>
        public static string GetEnumDisplayName(object enumvalue)
        {
            return GetEnumDisplayName_core(enumvalue);
        }

        /// <summary>
        /// 获取枚举值的显示名称
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <param name="enumvalue">要获取显示名称的枚举值</param>
        /// <returns>枚举值的显示名称</returns>
        public static string GetEnumDisplayName<T>(this T enumvalue)
            where T : struct
        {
            return GetEnumDisplayName_core(enumvalue);
        }
    }

    internal class EmptyDisposed : IDisposable
    {

        void IDisposable.Dispose()
        {
        }

    }
}
