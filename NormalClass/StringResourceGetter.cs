using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 强类型字符资源获取
    /// </summary>
    public static class StringResourceGetter
    {

        public static string GetResourceString(Type type, string key)
        {
            var x = type.GetProperty(key, BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            if (x != null)
                return (string)x.GetValue(null);
            return null;
        }

    }
}
