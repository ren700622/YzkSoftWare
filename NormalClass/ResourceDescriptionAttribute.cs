﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    public sealed class ResourceDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 强类型字符资源构造
        /// </summary>
        /// <param name="resourcetype">强类型字符资源的类型</param>
        /// <param name="staticpropertyname">强类型字符资源的类型的属性名称</param>
        public ResourceDescriptionAttribute(Type resourcetype, string staticpropertyname)
        {
            ResourceDescription = StringResourceGetter.GetResourceString(resourcetype, staticpropertyname);
        }

        public string ResourceDescription { get; private set; }

    }
}
