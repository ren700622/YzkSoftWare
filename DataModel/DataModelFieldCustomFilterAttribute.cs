using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 定义数据模型字段过滤器的基类的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class DataModelFieldCustomFilterAttribute : Attribute
    {

        /// <summary>
        /// 判断指定的属性是否是数据库字段
        /// </summary>
        /// <param name="pd">要判断的属性</param>
        /// <returns>如果pd是一个数据库字段则返回true,否则返回false</returns>
        public abstract bool IsDefineFeild(PropertyDescriptor pd);

    }
}
