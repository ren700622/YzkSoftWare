using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 字段模型索引接口
    /// </summary>
    public interface IDataModelFieldIndex
    {

        /// <summary>
        /// 字段名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 排序
        /// </summary>
        OrderEnum Order { get; }
    }

    /// <summary>
    /// 排序枚举
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "OrderEnum")]
    public enum OrderEnum
    {
        /// <summary>
        /// 升序
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Asc")]
        Asc = 0,
        /// <summary>
        /// 降序
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Desc")]
        Desc = 1

    }

    /// <summary>
    /// 排序索引
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "DataModelFieldIndex")]
    public sealed class DataModelFieldIndex : IDataModelFieldIndex
    {

        /// <summary>
        /// 字段名称
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "FieldName")]
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "OrderEnum")]
        public OrderEnum Order { get; set; }

    }
}
