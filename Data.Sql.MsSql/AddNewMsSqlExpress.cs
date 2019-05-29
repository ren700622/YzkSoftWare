using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data.Sql.MsSql
{
    /// <summary>
    /// 新增数据MS-SQL的Sql表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AddNewMsSqlExpress<T> : MsSqlExpressBase
        where T : class
    {

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public AddNewMsSqlExpress()
            : base(null, DataModel.DbTypeForDataModel.Sql, SqlExpressResult.EffectCount)
        {

        }

        private T p_NewItem;
        /// <summary>
        /// 要新增的数据
        /// </summary>
        public T NewItem
        {
            get { return p_NewItem; }
            set
            {
                p_NewItem = value;
                OnNewItemChanged();
            }
        }

        private AddNewSqlExpressOutputParamet p_Par;

        /// <summary>
        /// 新增数据发生变化
        /// <para>重新生成Sql表达式</para>
        /// </summary>
        private void OnNewItemChanged()
        {
            if (p_NewItem == null)
            {
                Express = null;
                Paramet = null;
            }
            else
            {
                Express = MsSqlServerHelper.GetAddNewSql<T>(p_NewItem, out p_Par);
                if (p_Par != null)
                    Paramet = p_Par.Paramets;
                else
                    Paramet = null;
            }
            
        }

        protected override object OnSuccessCommandForEffectCount(int effect)
        {
            IEnumerable<IDataParamet> p = Paramet;
            if (p!=null && p_NewItem != null && p_Par != null && p_Par.OutputGenerateFieldNames != null)
            {
                IDataModel model = typeof(T).GetDataModel();
                foreach (string fn in p_Par.OutputGenerateFieldNames)
                {
                    object v = MsSqlServerHelper.GetParametValue(fn, p_Par.NameHz, p);
                    var field = model.Fields[fn];
                    v = MsSqlServerHelper.ConvertFieldValueFromDbValue(field, v);
                    field.SetModelFieldValue(p_NewItem, v);
                }
            }
            return base.OnSuccessCommandForEffectCount(effect);
        }

        public override object Clone()
        {
            AddNewMsSqlExpress<T> a = new AddNewMsSqlExpress<T>();
            T n = null;
            if (p_NewItem != null)
            {
                if (p_NewItem is ICloneable)
                {
                    n = ((ICloneable)p_NewItem).Clone() as T;
                }
                else
                {
                    n = p_NewItem;
                }
            }
            a.NewItem = n;
            return a;
        }
    }
}
