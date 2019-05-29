using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;

namespace YzkSoftWare.DataModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinMaxValueAttribute : Attribute
    {

        /// <summary>
        /// 使用如下的表达式表示最大和最小值
        /// 格式如：MinValue,MaxValue(用*代替无限制,如*,MaxValue和MinValue,*)
        /// </summary>
        /// <param name="valueexpress"></param>
        public MinMaxValueAttribute(string valueexpress)
        {
            ValueExpress = valueexpress;
        }

        /// <summary>
        /// 获取最大和最小值表达式
        /// 格式如：MinValue,MaxValue(用*代替无限制,如*,MaxValue和MinValue,*)
        /// </summary>
        public string ValueExpress { get; private set; }
    }

    public static class MinMaxValueHelper
    {
        /// <summary>
        /// 根据表达式获取最大和最小值
        /// </summary>
        /// <param name="valueexp">表达式</param>
        /// <param name="valuetype">表达式值类型</param>
        /// <param name="minvalue">返回最小值</param>
        /// <param name="maxvalue">返回最大值</param>
        public static void GetMinMaxValue(string valueexp, Type valuetype, out object minvalue, out object maxvalue)
        {
            if (string.IsNullOrEmpty(valueexp))
                throw new InvalidOperationException(string.Format(LocalResource.InvalidExpress, valueexp));
            string[] y = valueexp.Split(new char[] { ',' });
            if (y.Length != 2)
                throw new InvalidOperationException(string.Format(LocalResource.InvalidExpress, valueexp));
            minvalue = null;
            maxvalue = null;
            if (valuetype == typeof(DateTime))
            {
                if (y[0] != "*")
                {
                    DateTime d;
                    if (DateTime.TryParse(y[0], out d))
                        minvalue = d;
                    else
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                }
                if (y[1] != "*")
                {
                    DateTime d;
                    if (DateTime.TryParse(y[1], out d))
                        maxvalue = d;
                    else
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                }
            }
            else if (valuetype == typeof(TimeSpan))
            {
                if (y[0] != "*")
                {
                    TimeSpan d;
                    if (TimeSpan.TryParse(y[0], out d))
                        minvalue = d;
                    else
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                }
                if (y[1] != "*")
                {
                    TimeSpan d;
                    if (TimeSpan.TryParse(y[1], out d))
                        maxvalue = d;
                    else
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                }
            }
            else
            {
                NumberType nv = DataValueParase.IsNumberType(valuetype);
                if (nv == NumberType.Unkown)
                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                switch (nv)
                {
                    case NumberType.Byte:
                        {
                            if (y[0] != "*")
                            {
                                byte d;
                                if (byte.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                byte d;
                                if (byte.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Decimal:
                        {
                            if (y[0] != "*")
                            {
                                decimal d;
                                if (decimal.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                decimal d;
                                if (decimal.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Double:
                        {
                            if (y[0] != "*")
                            {
                                double d;
                                if (double.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                double d;
                                if (double.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Float:
                        {
                            if (y[0] != "*")
                            {
                                float d;
                                if (float.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                float d;
                                if (float.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Int:
                        {
                            if (y[0] != "*")
                            {
                                int d;
                                if (int.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                int d;
                                if (int.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Long:
                        {
                            if (y[0] != "*")
                            {
                                long d;
                                if (long.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                long d;
                                if (long.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Sbyte:
                        {
                            if (y[0] != "*")
                            {
                                sbyte d;
                                if (sbyte.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                sbyte d;
                                if (sbyte.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.Short:
                        {
                            if (y[0] != "*")
                            {
                                short d;
                                if (short.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                short d;
                                if (short.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.UInt:
                        {
                            if (y[0] != "*")
                            {
                                uint d;
                                if (uint.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                uint d;
                                if (uint.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.ULong:
                        {
                            if (y[0] != "*")
                            {
                                ulong d;
                                if (ulong.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                ulong d;
                                if (ulong.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                    case NumberType.UShort:
                        {
                            if (y[0] != "*")
                            {
                                ushort d;
                                if (ushort.TryParse(y[0], out d))
                                    minvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            if (y[1] != "*")
                            {
                                ushort d;
                                if (ushort.TryParse(y[1], out d))
                                    maxvalue = d;
                                else
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 判断value值是否在valueexp表达式范围中
        /// </summary>
        /// <param name="valueexp">表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool ValueIsInRangle(string valueexp, object value)
        {
            if (string.IsNullOrEmpty(valueexp))
                throw new InvalidOperationException(string.Format(LocalResource.InvalidExpress, valueexp));
            if (value == null)
                throw new NullReferenceException(string.Format(LocalResource.ParametIsNull, "value"));
            string[] y = valueexp.Split(new char[] { ',' });
            if (y.Length != 2)
                throw new InvalidOperationException(string.Format(LocalResource.InvalidExpress, valueexp));
            Type valuetype = value.GetType();
            if (valuetype == typeof(DateTime))
            {
                if (y[0] != "*")
                {
                    DateTime d;
                    if (!DateTime.TryParse(y[0], out d))
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                    if ((DateTime)value < d)
                        return false;
                }
                if (y[1] != "*")
                {
                    DateTime d;
                    if (!DateTime.TryParse(y[1], out d))
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                    if ((DateTime)value > d)
                        return false;
                }
                return true;
            }
            else if (valuetype == typeof(TimeSpan))
            {
                if (y[0] != "*")
                {
                    TimeSpan d;
                    if (!TimeSpan.TryParse(y[0], out d))
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                    if ((TimeSpan)value < d)
                        return false;
                }
                if (y[1] != "*")
                {
                    TimeSpan d;
                    if (!TimeSpan.TryParse(y[1], out d))
                        throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                    if ((TimeSpan)value > d)
                        return false;
                }
                return true;
            }
            else
            {
                NumberType nv = DataValueParase.IsNumberType(valuetype);
                if (nv == NumberType.Unkown)
                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                switch (nv)
                {
                    case NumberType.Byte:
                        {
                            if (y[0] != "*")
                            {
                                byte d;
                                if (!byte.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((byte)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                byte d;
                                if (!byte.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((byte)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Decimal:
                        {
                            if (y[0] != "*")
                            {
                                decimal d;
                                if (!decimal.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((decimal)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                decimal d;
                                if (!decimal.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((decimal)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Double:
                        {
                            if (y[0] != "*")
                            {
                                double d;
                                if (!double.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((double)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                double d;
                                if (!double.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((double)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Float:
                        {
                            if (y[0] != "*")
                            {
                                float d;
                                if (!float.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((float)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                float d;
                                if (!float.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((float)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Int:
                        {
                            if (y[0] != "*")
                            {
                                int d;
                                if (!int.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((int)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                int d;
                                if (!int.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((int)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Long:
                        {
                            if (y[0] != "*")
                            {
                                long d;
                                if (!long.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((long)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                long d;
                                if (!long.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((long)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Sbyte:
                        {
                            if (y[0] != "*")
                            {
                                sbyte d;
                                if (!sbyte.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((sbyte)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                sbyte d;
                                if (!sbyte.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((sbyte)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.Short:
                        {
                            if (y[0] != "*")
                            {
                                short d;
                                if (!short.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((short)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                short d;
                                if (!short.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((short)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.UInt:
                        {
                            if (y[0] != "*")
                            {
                                uint d;
                                if (!uint.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((uint)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                uint d;
                                if (!uint.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((uint)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.ULong:
                        {
                            if (y[0] != "*")
                            {
                                ulong d;
                                if (!ulong.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((ulong)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                ulong d;
                                if (!ulong.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((ulong)value > d)
                                    return false;
                            }
                            return true;
                        }
                    case NumberType.UShort:
                        {
                            if (y[0] != "*")
                            {
                                ushort d;
                                if (!ushort.TryParse(y[0], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((ushort)value < d)
                                    return false;
                            }
                            if (y[1] != "*")
                            {
                                ushort d;
                                if (!ushort.TryParse(y[1], out d))
                                    throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                                if ((ushort)value > d)
                                    return false;
                            }
                            return true;
                        }
                    default:
                        {
                            throw new InvalidOperationException(string.Format(LocalResource.InvalidExpressForType, valuetype, valueexp));
                        }
                }
            }
        }
    }


}
