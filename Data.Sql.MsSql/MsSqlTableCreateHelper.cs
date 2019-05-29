using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data.Sql.MsSql
{
    public static class MsSqlTableCreateHelper
    {

        /// <summary>
        /// 缺省ChineseCollateName
        /// </summary>
        public const string ChineseCollateNames_Default = "Chinese_PRC_CI_AS";
        /// <summary>
        /// 香港ChineseCollateName
        /// </summary>
        public const string ChineseCollateNames_XG = "Chinese_Hong_Kong_Stroke_90_CI_AS";
        /// <summary>
        /// PRC_90_CI的ChineseCollateName
        /// </summary>
        public const string ChineseCollateNames_HM = "Chinese_PRC_90_CI_AS";
        /// <summary>
        /// 台弯ChineseCollateName
        /// </summary>
        public const string ChineseCollateNames_TW = "Chinese_Taiwan_Stroke_CI_AS";

        private static System.Data.SqlDbType DbTypeToSqlDbType(System.Data.DbType pSourceType)
        {
            if (pSourceType == System.Data.DbType.Object) return System.Data.SqlDbType.Text;
            System.Data.SqlClient.SqlParameter paraConver = new System.Data.SqlClient.SqlParameter();
            paraConver.DbType = pSourceType;
            return paraConver.SqlDbType;
        }

        /// <summary>
        /// 创建字段定义的SQL
        /// </summary>
        /// <param name="field">字段模型</param>
        /// <param name="tablesourcename">表名称</param>
        /// <param name="stringcollatename">字符排序</param>
        /// <returns></returns>
        private static string CreateTableFieldSql(
            IDataFieldModel field, 
            string tablesourcename, 
            string stringcollatename)
        {
            StringBuilder xx = new StringBuilder();
            //添加 "[fieldname]"
            xx.Append(MsSqlServerHelper.GetFeildNameInSqlExpress(field.Name));
            if (field.IsRowVersion())
            {
                xx.Append(" [timestamp] NULL");
            }
            else
            {
                //添加 " [dbtype]" -> "[fieldname] [dbtype]"
                xx.AppendFormat(" [{0}]", DbTypeToSqlDbType(field.FieldDbType).ToString().ToLower());
                switch (field.FieldDbType)
                {
                    case System.Data.DbType.AnsiString:
                    case System.Data.DbType.String:
                        {
                            int size = field.GetFieldSize();
                            if (size <= 0)
                                size = 50;
                            if (size == int.MaxValue)
                                xx.AppendFormat("(max) COLLATE {0}", stringcollatename);
                            else
                                xx.AppendFormat("({0}) COLLATE {1}", size, stringcollatename);
                            //添加 "(size) COLLATE ....." -> "[fieldname] [dbtype](size) COLLATE...."
                            break;
                        }
                    case System.Data.DbType.Decimal:
                        {
                            int l = 18;
                            int dot = 4;
                            var d = field.GetDecimalDefine();
                            if (d != null)
                            {
                                l = d.DecimalLength;
                                dot = d.DotLength;
                                if (l <= 0)
                                    l = 18;
                                if (dot < 0)
                                    dot = 0;
                            }
                            //添加 "(length,dotlength)" -> "[fieldname] [dbtype](length,dotlength)"
                            xx.AppendFormat("({0},{1})", l, dot);
                            break;
                        }
                    case System.Data.DbType.Int16:
                    case System.Data.DbType.Int32:
                    case System.Data.DbType.Int64:
                        {
                            var g = field.GetDbGeneratedDefine();
                            if (g != null)
                            {
                                //添加 " IDENTITY(start,step)" -> "[fieldname] [dbtype]IDENTITY(start,step)"
                                xx.AppendFormat(" IDENTITY({0},{1})", g.StartValue, g.Step);
                            }
                            break;
                        }
                }
                if (field.CanbeNull())
                    xx.Append(" NULL");
                else
                    xx.Append(" NOT NULL");

                string defaultvalue = field.GetDefaultValue();
                if (!string.IsNullOrEmpty(defaultvalue))
                {
                    xx.AppendFormat(" CONSTRAINT [DF_{0}_{1}_defaultvalue] DEFAULT ({2})", tablesourcename, field.Name, defaultvalue);
                }
            }
            return xx.ToString();
        }

        /// <summary>
        /// 创建表的字段集合定义SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="stringcollatename">字符排序</param>
        /// <returns></returns>
        private static string CreateTableFieldCollectionSql(IDataModel table, string stringcollatename)
        {
            var fields = table.Fields;
            StringBuilder str = new StringBuilder();
            foreach (var field in fields)
            {
                string sql = CreateTableFieldSql(field, table.Name, stringcollatename);
                if (str.Length == 0)
                    str.Append(sql);
                else
                    str.AppendFormat(",\r\n{0}", sql);
            }
            return str.ToString();
        }

        /// <summary>
        /// 创建表的主键定义SQL
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string CreatePrimKeySql(IDataModel table)
        {
            string kn = null;
            var keys = table.PrimaryKeys;
            if (keys != null)
            {
                foreach (var k in keys)
                {
                    string a = string.Format("[{0}] {1}", k.Name, k.Order.ToString().ToUpper());
                    if (string.IsNullOrEmpty(kn))
                        kn = a;
                    else
                        kn = string.Format("{0},\r\n{1}", kn, a);
                }
            }
            if (!string.IsNullOrEmpty(kn))
            {
                return string.Format("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED \r\n ({1}\r\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]", table.Name, kn);
            }
            return null;
        }

        /// <summary>
        /// 创建表定义的SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="stringcollatename">字符排序</param>
        /// <returns></returns>
        private static string CreateTableSql(IDataModel table,string stringcollatename)
        {
            string sql_define_fields = CreateTableFieldCollectionSql(table, stringcollatename);
            string sql_define_primkeys = CreatePrimKeySql(table);
            if (!string.IsNullOrEmpty(sql_define_primkeys))
                return string.Format("CREATE TABLE [dbo].[{0}](\r\n{1},\r\n{2}\r\n)ON [PRIMARY]", table.Name, sql_define_fields, sql_define_primkeys);
            else
                return string.Format("CREATE TABLE [dbo].[{0}](\r\n{1}\r\n)ON [PRIMARY]", table.Name, sql_define_fields);
        }

        /// <summary>
        /// 创建表的索引定义SQL
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string CreateTableIndexSql(IDataModel table)
        {
            var indexes = table.IndexKeys;
            if (indexes != null)
            {
                StringBuilder xx = new StringBuilder();
                foreach (var index in indexes)
                {
                    string x = string.Format("CREATE NONCLUSTERED INDEX [IX_{0}_{1}] ON [dbo].[{0}]\r\n([{1}] {2}\r\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]", table.Name, index.Name, index.Order.ToString().ToUpper());
                    xx.AppendLine(x);
                }
                if (xx.Length > 0)
                    return xx.ToString();
            }
            return null;
        }

        /// <summary>
        /// 创建表定义的SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="stringcollatename">字符排序</param>
        /// <returns></returns>
        internal static string CreateDefineTableSql(IDataModel table, string stringcollatename)
        {
            StringBuilder xx = new StringBuilder();
            xx.AppendLine("SET ANSI_NULLS ON");
            xx.AppendLine("SET QUOTED_IDENTIFIER ON");
            xx.AppendLine(CreateTableSql(table, stringcollatename));
            string index = CreateTableIndexSql(table);
            if(!string.IsNullOrEmpty(index))
            {
                xx.AppendLine(index);
            }
            return xx.ToString();
        }

        /// <summary>
        /// 使用Chinese_PRC_CI_AS排序规则来创建表定义的SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <returns></returns>
        internal static string CreateDefineTableSql(IDataModel table)
        {
            return CreateDefineTableSql(table, ChineseCollateNames_Default);
        }
    }
}
