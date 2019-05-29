using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 表示数据库对象名称的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DatabaseObjectNameAttribute : Attribute
    {

        /// <summary>
        /// 使用数据库对象名称的构造
        /// </summary>
        /// <param name="name"></param>
        public DatabaseObjectNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            Name = name;
        }

        /// <summary>
        /// 使用与model同名称的构造函数
        /// </summary>
        /// <param name="modal"></param>
        public DatabaseObjectNameAttribute(Type model)
            : this(DataModelHelper.GetDataModelNameForDatabase(model))
        {
            
        }

        /// <summary>
        /// 获取数据对象的名称
        /// </summary>
        public string Name { get; private set; }
    }
}
