using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 指示该属性不是数据库字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class NoneDatabaseFieldAttribute : Attribute
    {

        /// <summary>
        /// 是否在数据库对象中定义该字段
        /// </summary>
        public bool IsDefineOnDb { get; set; }

    }
}
