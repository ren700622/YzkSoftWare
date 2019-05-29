using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 定义数据字段值最大长度的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MaxValueLengthAttribute : Attribute
    {

        public MaxValueLengthAttribute(int maxValue)
        {
            MaxValue = maxValue;
        }

        public MaxValueLengthAttribute() : this(50) { }

        /// <summary>
        /// 获取数据字段值最大长度
        /// </summary>
        public int MaxValue { get; private set; }
    }
}
