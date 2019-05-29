using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 数据表达式
    /// </summary>
    public interface IDataExpress : ICloneable
    {

        /// <summary>
        /// 表达式
        /// </summary>
        string Express { get; }

        /// <summary>
        /// 表达式参数
        /// </summary>
        IEnumerable<IDataParamet> Paramet { get; set; }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <returns>返回表达式计算结果</returns>
        object Computational();
    }

    /// <summary>
    /// 数据表达式解析接口
    /// </summary>
    public interface IDataExpressParase
    {

        /// <summary>
        /// 解析数据表达式
        /// </summary>
        /// <param name="express"></param>
        void ParaseExpress(string express);

    }

    /// <summary>
    /// 参数接口
    /// </summary>
    public interface IDataParamet
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 参数值类型
        /// </summary>
        Type ValueType { get; }
        /// <summary>
        /// 参数值
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// 是否是输出参数
        /// </summary>
        ParameterDirection Direction { get; }
        /// <summary>
        /// 值的数据长度
        /// </summary>
        int ValueSize { get; }
    }

    public class DataParamet : IDataParamet
    {

        public DataParamet()
        {
            Direction = ParameterDirection.Input;
        }

        public string Name { get; set; }

        public Type ValueType { get; set; }

        public object Value { get; set; }

        public ParameterDirection Direction { get; set; }

        public int ValueSize { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// 数据表达式基础类
    /// </summary>
    public abstract class DataExpressBase : IDataExpress
    {

        protected DataExpressBase(string express, IEnumerable<IDataParamet> paramet)
        {
            Express = express;
            Paramet = paramet;
        }

        /// <summary>
        /// 获取或设置表达式
        /// </summary>
        protected virtual string Express { get; set; }

        /// <summary>
        /// 获取表达式参数
        /// </summary>
        protected IEnumerable<IDataParamet> Paramet { get; set; }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <returns></returns>
        protected abstract object Computational();

        string IDataExpress.Express
        {
            get { return Express; }
        }

        IEnumerable<IDataParamet> IDataExpress.Paramet
        {
            get { return Paramet; }
            set { Paramet = value; }
        }

        object IDataExpress.Computational()
        {
            return Computational();
        }

        public abstract object Clone();
    }
}
