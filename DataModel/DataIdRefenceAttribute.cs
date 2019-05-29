using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 引用特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class DataIdRefenceAttribute : Attribute
    {

        public DataIdRefenceAttribute(Type refencetype, string sourcename)
        {
            RefenceModalType = refencetype;
            SourceFieldName = sourcename;
        }

        public DataIdRefenceAttribute(string sourcename)
            : this(null, sourcename)
        {
        }

        /// <summary>
        /// 引用的数据模型类型
        /// 对应该类型的字段名称是Id
        /// 如果为null,则引用自身
        /// </summary>
        public Type RefenceModalType { get; private set; }

        /// <summary>
        /// 引用RefenceModalType数据模型Id字段的字段名称
        /// </summary>
        public string SourceFieldName { get; private set; }

        /// <summary>
        /// 引用RefenceModalType数据模型Id字段是否是父级
        /// </summary>
        public bool RefenceIsParent { get; set; }

    }

    /// <summary>
    /// 删除时检查引用的特性
    /// 在父表使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class DeleteCheckRefenceAttribute : Attribute
    {

        public DeleteCheckRefenceAttribute(Type checkmodalType, string refencefieldname)
        {
            CheckModalType = checkmodalType;
            RefenceFieldName = refencefieldname;
        }

        public DeleteCheckRefenceAttribute(string refencefieldname)
            : this(null, refencefieldname)
        {

        }

        /// <summary>
        /// 删除时要一起删除的子类数据模型类型
        /// 如果为null,则引用自身
        /// </summary>
        public Type CheckModalType { get; private set; }

        /// <summary>
        /// 删除时要检查的数据模型的字段名称
        /// 该字段名称与父级的Id字段对应
        /// </summary>
        public string RefenceFieldName { get; private set; }

        /// <summary>
        /// false:则检测该引用
        /// true:则不检测该引用,删除时同时删除该引用
        /// </summary>
        public bool IsChild { get; set; }
    }
}
