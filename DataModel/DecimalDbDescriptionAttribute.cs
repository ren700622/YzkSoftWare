using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 定义decimal在数据库中保留小数点数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DecimalDbDescriptionAttribute : Attribute
    {

        public DecimalDbDescriptionAttribute()
        {
            DecimalLength = 18;
            DotLength = 2;
        }

        public int DecimalLength { get; set; }

        public int DotLength { get; set; }

    }
}
