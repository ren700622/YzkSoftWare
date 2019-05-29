using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Components;

namespace YzkSoftWare.DataService
{
    public interface IDatabaseObjectDefineService : IAppService
    {

        /// <summary>
        /// 添加新数据模型到数据库中
        /// </summary>
        /// <param name="model">要添加的数据模型的类型</param>
        void AddDatabaseModel(Type model);

        /// <summary>
        /// 添加新数据模型到数据库中
        /// </summary>
        /// <typeparam name="T">要添加的数据模型的类型</typeparam>
        void AddDatabaseModel<T>() where T : class,new();

    }

    internal class DatabaseObjectDefineService : AppServiceBase, IDatabaseObjectDefineService
    {

        public void AddDatabaseModel(Type model)
        {
            using (IComponentServiceContainer container = DataServiceProvider.CreateService<IComponentServiceContainer>())
            {
                IDatabaseObjectDefineComponentService cmpservice = ComponentServiceProvider.CreateComponentService<IDatabaseObjectDefineComponentService>(AppName);
                container.AddComponentService(cmpservice);
                cmpservice.AddDatabaseModel(model);
                container.SaveChanged();
            }
        }

        public void AddDatabaseModel<T>() where T : class, new()
        {
            using (IComponentServiceContainer container = DataServiceProvider.CreateService<IComponentServiceContainer>())
            {
                IDatabaseObjectDefineComponentService cmpservice = ComponentServiceProvider.CreateComponentService<IDatabaseObjectDefineComponentService>(AppName);
                container.AddComponentService(cmpservice);
                cmpservice.AddDatabaseModel<T>();
                container.SaveChanged();
            }
        }
    }
}
