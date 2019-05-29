using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data.Sql.MsSql
{
    /// <summary>
    /// 删除数据MS-SQL的Sql表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DeleteMsSqlExpress<T> : MsSqlExpressBase
        where T : class
    {
        public DeleteMsSqlExpress()
            : base(null, DataModel.DbTypeForDataModel.Sql, SqlExpressResult.EffectCount)
        {

        }

        private T p_DeleteItem;
        /// <summary>
        /// 要删除的数据(仅需设置主键值即可)
        /// </summary>
        public T DeleteItem
        {
            get { return p_DeleteItem; }
            set { p_DeleteItem = value; OnDeleteItemChanged(); }
        }

        private void OnDeleteItemChanged()
        {
            if (p_DeleteItem == null)
            {
                Express = null;
                Paramet = null;
            }
            else
            {
                IEnumerable<IDataParamet> p;
                Express = MsSqlServerHelper.GetDeleteSql<T>(p_DeleteItem, out p);
                Paramet = p;
            }
        }

        public override object Clone()
        {
            DeleteMsSqlExpress<T> a = new DeleteMsSqlExpress<T>();
            T d = null;
            if (p_DeleteItem != null)
            {
                if (p_DeleteItem is ICloneable)
                    d = ((ICloneable)p_DeleteItem).Clone() as T;
                else
                    d = p_DeleteItem;
            }
            a.DeleteItem = d;
            return a;
        }
    }
}
