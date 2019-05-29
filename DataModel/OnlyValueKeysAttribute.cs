using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据模型中唯一值特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OnlyValueKeysAttribute : Attribute
    {

        public OnlyValueKeysAttribute(string[] onlykeys)
        {
            OnlyKeys = onlykeys;
            IsIncludeAllNull = false;
        }

        /// <summary>
        /// 指定的字段值组合唯一
        /// </summary>
        public string[] OnlyKeys { get; private set; }

        /// <summary>
        /// false；当指定的字段值全为null时忽略检测,这是默认值
        /// true:当指定的字段值全为null时也是一个唯一组合,需要检测
        /// </summary>
        public bool IsIncludeAllNull { get; set; }
    }
}
