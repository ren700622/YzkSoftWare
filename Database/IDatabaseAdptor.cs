using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;

namespace YzkSoftWare.Database
{

    /// <summary>
    /// 数据库连接适配器接口
    /// </summary>
    public interface IDatabaseAdptor : IDisposable
    {

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 开始事务:指定隔离级别
        /// </summary>
        /// <param name="level"></param>
        void BeginTransaction(IsolationLevel level);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();

        /// <summary>
        /// 提交到数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回IDataExpress.Computational返回的对象</returns>
        object CommitToDatabase(ISqlExpress sql);

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        string DbTypeName { get; }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="data"></param>
        void BulkCopyData<D>(IEnumerable<D> data);
    }

    /// <summary>
    /// 数据库连接适配器接口基础类
    /// </summary>
    public abstract class DatabaseAdptorBase : IDatabaseAdptor
    {
        protected DatabaseAdptorBase(string connectstring)
        {
            p_ConnectString = connectstring;
        }

        private string p_ConnectString;

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connectstring">连接字符串</param>
        /// <returns></returns>
        protected abstract IDbConnection CreateDbConnection(string connectstring);

        private IDbTransaction p_Transe = null;

        private IDbConnection p_Connect = null;

        protected IDbConnection Connect
        {
            get
            {
                if (p_Connect == null)
                    p_Connect = CreateDbConnection(p_ConnectString);
                return p_Connect;
            }
        }

        private IDbCommand p_Command = null;

        protected IDbCommand Command
        {
            get
            {
                if (p_Command == null)
                {
                    IDbConnection cnnt = Connect;
                    if (cnnt.State == ConnectionState.Closed)
                        cnnt.Open();
                    p_Command = Connect.CreateCommand();
                }
                return p_Command;
            }
        }

        private void CloseDbConnect()
        {
            try
            {
                if (p_Connect != null)
                {
                    p_Connect.Close();
                }
            }
            catch { }
            try
            {
                if (p_Connect != null)
                {
                    p_Connect.Dispose();
                }
            }
            catch { }
            p_Connect = null;
            p_Command = null;
        }

        void IDatabaseAdptor.BeginTransaction()
        {
            BeginTransaction();
        }

        void IDatabaseAdptor.CommitTran()
        {
            CommitTran();
        }

        void IDatabaseAdptor.RollbackTran()
        {
            RollbackTran();
        }

        object IDatabaseAdptor.CommitToDatabase(ISqlExpress sql)
        {
            return CommitToDatabase(sql);
        }

        string IDatabaseAdptor.DbTypeName
        {
            get { return DbTypeName; }
        }

        void IDatabaseAdptor.BulkCopyData<D>(IEnumerable<D> data)
        {
            BulkCopyData(data);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public virtual void BeginTransaction()
        {
            IDbCommand cmd = Command;
            if (p_Transe == null)
            {
                p_Transe = Connect.BeginTransaction();
                if (cmd.Transaction == null)
                    cmd.Transaction = p_Transe;
            }
        }

        void IDatabaseAdptor.BeginTransaction(IsolationLevel level)
        {
            IDbCommand cmd = Command;
            if (p_Transe == null)
            {
                p_Transe = Connect.BeginTransaction(level);
                if (cmd.Transaction == null)
                    cmd.Transaction = p_Transe;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void CommitTran()
        {
            if (p_Transe != null)
            {
                p_Transe.Commit();
                p_Transe.Dispose();
                p_Transe = null;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void RollbackTran()
        {
            if (p_Transe != null)
            {
                p_Transe.Rollback();
                p_Transe.Dispose();
                p_Transe = null;
            }
        }

        /// <summary>
        /// 将sql提交到数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回IDataExpress.Computational返回的对象</returns>
        public virtual object CommitToDatabase(ISqlExpress sql)
        {
            sql.CurrentDbType = DbTypeName;
            sql.Command = Command;
            return sql.Computational();
        }

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public abstract string DbTypeName { get; }

        public abstract void BulkCopyData<D>(IEnumerable<D> data);

        public virtual void Dispose()
        {
            CloseDbConnect();
        }


       
    }

    /// <summary>
    /// MS SQL SERVER的数据库连接器
    /// </summary>
    internal sealed class MsSqlDatabaseAdptor : DatabaseAdptorBase
    {

        internal MsSqlDatabaseAdptor(string connectstring)
            : base(connectstring)
        {

        }

        protected override IDbConnection CreateDbConnection(string connectstring)
        {
            return new System.Data.SqlClient.SqlConnection(connectstring);
        }

        public override string DbTypeName
        {
            get { return DatabaseTypeNames.MS_Sql; }
        }

        public override void BulkCopyData<D>(IEnumerable<D> data)
        {
            YzkSoftWare.DataModel.IDataModel model = typeof(D).GetDataModel();
            using (DataTable dt = CreateDataTableSchema(model))
            {
                int columcount = dt.Columns.Count;
                foreach (var d in data)
                {
                    DataRow nr = dt.NewRow();
                    for (int i = 0; i < columcount; i++)
                    {
                        var col = dt.Columns[i];
                        object fieldvalue = model.Fields[col.ColumnName].GetModelFieldValue(d);
                        nr[col.ColumnName] = fieldvalue;
                    }
                    dt.Rows.Add(nr);
                }

                System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy(Connect as System.Data.SqlClient.SqlConnection);
                try
                {
                    bulkCopy.DestinationTableName = model.Name;
                    bulkCopy.BatchSize = dt.Rows.Count;
                    if (Connect.State == ConnectionState.Closed)
                        Connect.Open();
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                }
                finally
                {
                    bulkCopy.Close();
                }
            }
        }

        private System.Data.DataTable CreateDataTableSchema(YzkSoftWare.DataModel.IDataModel moudle)
        {
            System.Data.DataTable dt = new System.Data.DataTable(moudle.Name);
            foreach (var field in moudle.Fields)
            {
                System.Data.DataColumn dc = new System.Data.DataColumn(field.Name, field.GetClrType());
                dt.Columns.Add(dc);
            }
            return dt;
        }
    }
}
