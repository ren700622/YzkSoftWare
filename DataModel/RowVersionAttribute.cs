using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 确定该字段模型是行版本
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class RowVersionAttribute : Attribute
    {
    }
}
