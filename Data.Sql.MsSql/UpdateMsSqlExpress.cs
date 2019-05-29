using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data.Sql.MsSql
{
    /// <summary>
    /// 修改数据MS-SQL的Sql表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class UpdateMsSqlExpress<T> : MsSqlExpressBase
        where T : class
    {

        public UpdateMsSqlExpress()
            : base(null, DataModel.DbTypeForDataModel.Sql, SqlExpressResult.EffectCount)
        {

        }

        /// <summary>
        /// 要修改的数据
        /// </summary>
        public T UpdateItem { get; private set; }

        /// <summary>
        /// 要修改的数据的字段范围
        /// </summary>
        public FieldSelectRange UpdateFieldRange { get; private set; }

        /// <summary>
        /// UpdateFieldRange指定的字段范围的字段名称
        /// </summary>
        public IEnumerable<string> UpdateFieldNames { get; private set; }

        /// <summary>
        /// 设置要修改的数据
        /// </summary>
        /// <param name="updateItem">要修改的数据</param>
        /// <param name="range">修改值的字段范围</param>
        /// <param name="upfieldnames">range指定的字段范围的字段名称</param>
        public void SetUpdateItem(T updateItem, FieldSelectRange range, IEnumerable<string> upfieldnames)
        {
            UpdateItem = updateItem;
            UpdateFieldRange = range;
            UpdateFieldNames = upfieldnames;
            OnUpdateItemChanged();
        }

        /// <summary>
        /// 新增数据发生变化
        /// <para>重新生成Sql表达式</para>
        /// </summary>
        private void OnUpdateItemChanged()
        {
            if (UpdateItem == null)
            {
                Express = null;
                Paramet = null;
            }
            else
            {
                IEnumerable<IDataParamet> p;
                Express = MsSqlServerHelper.GetUpdateSql<T>(UpdateItem, UpdateFieldRange,UpdateFieldNames,out p);
                Paramet = p;
            }

        }

        public override object Clone()
        {
            UpdateMsSqlExpress<T> a = new UpdateMsSqlExpress<T>();
            T item = null;
            if (UpdateItem != null)
            {
                if (UpdateItem is ICloneable)
                    item = ((ICloneable)UpdateItem).Clone() as T;
                else
                    item = UpdateItem;
            }
            a.SetUpdateItem(item, UpdateFieldRange, UpdateFieldNames);
            return a;
        }
    }
}
