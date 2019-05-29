using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Components;
using YzkSoftWare.Database;
using YzkSoftWare.Encrypt;
using YzkSoftWare.Formates;

namespace YzkSoftWare.DataService
{
    /// <summary>
    /// 服务创建容器
    /// </summary>
    public static class DataServiceProvider
    {

        static DataServiceProvider()
        {
            AddDataService<IObjectFormater, XmlObjectFormater>();
            AddDataService<ISimpleEncrypt, DESSimpleEncrypt>();
            AddDataService<IComponentDatabaseContainer, ComponentDatabaseContainer>();
            AddDataService<IComponentServiceContainer, ComponentServiceContainer>();
        }

        private static readonly Dictionary<Type, Type> p_Services = new Dictionary<Type, Type>();

        /// <summary>
        /// 添加数据服务接口
        /// </summary>
        /// <param name="serviceinterface">接口</param>
        /// <param name="serviceType">该接口可无参数实例化的类型</param>
        public static void AddDataService(Type serviceinterface, Type serviceType)
        {
            if (serviceinterface != null && serviceinterface.IsInterface && serviceType != null && serviceType.IsClass && !serviceType.IsInterface)
            {
                if (p_Services.ContainsKey(serviceinterface))
                    p_Services[serviceinterface] = serviceType;
                else
                    p_Services.Add(serviceinterface, serviceType);
            }
        }

        /// <summary>
        /// 添加数据服务接口
        /// </summary>
        /// <typeparam name="I">接口</typeparam>
        /// <typeparam name="T">该接口可无参数实例化的类型</typeparam>
        public static void AddDataService<I, T>()
        {
            AddDataService(typeof(I), typeof(T));
        }

        /// <summary>
        /// 创建数据服务接口
        /// </summary>
        /// <param name="serviceinterface"></param>
        /// <returns></returns>
        public static object CreateService(Type serviceinterface)
        {
            if (serviceinterface != null && serviceinterface.IsInterface && p_Services.ContainsKey(serviceinterface))
                return p_Services[serviceinterface].CreateObject();
            return null;
        }

        /// <summary>
        /// 创建数据服务接口
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <returns></returns>
        public static I CreateService<I>()
        {
            object o = CreateService(typeof(I));
            if (o is I)
                return (I)o;
            return default(I);
        }

    }
}
