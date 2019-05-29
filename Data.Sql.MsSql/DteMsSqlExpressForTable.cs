using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data.Sql.MsSql
{
    public class DteMsSqlExpressForTable<T> : MsSqlExpressBase
        where T : class
    {

        public DteMsSqlExpressForTable()
            : base(null, DataModel.DbTypeForDataModel.Sql, SqlExpressResult.EffectCount)
        {
            Express = MsSqlTableCreateHelper.CreateDefineTableSql(typeof(T).GetDataModel());
        }

        public override object Clone()
        {
            return new DteMsSqlExpressForTable<T>();
        }
    }
}
