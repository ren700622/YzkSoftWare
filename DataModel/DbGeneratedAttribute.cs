using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 定义数据库字段的自动增加属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DbGeneratedAttribute : Attribute
    {

        public DbGeneratedAttribute(int start, int step)
        {
            StartValue = start;
            Step = step;
        }

        public DbGeneratedAttribute() : this(1, 1) { }

        /// <summary>
        /// 起始值
        /// </summary>
        public int StartValue { get; private set; }

        /// <summary>
        /// 步长
        /// </summary>
        public int Step { get; private set; }

    }
}
