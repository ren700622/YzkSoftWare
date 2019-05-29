using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 扩展属性支持接口
    /// </summary>
    public interface IExtendProperty
    {

        /// <summary>
        /// 获取当前已有的所有属性名称集合
        /// </summary>
        IEnumerable<string> PropertyNames { get; }

        /// <summary>
        /// 指示当前是否包含指定name的属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasProperty(string name);

        /// <summary>
        /// 获取扩展属性
        /// </summary>
        /// <param name="propername">扩展属性名称</param>
        /// <returns>如果不存在名为propertyname的扩展属性则返回null</returns>
        object GetExtendPropertyValue(string propername);

        /// <summary>
        /// 设置扩展属性
        /// </summary>
        /// <param name="propername">扩展属性名称</param>
        /// <param name="value">扩展属性值</param>
        void SetExtendPropertyValue(string propername, object value);

        /// <summary>
        /// 获取扩展属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyname">扩展属性名称</param>
        /// <param name="defaultvalue">不存在时返回的值</param>
        /// <returns>如果不存在名为propertyname的扩展属性则返回defaultvalue</returns>
        T GetExtendPropertyValue<T>(string propertyname, T defaultvalue);
    }

    public class ExtendProperty : IExtendProperty
    {

        public ExtendProperty()
        {
            p_ExtendPropertyValues = new Dictionary<string, object>();
        }


        private IDictionary<string, object> p_ExtendPropertyValues;

        protected virtual T GetExtendPropertyValue<T>(string propertyname, T defaultvalue)
        {
            if (!string.IsNullOrEmpty(propertyname))
            {
                if (p_ExtendPropertyValues.ContainsKey(propertyname))
                    return (T)p_ExtendPropertyValues[propertyname];
            }
            return defaultvalue;
        }

        protected virtual object GetExtendPropertyValue(string propername)
        {
            if (!string.IsNullOrEmpty(propername))
            {
                if (p_ExtendPropertyValues.ContainsKey(propername))
                    return p_ExtendPropertyValues[propername];
            }
            return null;
        }

        protected virtual void SetExtendPropertyValue(string propername, object value)
        {
            if (!string.IsNullOrEmpty(propername))
            {
                if (p_ExtendPropertyValues.ContainsKey(propername))
                    p_ExtendPropertyValues[propername] = value;
                else
                    p_ExtendPropertyValues.Add(propername, value);
            }
        }

        object IExtendProperty.GetExtendPropertyValue(string propername)
        {
            return GetExtendPropertyValue(propername);
        }

        void IExtendProperty.SetExtendPropertyValue(string propername, object value)
        {
            SetExtendPropertyValue(propername, value);
        }

        T IExtendProperty.GetExtendPropertyValue<T>(string propertyname, T defaultvalue)
        {
            return GetExtendPropertyValue<T>(propertyname, defaultvalue);
        }

        IEnumerable<string> IExtendProperty.PropertyNames
        {
            get { return p_ExtendPropertyValues.Keys.ToArray(); }
        }

        bool IExtendProperty.HasProperty(string name)
        {
            return p_ExtendPropertyValues.ContainsKey(name);
        }
    }
}
