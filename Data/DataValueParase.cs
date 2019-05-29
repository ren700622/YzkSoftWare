using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data
{
    public static class DataValueParase
    {

        /// <summary>
        /// 指定的id是否不等于null且大于0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsValidId(long? id)
        {
            return id != null && id.HasValue ? id.Value > 0 : false;
        }

        /// <summary>
        /// 是否是空值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDbNullValue(object value)
        {
            if (value == null)
                return true;
            if (value is DBNull)
                return true;
            if (value is string)
                return string.IsNullOrEmpty((string)value);
            return false;
        }

        /// <summary>
        /// 是否通过空值验证
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool PassNullValue(IDataFieldModel field, object value)
        {
            if (field.CanbeNull())
                return true;
            if (IsDbNullValue(value))
                return false;
            var x = field.GetDataIdRefence();
            if (x != null)
            {
                if (value is long)
                    return IsValidId((long)value);
                if (value is long?)
                    return IsValidId((long?)value);
            }
            return true;
        }


        /// <summary>
        /// 判断type是否是数字的类型
        /// </summary>
        /// <param name="type">要判断的type</param>
        /// <returns>当返回NumberType.Unkown时不是数字类型，否则表示是相应的数字类型</returns>
        public static NumberType IsNumberType(Type type)
        {
            if (type == typeof(int))
                return NumberType.Int;
            if (type == typeof(long))
                return NumberType.Long;
            if (type == typeof(short))
                return NumberType.Short;
            if (type == typeof(byte))
                return NumberType.Byte;
            if (type == typeof(sbyte))
                return NumberType.Sbyte;
            if (type == typeof(uint))
                return NumberType.UInt;
            if (type == typeof(ushort))
                return NumberType.UShort;
            if (type == typeof(ulong))
                return NumberType.ULong;
            if (type == typeof(decimal))
                return NumberType.Decimal;
            if (type == typeof(double))
                return NumberType.Double;
            if (type == typeof(float))
                return NumberType.Float;
            return NumberType.Unkown;
        }

        /// <summary>
        /// 判断value是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDigit(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char[] cc = value.ToCharArray();
                foreach (char ch in cc)
                {
                    if (!char.IsDigit(ch))
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
