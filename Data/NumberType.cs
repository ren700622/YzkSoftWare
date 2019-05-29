using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 数字类型
    /// </summary>
    public enum NumberType
    {
        Unkown = 0,
        /// <summary>
        /// Int32
        /// </summary>
        Int = 1,
        /// <summary>
        /// int64
        /// </summary>
        Long = 2,
        /// <summary>
        /// byte
        /// </summary>
        Byte = 3,
        /// <summary>
        /// int16
        /// </summary>
        Short = 4,
        /// <summary>
        /// uint32
        /// </summary>
        UInt = 5,
        /// <summary>
        /// uint64
        /// </summary>
        ULong = 6,
        /// <summary>
        /// uint16
        /// </summary>
        UShort = 7,
        /// <summary>
        /// ubyte
        /// </summary>
        Sbyte = 8,
        /// <summary>
        /// decimal
        /// </summary>
        Decimal = 9,
        /// <summary>
        /// double
        /// </summary>
        Double = 10,
        /// <summary>
        /// float
        /// </summary>
        Float = 11
    }
}
