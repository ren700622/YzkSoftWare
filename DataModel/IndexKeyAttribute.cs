using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 确定字段是索引
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class IndexKeyAttribute : Attribute
    {
        public IndexKeyAttribute(bool isIndexKey,OrderEnum order)
        {
            IsIndexKey = isIndexKey;
            Order = order;
        }

        public IndexKeyAttribute(OrderEnum order)
            : this(true, order)
        {

        }

        public IndexKeyAttribute()
            : this(OrderEnum.Asc)
        {

        }

        /// <summary>
        /// 是否索引
        /// </summary>
        public bool IsIndexKey { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public OrderEnum Order { get; private set; }
    }
}
