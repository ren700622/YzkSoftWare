using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据模型的帮助方法
    /// </summary>
    public static class DataModelHelpers
    {

        private static System.Collections.Concurrent.ConcurrentDictionary<Type, long> p_DataModelIds = new System.Collections.Concurrent.ConcurrentDictionary<Type, long>();

        /// <summary>
        /// 为指定的数据模型创建新的临时Id
        /// 该Id必定是小于0的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static long CreateDataId(Type type)
        {
            return p_DataModelIds.AddOrUpdate(type, -1, (t, v) => UpdateDataId(t, v));
        }

        internal static long UpdateDataId(Type type, long value)
        {
            if (value == long.MinValue)
                return -1;
            return value - 1;
        }

    }
}
