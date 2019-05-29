using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Database;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// SQL表达式接口
    /// </summary>
    public interface ISqlExpress : IDataExpress
    {

        /// <summary>
        /// 命令
        /// </summary>
        IDbCommand Command { get; set; }

        /// <summary>
        /// 命令超时值,以秒为单位
        /// </summary>
        int? CommandTimeOut { get; set; }

        /// <summary>
        /// 当前使用的数据库类型
        /// </summary>
        string CurrentDbType { get; set; }
    }


    /// <summary>
    /// SQL表达式基础类
    /// </summary>
    public abstract class SqlExpressBase : DataExpressBase, ISqlExpress
    {
        protected SqlExpressBase(string express, IEnumerable<IDataParamet> paramet)
            : base(express, paramet)
        {

        }

        protected SqlExpressBase(string express)
            : this(express, null)
        {

        }


        /// <summary>
        /// 是否支持指定dbtype的数据库
        /// </summary>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        protected abstract bool CanbeSupportDb(string dbtype);

        /// <summary>
        /// 表达式类型
        /// </summary>
        protected abstract DbTypeForDataModel SqlExpressType { get; }

        /// <summary>
        /// 生成参数名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected abstract string CreateParametName(string name);

        /// <summary>
        /// SQl的Command返回类型
        /// </summary>
        protected abstract SqlExpressResult ResultType { get; }

        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected virtual IDbDataParameter CreateParamet(IDbCommand cmd)
        {
            return cmd.CreateParameter();
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="dbp"></param>
        /// <param name="p"></param>
        protected virtual void SetDbDataParameter(IDbDataParameter dbp, IDataParamet p)
        {
            dbp.Direction = p.Direction;
            dbp.DbType = p.ValueType.GetDbType();
            switch (dbp.DbType)
            {
                case DbType.String:
                case DbType.Object:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    {
                        if (p.ValueSize > 0)
                            dbp.Size = p.ValueSize;
                        break;
                    }
            }
            if (p.Value != null)
                dbp.Value = p.Value;
            else
                dbp.Value = DBNull.Value;
            dbp.ParameterName = CreateParametName(p.Name);
        }

        /// <summary>
        /// 指示pn1和pn2参数名称是否相同
        /// </summary>
        /// <param name="pn1"></param>
        /// <param name="pn2"></param>
        /// <returns></returns>
        protected virtual bool IsSameParametName(string pn1, string pn2)
        {
            return string.Equals(CreateParametName(pn1), CreateParametName(pn2), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 当成功执行IDbCommand.ExecuteNonQuery后调用
        /// </summary>
        /// <param name="effect">IDbCommand.ExecuteNonQuery返回的影响记录数</param>
        /// <returns>基类没有任何实现直接返回effect</returns>
        protected virtual object OnSuccessCommandForEffectCount(int effect)
        {
            return effect;
        }

        /// <summary>
        /// 当成功执行IDbCommand.ExecuteScalar后调用
        /// </summary>
        /// <param name="scalarobj">IDbCommand.ExecuteScalar返回的object</param>
        /// <returns>基类没有任何实现直接返回scalarobj</returns>
        protected virtual object OnSuccessCommandForObject(object scalarobj)
        {
            return scalarobj;
        }

        /// <summary>
        /// 当成功执行IDbCommand.ExecuteReader后调用
        /// </summary>
        /// <param name="reader">IDbCommand.ExecuteReader返回的记录读取器</param>
        /// <returns>基类没有任何实现直接返回null</returns>
        protected virtual object OnSuccessCommandForDataReader(IDataReader reader)
        {
            return null;
        }

        private void SetReturnParamets(IEnumerable<IDataParamet> ps, IDataParameterCollection paramets)
        {
            if (ps != null)
            {
                foreach (var p in ps)
                {
                    if (p.Direction == ParameterDirection.InputOutput
                        || p.Direction == ParameterDirection.Output
                        || p.Direction == ParameterDirection.ReturnValue)
                    {
                        foreach (var xp in paramets)
                        {
                            IDbDataParameter dbp = xp as IDbDataParameter;
                            if (dbp != null && IsSameParametName(dbp.ParameterName, p.Name))
                            {
                                p.Value = dbp.Value;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ResetSetCommand(IDbCommand cmd)
        {
            cmd.CommandText = null;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 30;
        }

        private void CloseReader(IDataReader reader)
        {
            if (reader != null)
            {
                try
                {
                    reader.Close();
                }
                catch { }
                try
                {
                    reader.Dispose();
                }
                catch { }
            }
        }

        private bool PreparForCommand(IDbCommand cmd)
        {
            string sql = Express;
            if (!string.IsNullOrEmpty(sql))
            {
                cmd.CommandText = sql;
                int c = CommandTimeOut != null && CommandTimeOut.HasValue ? CommandTimeOut.Value : 30;
                if (c <= 0)
                    c = 30;
                cmd.CommandTimeout = c;
                switch (SqlExpressType)
                {
                    case DbTypeForDataModel.View:
                    case DbTypeForDataModel.Sql:
                        cmd.CommandType = CommandType.Text;break;
                    case DbTypeForDataModel.Store:
                        cmd.CommandType = CommandType.StoredProcedure;break;
                    case DbTypeForDataModel.Table:
                        cmd.CommandType = CommandType.TableDirect;break;
                }
                IEnumerable<IDataParamet> paramet = Paramet;
                if (paramet != null)
                {
                    foreach (var p in paramet)
                    {
                        IDbDataParameter dbp = CreateParamet(cmd);
                        SetDbDataParameter(dbp, p);
                        cmd.Parameters.Add(dbp);
                    }
                }
                return true;
            }
            return false;
        }

        private object ExcuteCommand(IDbCommand cmd)
        {
            switch (ResultType)
            {
                case SqlExpressResult.EffectCount:
                    {
                        int efcount = cmd.ExecuteNonQuery();
                        SetReturnParamets(Paramet, cmd.Parameters);
                        return OnSuccessCommandForEffectCount(efcount);
                    }
                case SqlExpressResult.SingleObject:
                    {
                        object so = cmd.ExecuteScalar();
                        SetReturnParamets(Paramet, cmd.Parameters);
                        return OnSuccessCommandForObject(so);
                    }
                case SqlExpressResult.RowData:
                    {
                        IDataReader reader = cmd.ExecuteReader();
                        SetReturnParamets(Paramet, cmd.Parameters);
                        try
                        {
                            return OnSuccessCommandForDataReader(reader);
                        }
                        finally
                        {
                            CloseReader(reader);
                        }
                    }
            }
            return null;
        }

        protected override object Computational()
        {
            if (!CanbeSupportDb(CurrentDbType))
                throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, CurrentDbType));
            if (p_Command == null)
                throw new ObjectIsNullException(LocalResource.CommandItem);
            ResetSetCommand(p_Command);
            if (PreparForCommand(p_Command))
                return ExcuteCommand(p_Command);
            return null;
        }

        private IDbCommand p_Command;
        /// <summary>
        /// 数据库命令
        /// </summary>
        public IDbCommand Command
        {
            get
            {
                return p_Command;
            }
            set
            {
                p_Command = value;
            }
        }

        /// <summary>
        /// 命令超时值,以秒为单位
        /// </summary>
        public int? CommandTimeOut { get; set; }

        /// <summary>
        /// 当前使用的数据库类型
        /// </summary>
        public string CurrentDbType { get; set; }
    }

    /// <summary>
    /// sql表达式返回类型
    /// </summary>
    [ResourceDisplayName(typeof(LocalResource), "SqlExpressResult")]
    public enum SqlExpressResult
    {
        /// <summary>
        /// 返回数据行
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "RowData")]
        RowData = 0,
        /// <summary>
        /// 返回单行单列的值
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "SingleObject")]
        SingleObject = 1,
        /// <summary>
        /// 返回影响到的更新数据的记录数
        /// </summary>
        [ResourceDisplayName(typeof(LocalResource), "EffectCount")]
        EffectCount = 2
    }


}
