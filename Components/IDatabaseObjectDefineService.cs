using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;
using YzkSoftWare.Data.Sql;

namespace YzkSoftWare.Components
{
    /// <summary>
    /// 数据库模型定义组件
    /// </summary>
    public interface IDatabaseObjectDefineComponentService : IComponentService
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

    internal sealed class DatabaseObjectDefineComponentService : ComponentServiceBase, IDatabaseObjectDefineComponentService
    {
        public void AddDatabaseModel(Type model)
        {
            AddUpdateContext(new DatabaseModelDefineUpdate() { ModelType = model });
        }

        public void AddDatabaseModel<T>() where T : class, new()
        {
            AddUpdateContext(new DatabaseModelDefineUpdate() { ModelType = typeof(T) });
        }
    }

    internal class DatabaseModelDefineUpdate : IUpdateContext
    {

        public Type ModelType { get; set; }

        public ISqlExpress Update(string dbtypeName)
        {
            var dt = ModelType.GetDataModel().DbType;
            switch (dt)
            {
                case DataModel.DbTypeForDataModel.Table:
                    return QuerySqlFactory.CreateTableDefineSql(dbtypeName, ModelType);
                default:
                    throw new UnkownCodeException(ErrorCodes.UnkownError, string.Format(LocalResource.NotSupportCreateDatabaseObject, dt.GetEnumDisplayName()));
            }
            
        }

        public IUpdateDataNoty UpdateDataNoty { get; set; }

        public bool SuccessUpdate { get; private set; }

        public object SuccessReturn { get; private set; }

        public void SuccessCommitToDatabase(object returnvalue)
        {
            SuccessReturn = returnvalue;
            SuccessUpdate = true;
        }
    }
}
