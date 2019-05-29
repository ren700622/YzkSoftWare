using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 确定该字段模型是主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {

        public PrimaryKeyAttribute(bool isPrimaryKey,OrderEnum order)
        {
            IsPrimaryKey = isPrimaryKey;
            Order = order;
        }

        public PrimaryKeyAttribute(OrderEnum order)
            : this(true, order)
        {

        }

        public PrimaryKeyAttribute()
            : this(OrderEnum.Asc)
        {

        }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public OrderEnum Order { get; private set; }
    }
}
