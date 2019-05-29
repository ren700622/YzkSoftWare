using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 所有具有Id字段的数据库表的基类
    /// </summary>
    [DatabaseObject(DbTypeForDataModel.Table)]
    public abstract class IdDataModelBase : IDataId
    {

        protected IdDataModelBase()
        {
            Id = DataModelHelpers.CreateDataId(this.GetType());
        }

        /// <summary>
        /// 记录主键:按起始值一每次自动加一生成
        /// </summary>
        [DbGenerated]
        [PrimaryKey]
        [ResourceDisplayName(typeof(LocalResource), "Model_Field_Id_DisplayName")]
        [ResourceDescription(typeof(LocalResource), "Model_Field_Id_Description")]
        public long Id { get; set; }
    }
}
