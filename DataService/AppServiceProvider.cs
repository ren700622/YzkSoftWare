using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataService
{
    public static class AppServiceProvider
    {

        static AppServiceProvider()
        {
            p_Container = new AppServiceProvider_CC();
            AddAppService<IDatabaseObjectDefineService, DatabaseObjectDefineService>();
        }

        private static AppServiceProvider_CC p_Container;

        /// <summary>
        /// 添加服务组件实现的类型
        /// </summary>
        /// <typeparam name="I">服务组件的接口类型,必须是继承自IAppService接口的接口类型</typeparam>
        /// <typeparam name="S">实现I的类型,必须是实现了IAppService接口的非抽象且有无参的公共构造函数的类型</typeparam>
        public static void AddAppService<I, S>()
            where I : IAppService
            where S : IAppService
        {
            p_Container.AddOrReplaceValue(typeof(I), typeof(S));
        }

        /// <summary>
        /// 创建服务组件
        /// </summary>
        /// <typeparam name="I">要创建的服务组件的接口类型</typeparam>
        /// <returns>返回实现接口I的服务组件</returns>
        public static I CreateAppService<I>(string appname)
            where I : IAppService
        {
            Type p = p_Container.GetValue(typeof(I));
            if (p != null)
            {
                object o = p.CreateObject();
                if (o != null)
                {
                    I x = (I)o;
                    x.AppName = appname;
                    return x;
                }
            }
            return default(I);
        }

    }

    [Synchronization]
    internal class AppServiceProvider_CC : ObjectProvider<Type, Type>
    {

        private void CheckIsValidKeyValue(Type key, Type value)
        {
            if (key == null)
                throw new ObjectIsNullException("key");
            if (!(key.IsInterface && key.FindInterface(typeof(IAppService))))
                throw new UnkownCodeException(ErrorCodes.UnkownError, string.Format(LocalResource.AppServiceProvider_Error_Key, key.FullName));
            if (value != null && value.IsClass && !value.IsInterface && value.FindInterface(typeof(IAppService)) && value.GetConstructor(ObjectExtendDescription.EmptyTypes) != null)
                return;
            throw new UnkownCodeException(ErrorCodes.UnkownError, string.Format(LocalResource.AppServiceProvider_Error_Value, key.FullName));
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
