using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据模型检查特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DataUpdateCheckersAttribute : TypeSetAttribute
    {

        public DataUpdateCheckersAttribute(Type typevalue) : base(typevalue) { }

    }
}
