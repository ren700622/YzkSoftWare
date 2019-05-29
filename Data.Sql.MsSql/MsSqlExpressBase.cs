using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Database;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data.Sql.MsSql
{
    public abstract class MsSqlExpressBase : SqlExpressBase
    {
        /// <summary>
        /// 使用表达式、表达式参数、表达式类型、表达式返回类型的构造
        /// </summary>
        /// <param name="express">表达式</param>
        /// <param name="paramet">表达式参数</param>
        /// <param name="sqlexpType">表达式类型</param>
        /// <param name="resultType">表达式返回类型</param>
        protected MsSqlExpressBase(string express, IEnumerable<IDataParamet> paramet, DbTypeForDataModel sqlexpType, SqlExpressResult resultType)
            : base(express, paramet)
        {
            p_SqlExpressType = sqlexpType;
            p_ResultType = resultType;
        }

        /// <summary>
        /// 使用表达式、表达式类型、表达式返回类型的构造
        /// </summary>
        /// <param name="express">表达式</param>
        /// <param name="sqlexpType">表达式类型</param>
        /// <param name="resultType">表达式返回类型</param>
        protected MsSqlExpressBase(string express, DbTypeForDataModel sqlexpType, SqlExpressResult resultType)
            : base(express, null)
        {
            p_SqlExpressType = sqlexpType;
            p_ResultType = resultType;
        }

        /// <summary>
        /// 是否支持指定dbtype的数据库
        /// <para>本类型支持:"MS_SQL"</para>
        /// </summary>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        protected override bool CanbeSupportDb(string dbtype)
        {
            return string.Equals(dbtype, DatabaseTypeNames.MS_Sql, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 生成参数名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string CreateParametName(string name)
        {
            return MsSqlServerHelper.GetParametName(name);
        }

        private DbTypeForDataModel p_SqlExpressType;
        /// <summary>
        /// 表达式类型
        /// </summary>
        protected override DbTypeForDataModel SqlExpressType
        {
            get { return p_SqlExpressType; }
        }

        private SqlExpressResult p_ResultType;
        /// <summary>
        /// SQl的Command返回类型
        /// </summary>
        protected override SqlExpressResult ResultType
        {
            get
            {
                return p_ResultType;
            }
        }

        protected override void SetDbDataParameter(System.Data.IDbDataParameter dbp, IDataParamet p)
        {
            base.SetDbDataParameter(dbp, p);
            if (dbp is SqlParameter && dbp.DbType == System.Data.DbType.Object)
            {
                ((SqlParameter)dbp).SqlDbType = System.Data.SqlDbType.NText;
            }
        }



    }
}
