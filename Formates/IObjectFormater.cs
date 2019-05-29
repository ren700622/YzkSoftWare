using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Formates
{
    /// <summary>
    /// 对象字符格式化接口
    /// </summary>
    public interface IObjectFormater
    {
        /// <summary>
        /// 序列化对象为字符串
        /// </summary>
        /// <param name="value">要序列化的对象</param>
        /// <returns>返回序列化完成的字符</returns>
        string SerializeObject(object value);
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="inputvalue">要反序列化的字符值</param>
        /// <param name="valuetype">对象类型</param>
        /// <returns>已反序列化的对象</returns>
        object DeserializeObject(string inputvalue, Type valuetype);
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="inputvalue">要反序列化的字符值</param>
        /// <returns>已反序列化的对象</returns>
        T DeserializeObject<T>(string inputvalue);
    }

    /// <summary>
    /// Xml序列化接口的实现
    /// </summary>
    internal class XmlObjectFormater : IObjectFormater
    {

        string IObjectFormater.SerializeObject(object value)
        {
            if (value == null) return null;
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(value.GetType());
            StringBuilder xb = new StringBuilder();
            using (System.IO.StringWriter w = new System.IO.StringWriter(xb))
            {
                xml.Serialize(w, value);
                w.Close();
            }
            return xb.ToString();
        }

        private object DeserializeObject(string inputvalue, Type valuetype)
        {
            if (string.IsNullOrEmpty(inputvalue))
                return null;
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(valuetype);
            using (System.IO.StringReader w = new System.IO.StringReader(inputvalue))
            {
                object ro = xml.Deserialize(w);
                w.Close();
                return ro;
            }
        }

        object IObjectFormater.DeserializeObject(string inputvalue, Type valuetype)
        {
            return DeserializeObject(inputvalue, valuetype);
        }

        T IObjectFormater.DeserializeObject<T>(string inputvalue)
        {
            return (T)DeserializeObject(inputvalue, typeof(T));
        }

    }
}
