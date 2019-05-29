using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data.Sql.MsSql
{
    public class MsQuerySqlExpress : MsSqlExpressBase
    {

        public MsQuerySqlExpress() : this(SqlExpressResult.RowData) { }

        protected MsQuerySqlExpress(SqlExpressResult result)
            : base(null, null, DataModel.DbTypeForDataModel.Sql, result) { }

        protected override string Express
        {
            get
            {
                return GetDataExpress();
            }
            set
            {
                base.Express = value;
            }
        }

        private string GetSelectFieldExpress(string n)
        {
            if (MsSqlServerHelper.IsFeildNameInSqlExpress(n))
                return n;
            if (MsSqlServerHelper.IsValidFieldName(n))
            {
                return MsSqlServerHelper.GetFeildNameInSqlExpress(n);
            }
            return n;
        }

        private string GetOrderPartFieldExpress(IOrder od)
        {
            string fn = GetSelectFieldExpress(od.OrderMemberName);
            if (!string.IsNullOrEmpty(fn))
            {
                switch (od.FieldOrder)
                {
                    case FieldOrder.ASC: return string.Format("{0} ASC", fn);
                    case FieldOrder.DESC: return string.Format("{0} DESC", fn);
                    default: return fn;
                }
            }
            return null;
        }

        private string GetSelectPart(IEnumerable<string> selectfields, int? top)
        {
            string s = null;
            if (selectfields != null && selectfields.Count() > 0)
                s = selectfields.Aggregate((f1, f2) => string.IsNullOrEmpty(f1) ? GetSelectFieldExpress(f2) : string.Format("{0},{1}", f1, GetSelectFieldExpress(f2)));
            if (string.IsNullOrEmpty(s))
                s = "*";
            if (top != null && top.HasValue && top.Value > 0)
                s = string.Format("TOP {1} {0}", s, top.Value);
            return s;
        }

        private string GetOrderPart(IEnumerable<IOrder> od)
        {
            if (od != null && od.Count() > 0)
            {
                return od.Aggregate<IOrder, string>(null, (f1, f2) => string.IsNullOrEmpty(f1) ? GetOrderPartFieldExpress(f2) : string.Format("{0},{1}", f1, GetOrderPartFieldExpress(f2)));
            }
            return null;
        }

        private string GetGroupPart(IEnumerable<string> groupfields)
        {
            if (groupfields != null && groupfields.Count() > 0)
                return groupfields.Aggregate((f1, f2) => string.IsNullOrEmpty(f1) ? GetSelectFieldExpress(f2) : string.Format("{0},{1}", f1, GetSelectFieldExpress(f2)));
            return null;
        }

        public string GetFromPart(string from)
        {
            return string.IsNullOrEmpty(from) ? null : GetSelectFieldExpress(from);
        }

        /// <summary>
        /// SELECT语句的Top选项
        /// </summary>
        public virtual int? Top { get; set; }

        /// <summary>
        /// 选择字段
        /// </summary>
        public virtual IEnumerable<string> Select { get; set; }

        /// <summary>
        /// 分组字段
        /// </summary>
        public virtual IEnumerable<string> GroupBy { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual IEnumerable<IOrder> OrderBy { get; set; }

        /// <summary>
        /// 条件语句
        /// </summary>
        public virtual string Filter { get; set; }

        /// <summary>
        /// SQL语句的From部分
        /// </summary>
        public virtual string From { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public virtual IEnumerable<IDataParamet> QueryParamets
        {
            get { return base.Paramet; }
            set { base.Paramet = value; }
        }

        /// <summary>
        /// 查询语句返回的记录对应构造的数据类型
        /// </summary>
        public virtual Type ResultDataModelType { get; set; }

        public override object Clone()
        {
            MsQuerySqlExpress exp = this.GetType().CreateObject() as MsQuerySqlExpress;
            exp.Select = Select;
            exp.GroupBy = GroupBy;
            exp.OrderBy = OrderBy;
            exp.Filter = Filter;
            exp.From = From;
            exp.Top = Top;
            exp.QueryParamets = QueryParamets;
            exp.ResultDataModelType = ResultDataModelType;
            return exp;
        }


        /// <summary>
        /// 获取当前表示的SQL查询语句
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDataExpress()
        {
            string select = string.Format("SELECT {0}", GetSelectPart(Select, Top));
            string where = string.IsNullOrEmpty(Filter) ? null : string.Format("WHERE {0}", Filter);
            string order = GetOrderPart(OrderBy);
            if (!string.IsNullOrEmpty(order))
                order = string.Format("ORDER BY {0}", order);
            string group = GetGroupPart(GroupBy);
            if (!string.IsNullOrEmpty(group))
                group = string.Format("GROUP BY {0}", group);
            string from = GetFromPart(From);
            if (!string.IsNullOrEmpty(from))
                from = string.Format("FROM {0}", from);
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(select))
            {
                return string.Format("{0} {1} {2} {3} {4}", select, from, where, group, order);
            }
            return null;
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        public object QueryResult { get; protected set; }

       
        protected override object OnSuccessCommandForDataReader(IDataReader reader)
        {
            System.Collections.IList list = null;
            Type resultType = ResultDataModelType;
            if (reader != null && !reader.IsClosed && resultType != null)
            {
                var model = resultType.GetDataModel();
                if (model != null && model.DbType == DataModel.DbTypeForDataModel.Unkown)
                    model = null;
                list = typeof(List<>).GenerateGenericDefaultObject(new Type[] { resultType }) as System.Collections.IList;
                int fieldcount = reader.FieldCount;
                string[] keys = new string[fieldcount];
                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i] = reader.GetName(i);
                }
                while (reader.Read())
                {
                    object dataItem = resultType.CreateObject();
                    for (int i = 0; i < fieldcount; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            string name = keys[i];
                            object value = ConvertFieldValueFromDbValue(model, name, reader.GetValue(i));
                            dataItem.SetMemberValue(name, value);
                        }
                    }
                    list.Add(dataItem);
                }
            }
            return list;
        }

        /// <summary>
        /// 将字段值从数据库格式转换为本机格式
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        protected object ConvertFieldValueFromDbValue(IDataModel model, string name, object value)
        {
            return MsSqlServerHelper.ConvertFieldValueFromDbValue(model.Fields[name], value);
        }
    }
}
