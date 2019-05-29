using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 指示如何排序的接口
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// 排序成员名称
        /// </summary>
        string OrderMemberName { get; }

        /// <summary>
        /// 排序类型
        /// </summary>
        FieldOrder FieldOrder { get; }
    }

    /// <summary>
    /// 排序类型枚举
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "FieldOrder")]
    public enum FieldOrder
    {
        /// <summary>
        /// 未知
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "Unkown")]
        Unkown = 0,
        /// <summary>
        /// 升序
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "ASC")]
        ASC = 1,
        /// <summary>
        /// 降序
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "DESC")]
        DESC = 2
    }

    /// <summary>
    /// 指示如何排序
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "FieldSortOrder")]
    public class FieldSortOrder : IOrder
    {

        public FieldSortOrder()
        {
            FieldOrder = Data.FieldOrder.ASC;
        }

        /// <summary>
        /// 排序成员名称
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "OrderMemberName")]
        public string OrderMemberName { get; set; }

        /// <summary>
        /// 排序类型枚举
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "FieldOrder")]
        public FieldOrder FieldOrder { get; set; }
    }
}
