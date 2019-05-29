using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 行数据读取器
    /// </summary>
    public interface IDataRowReader
    {

        /// <summary>
        /// 获取指定字段名称的值
        /// </summary>
        /// <param name="name">要获取值的字段名称</param>
        /// <returns>返回指定字段的值(如果存在的话),否则引发异常ObjectIsNullException</returns>
        object this[string name] { get; }

    }

    /// <summary>
    /// 行数据设置器
    /// </summary>
    public interface IDataRowSetter
    {
        /// <summary>
        /// 设置指定字段名称的值
        /// </summary>
        /// <param name="name">要设置值的字段名称</param>
        /// <param name="value">要设置的值</param>
        void SetFieldValue(string name, object value);

    }
}
