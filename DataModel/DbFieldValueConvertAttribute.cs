using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据库字段值转换特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DbFieldValueConvertAttribute : TypeSetAttribute
    {

        public DbFieldValueConvertAttribute(Type typevalue) : base(typevalue) { }

    }
}
