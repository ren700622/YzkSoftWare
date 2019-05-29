using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 定义类型是数据库对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DatabaseObjectAttribute : Attribute
    {

        public DatabaseObjectAttribute(DbTypeForDataModel dbtype)
        {
            DbType = dbtype;
        }

        /// <summary>
        /// 数据库对象类型
        /// </summary>
        public DbTypeForDataModel DbType { get; private set; }

    }
}
