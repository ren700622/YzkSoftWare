using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Components
{
    /// <summary>
    /// 服务组件(IComponentService)提供者
    /// </summary>
    public static class ComponentServiceProvider
    {

        static ComponentServiceProvider()
        {
            p_Container = new ComponentServiceProvider_CC();
            AddComponentService<IDatabaseObjectDefineComponentService, DatabaseObjectDefineComponentService>();
        }

        private static ComponentServiceProvider_CC p_Container;

        /// <summary>
        /// 添加服务组件实现的类型
        /// </summary>
        /// <typeparam name="I">服务组件的接口类型,必须是继承自IComponentService接口的接口类型</typeparam>
        /// <typeparam name="S">实现I的类型,必须是实现了IComponentService接口的非抽象且有无参的公共构造函数的类型</typeparam>
        public static void AddComponentService<I, S>()
            where I : IComponentService
            where S : IComponentService
        {
            p_Container.AddOrReplaceValue(typeof(I), typeof(S));
        }

        /// <summary>
        /// 创建服务组件
        /// </summary>
        /// <typeparam name="I">要创建的服务组件的接口类型</typeparam>
        /// <returns>返回实现接口I的服务组件</returns>
        public static I CreateComponentService<I>(string appname)
            where I : IComponentService
        {
            Type p = p_Container.GetValue(typeof(I));
            if (p != null)
            {
                object o = p.CreateObject();
                if (o != null)
                {
                    I x = (I)o;
                    x.CurrentAppName = appname;
                    return x;
                }
            }
            return default(I);
        }
    }

    internal class ComponentServiceProvider_CC : ObjectProvider<Type,Type>
    {

        private void CheckIsValidKeyValue(Type key, Type value)
        {
            if (key == null)
                throw new ObjectIsNullException("key");
            if (!(key.IsInterface && key.FindInterface(typeof(IComponentService))))
                throw new UnkownCodeException(ErrorCodes.UnkownError, string.Format(LocalResource.ComponentServiceProvider_Error_Key, key.FullName));
            if (value != null && value.IsClass && !value.IsInterface && value.FindInterface(typeof(IComponentService)) && value.GetConstructor(ObjectExtendDescription.EmptyTypes) != null)
                return;
            throw new UnkownCodeException(ErrorCodes.UnkownError, string.Format(LocalResource.ComponentServiceProvider_Error_Value, key.FullName));
        }

        protected override void OnAdd(Type key, Type value)
        {
            base.OnAdd(key, value);
            CheckIsValidKeyValue(key, value);
        }

        protected override void OnReplace(Type key, Type value)
        {
            base.OnReplace(key, value);
            CheckIsValidKeyValue(key, value);
        }

    }
}
