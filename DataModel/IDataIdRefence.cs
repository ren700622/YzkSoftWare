using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据字段引用表中的Id字段信息
    /// </summary>
    public interface IDataIdRefence
    {

        /// <summary>
        /// 引用的数据模型类型
        /// 对应该类型的字段名称是Id
        /// 如果为null,则引用自身
        /// </summary>
        Type RefenceModalType { get; }

        /// <summary>
        /// 引用RefenceModalType数据模型Id字段的字段名称
        /// </summary>
        string SourceFieldName { get; }

        /// <summary>
        /// 引用RefenceModalType数据模型Id字段是否是父级
        /// </summary>
        bool RefenceIsParent { get; }
    }

    internal class DataIdRefence : IDataIdRefence
    {

        public Type RefenceModalType { get; set; }

        public string SourceFieldName { get; set; }

        public bool RefenceIsParent { get; set; }
    }
}
