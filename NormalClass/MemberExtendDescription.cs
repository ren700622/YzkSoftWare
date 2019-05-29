using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataModel;

namespace YzkSoftWare
{
    public static class PropertyExtendDescription
    {

        /// <summary>
        /// 获取属性说明符上的显示名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDisplayName(this PropertyDescriptor member)
        {
            ResourceDisplayNameAttribute attr = member.Attributes[typeof(ResourceDisplayNameAttribute)] as ResourceDisplayNameAttribute;
            if (attr != null)
                return attr.ResourceDisplayName;
            DisplayNameAttribute attr2 = member.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
            if (attr2 != null)
                return attr2.DisplayName;
            return null;
        }

        /// <summary>
        /// 获取属性说明符上的说明文本
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDescription(this PropertyDescriptor member)
        {
            ResourceDescriptionAttribute attr = member.Attributes[typeof(ResourceDescriptionAttribute)] as ResourceDescriptionAttribute;
            if (attr != null)
                return attr.ResourceDescription;
            DescriptionAttribute attr2 = member.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
            if (attr2 != null)
                return attr2.Description;
            return null;
        }

        /// <summary>
        /// 获取字段说明符上的显示名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDisplayName(this FieldInfo field)
        {
            if (Attribute.IsDefined(field, typeof(ResourceDisplayNameAttribute)))
            {
                ResourceDisplayNameAttribute att = Attribute.GetCustomAttribute(field, typeof(ResourceDisplayNameAttribute)) as ResourceDisplayNameAttribute;
                return att.ResourceDisplayName;
            }
            if (Attribute.IsDefined(field, typeof(DisplayNameAttribute)))
            {
                DisplayNameAttribute att = Attribute.GetCustomAttribute(field, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                return att.DisplayName;
            }
            return null;
        }

        /// <summary>
        /// 获取字段说明符上的说明文本
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDescription(this FieldInfo field)
        {
            if (Attribute.IsDefined(field, typeof(ResourceDescriptionAttribute)))
            {
                ResourceDescriptionAttribute att = Attribute.GetCustomAttribute(field, typeof(ResourceDescriptionAttribute)) as ResourceDescriptionAttribute;
                return att.ResourceDescription;
            }
            if (Attribute.IsDefined(field, typeof(DescriptionAttribute)))
            {
                DescriptionAttribute att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return att.Description;
            }
            return null;
        }

        /// <summary>
        /// 获取指定属性在数据库中的名称
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetNameForDatabase(this PropertyDescriptor member)
        {
            return DataModelHelper.GetDataFieldModelNameForDatabase(member);
        }

    }
}
