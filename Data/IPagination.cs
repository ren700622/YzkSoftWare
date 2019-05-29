using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 分页信息接口
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// 当前页索引
        /// </summary>
        int CurrentPageIndex { get; }
        /// <summary>
        /// 分页数据每页包含的记录数
        /// </summary>
        int PageRowNumber { get; }
    }

    /// <summary>
    /// 分页信息
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "PaginationInfo")]
    public class PaginationInfo : IPagination
    {

        /// <summary>
        /// 当前页索引
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "CurrentPageIndex")]
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 分页数据每页包含的记录数
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "PageRowNumber")]
        public int PageRowNumber { get; set; }
    }
}
