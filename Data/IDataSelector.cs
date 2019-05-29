using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Database;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 数据获取接口
    /// </summary>
    public interface IDataSelector : IDisposable
    {

        /// <summary>
        /// 数据库类型名称
        /// </summary>
        string DbTypeName { get; }

        /// <summary>
        /// 单行单列的数据选择
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        V ExecuteScalar<V>(ISqlExpress sql) where V : struct;

        /// <summary>
        /// 单行单列的数据选择
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string ExecuteScalarForString(ISqlExpress sql);

        /// <summary>
        /// 数据集选择
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        IEnumerable<T> ExecuteReader<T>(ISqlExpress sql) where T : class,new();

        /// <summary>
        /// 数据集选择
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        System.Collections.IEnumerable ExecuteReaderFor(ISqlExpress sql);

        /// <summary>
        /// 第一行数据选择
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T ExecuteReaderOfFirst<T>(ISqlExpress sql) where T : class,new();

        /// <summary>
        /// 第一行数据选择
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        object ExecuteReaderOfFirstFor(ISqlExpress sql);
    }

    public sealed class DataSelector : IDataSelector
    {

        public DataSelector(IDatabaseAdptor db)
        {
            p_Db = db;
        }

        private IDatabaseAdptor p_Db;

        public string DbTypeName
        {
            get
            {
                if (p_Db != null)
                    return p_Db.DbTypeName;
                return null;
            }
        }

        public V ExecuteScalar<V>(ISqlExpress sql) where V : struct
        {
            return (V)p_Db.CommitToDatabase(sql);
        }

        public string ExecuteScalarForString(ISqlExpress sql)
        {
            return (string)p_Db.CommitToDatabase(sql);
        }

        public IEnumerable<T> ExecuteReader<T>(ISqlExpress sql) where T : class, new()
        {
            return p_Db.CommitToDatabase(sql) as IEnumerable<T>;
        }

        public System.Collections.IEnumerable ExecuteReaderFor(ISqlExpress sql)
        {
            return p_Db.CommitToDatabase(sql) as System.Collections.IEnumerable;
        }

        public T ExecuteReaderOfFirst<T>(ISqlExpress sql) where T : class, new()
        {
            IEnumerable<T> vs = ExecuteReader<T>(sql);
            if (vs != null)
                return vs.FirstOrDefault();
            return null;
        }

        public object ExecuteReaderOfFirstFor(ISqlExpress sql)
        {
            System.Collections.IEnumerable l = ExecuteReaderFor(sql);
            if (l != null)
            {
                var g = l.GetEnumerator();
                if (g!=null && g.MoveNext())
                    return g.Current;
            }
            return null;
        }

        public void Dispose()
        {
            if (p_Db != null)
                p_Db.Dispose();
        }
    }
}
