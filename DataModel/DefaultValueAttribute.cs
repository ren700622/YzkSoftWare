using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 字段缺省默认值特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DefaultValueAttribute : Attribute
    {

        public DefaultValueAttribute(string defaultvalue) { DefaultValue = defaultvalue; }

        /// <summary>
        /// 缺省默认值
        /// </summary>
        public string DefaultValue { get; private set; }

    }
}
