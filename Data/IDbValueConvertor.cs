using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataService;
using YzkSoftWare.Formates;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 数据库值转换
    /// </summary>
    public interface IDbValueConvertor
    {

        /// <summary>
        /// 将数据库字段值转换为对象成员值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object ParseValueFromDbValue(object value, Type objectType);

        /// <summary>
        /// 将对象成员值转换为数据库值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object FormateValueToDbValue(object value);
    }

    /// <summary>
    /// 使用DataServiceProvider.CreateService[IObjectFormater]方法提供的格式化对象格式化数据库值
    /// 使用该类作为格式化类的格式化对象成员中有数组时要在该成员定义XmlArray和XmlArrayItem特殊
    /// </summary>
    /// <typeparam name="D"></typeparam>
    public sealed class CurrentObjectFormateDbConvertor : IDbValueConvertor
    {

        public object ParseValueFromDbValue(object value, Type objectType)
        {
            if (value is string)
            {
                IObjectFormater f = DataServiceProvider.CreateService<IObjectFormater>();
                return f.DeserializeObject((string)value, objectType);
            }
            return null;
        }

        public object FormateValueToDbValue(object value)
        {
            IObjectFormater f = DataServiceProvider.CreateService<IObjectFormater>();
            return f.SerializeObject(value);
        }
    }
}
