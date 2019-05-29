using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 指示是否是软删除字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SoftDeleteFieldAttribute : Attribute
    {
        public SoftDeleteFieldAttribute()
        {
            IsSoftDelete = true;
        }
        public bool IsSoftDelete { get; set; }

    }
}
