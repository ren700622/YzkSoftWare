using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data.Sql.MsSql;
using YzkSoftWare.Database;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data.Sql
{
    public static class QuerySqlFactory
    {

        private static bool IsMsSqlDb(string dbtype)
        {
            return string.Equals(dbtype, DatabaseTypeNames.MS_Sql, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsNullableFieldForBooleanOrNum(IDataFieldModel f)
        {
            if (f == null)
                return false;
            return f.CanbeNull() && (f.FieldDbType == System.Data.DbType.Boolean || DataValueParase.IsNumberType(f.GetClrType()) != NumberType.Unkown);
        }

        /// <summary>
        /// 合并条件表达式为并且表达式
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="wheres">表达式数组</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="rtparamets">合并后的表达式参数数组</param>
        /// <returns>并且表达式</returns>
        public static string UnionWhereForAnd(
            string dbtypename,
            IEnumerable<string> wheres,
            IEnumerable<IDataParamet> paramets,
            out IEnumerable<IDataParamet> rtparamets)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.UnionWhereForAnd(wheres, paramets, out rtparamets);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 合并条件表达式为或者表达式
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="wheres">表达式数组</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="rtparamets">合并后的表达式参数数组</param>
        /// <returns>或者表达式</returns>
        public static string UnionWhereForOr(
           string dbtypename,
           IEnumerable<string> wheres,
           IEnumerable<IDataParamet> paramets,
           out IEnumerable<IDataParamet> rtparamets)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.UnionWhereForOr(wheres, paramets, out rtparamets);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }
       
        /// <summary>
        /// 创建等于条件表达式
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="fieldname">左边字段名称</param>
        /// <param name="parametname">右边参数名称</param>
        /// <param name="parametvalue">右边参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        public static string CreateEqWhere(
            string dbtypename, 
            string fieldname,
            string parametname,
            object parametvalue,
            out IDataParamet paramet)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.CreateEqWhere(fieldname, parametname, parametvalue, out paramet);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 创建等于条件表达式(左边和右边名称一样)
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="fieldname">字段名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        public static string CreateEqWhere(
            string dbtypename,
            string fieldname,
            object parametvalue,
            out IDataParamet paramet)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.CreateEqWhere(fieldname, parametvalue, out paramet);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 创建不等于条件表达式
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="fieldname">左边字段名称</param>
        /// <param name="parametname">右边参数名称</param>
        /// <param name="parametvalue">右边参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        public static string CreateNotEqWhere(
            string dbtypename,
            string fieldname,
            string parametname,
            object parametvalue,
            out IDataParamet paramet)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.CreateNotEqWhere(fieldname, parametname, parametvalue, out paramet);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 创建不等于条件表达式(左边和右边名称一样)
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="fieldname">字段名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        public static string CreateNotEqWhere(
            string dbtypename,
            string fieldname,
            object parametvalue,
            out IDataParamet paramet)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.CreateNotEqWhere(fieldname, parametvalue, out paramet);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 创建以parametvalue开始或在parametvalue中以列的值开始的条件表达式
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="fieldname">列字段名称</param>
        /// <param name="parametname">参数名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="parametIsExpress">是否是取在parametvalue中以列的值开始的条件表达式</param>
        /// <param name="paramet"></param>
        /// <returns></returns>
        public static string CreateFirstIndexOfWhere(
            string dbtypename,
            string fieldname,
            string parametname,
            string parametvalue,
            bool parametIsExpress,
            out IDataParamet paramet)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.CreateFirstIndexOfWhere(fieldname, parametname, parametvalue, parametIsExpress, out paramet);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 创建以parametvalue开始或在parametvalue中以列的值开始的条件表达式
        /// </summary>
        /// <param name="dbtypename">数据库类型</param>
        /// <param name="fieldname">列字段名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="parametIsExpress">是否是取在parametvalue中以列的值开始的条件表达式</param>
        /// <param name="paramet"></param>
        /// <returns></returns>
        public static string CreateFirstIndexOfWhere(
            string dbtypename,
            string fieldname,
            string parametvalue,
            bool parametIsExpress,
            out IDataParamet paramet)
        {
            if (IsMsSqlDb(dbtypename))
            {
                return MSSql_QuerySqlFactory.CreateFirstIndexOfWhere(fieldname, parametvalue, parametIsExpress, out paramet);
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtypename));
        }

        /// <summary>
        /// 创建表定义的SQL表达式
        /// </summary>
        /// <param name="dbtype"></param>
        /// <param name="tableType"></param>
        /// <returns></returns>
        public static ISqlExpress CreateTableDefineSql(string dbtype,Type tableType)
        {
            if (IsMsSqlDb(dbtype))
            {
                return typeof(DteMsSqlExpressForTable<>).GenerateGenericDefaultObject(tableType) as ISqlExpress;
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtype));
        }

        /// <summary>
        /// 获取通过指定记录Id获取对象的SQL查询表达式
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="from">查询语句的表或视图名称</param>
        /// <param name="resultType">使用查询语句返回记录来构造对象的对象类型</param>
        /// <param name="select">查询语句的字段,传null指示使用*</param>
        /// <param name="id">对象Id</param>
        /// <returns></returns>
        public static ISqlExpress CreateQuerySqlByIdValue(string dbtype, string from, Type resultType, IEnumerable<string> select, long id)
        {
            if (IsMsSqlDb(dbtype))
            {
                IDataParamet p;
                MsQuerySqlExpress sql = new MsQuerySqlExpress()
                {
                    Filter = MSSql_QuerySqlFactory.CreateEqWhere("Id",id,out p),
                    From = from,
                    ResultDataModelType = resultType,
                    Select = select,
                    Top = 1
                };
                if (p != null)
                {
                    sql.QueryParamets = new IDataParamet[] 
                    {
                        p
                    };
                }
                return sql;
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtype));
        }

        /// <summary>
        /// 获取通过指定记录Id获取对象的SQL查询表达式
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="resultType">使用查询语句返回记录来构造对象的对象类型,同时查询语句的from部分使用定义在该类型商的数据库对象名称</param>
        /// <param name="select">查询语句的字段,传null指示使用*</param>
        /// <param name="id">对象Id</param>
        /// <returns></returns>
        public static ISqlExpress CreateQuerySqlByIdValue(string dbtype, Type resultType, IEnumerable<string> select, long id)
        {
            return CreateQuerySqlByIdValue(dbtype, resultType.GetNameForDatabase(), resultType, select, id);
        }

        /// <summary>
        /// 创建指定filter的表达式
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="from">查询语句的表或视图名称</param>
        /// <param name="resultType">使用查询语句返回记录来构造对象的对象类型</param>
        /// <param name="filter">条件表达式语句</param>
        /// <param name="paramets">条件表达式语句的参数数组</param>
        /// <param name="select">查询语句的字段,传null指示使用*</param>
        /// <returns></returns>
        public static ISqlExpress CreateQuerySqlByFilters(string dbtype, string from, Type resultType,string filter,IEnumerable<IDataParamet> paramets, IEnumerable<string> select,int? top)
        {
            if (IsMsSqlDb(dbtype))
            {
                MsQuerySqlExpress sql = new MsQuerySqlExpress()
                {
                    Filter = filter,
                    From = from,
                    ResultDataModelType = resultType,
                    Select = select,
                    QueryParamets = paramets,
                    Top = top
                };
                return sql;
            }
            throw new NotSupportException(ErrorCodes.NotSupportDbType, string.Format(LocalResource.NotSupportDbType, dbtype));
        }

        /// <summary>
        /// 创建指定filter的表达式
        /// 并使用resultType类型中定义的数据库名称作为from名称
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="resultType">使用查询语句返回记录来构造对象的对象类型</param>
        /// <param name="filter">条件表达式语句</param>
        /// <param name="paramets">条件表达式语句的参数数组</param>
        /// <param name="select">查询语句的字段,传null指示使用*</param>
        /// <returns></returns>
        public static ISqlExpress CreateQuerySqlByFilters(string dbtype, Type resultType, string filter, IEnumerable<IDataParamet> paramets, IEnumerable<string> select, int? top)
        {
            return CreateQuerySqlByFilters(dbtype, resultType.GetNameForDatabase(), resultType, filter, paramets, select, top);
        }
    }

    internal static class MSSql_QuerySqlFactory
    {

        /// <summary>
        /// 创建以parametvalue开始或在parametvalue中以列的值开始的条件表达式
        /// </summary>
        /// <param name="fieldname">列字段名称</param>
        /// <param name="parametname">参数名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="parametIsExpress">是否是取在parametvalue中以列的值开始的条件表达式</param>
        /// <param name="paramet"></param>
        /// <returns></returns>
        internal static string CreateFirstIndexOfWhere(
            string fieldname,
            string parametname,
            string parametvalue,
            bool parametIsExpress,
            out IDataParamet paramet)
        {
            paramet = null;
            if (string.IsNullOrEmpty(fieldname) || string.IsNullOrEmpty(parametname))
                return null;
            string left = fieldname;
            if (MsSqlServerHelper.IsValidFieldName(fieldname))
            {
                left = MsSqlServerHelper.GetFeildNameInSqlExpress(fieldname);
            }
            if (parametvalue == null)
            {
                return null;
            }
            string right = MsSqlServerHelper.GetParametName(parametname);
            paramet = new DataParamet() { Direction = System.Data.ParameterDirection.Input, Name = parametname, Value = parametvalue, ValueType = parametvalue.GetType() };
            if (parametIsExpress)
                return string.Format("CHARINDEX({1},{0})=1", left, right);
            else
                return string.Format("CHARINDEX({0},{1})=1", left, right);
        }

        /// <summary>
        /// 创建以parametvalue开始或在parametvalue中以列的值开始的条件表达式
        /// </summary>
        /// <param name="fieldname">列字段名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="parametIsExpress">是否是取在parametvalue中以列的值开始的条件表达式</param>
        /// <param name="paramet"></param>
        /// <returns></returns>
        internal static string CreateFirstIndexOfWhere(
            string fieldname,
            string parametvalue,
            bool parametIsExpress,
            out IDataParamet paramet)
        {
            return CreateFirstIndexOfWhere(fieldname, fieldname, parametvalue, parametIsExpress, out paramet);
        }

        /// <summary>
        /// 创建等于条件表达式
        /// </summary>
        /// <param name="fieldname">左边字段名称</param>
        /// <param name="parametname">右边参数名称</param>
        /// <param name="parametvalue">右边参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        internal static string CreateEqWhere(
            string fieldname,
            string parametname,
            object parametvalue, 
            out IDataParamet paramet)
        {
            paramet = null;
            if (string.IsNullOrEmpty(fieldname) || string.IsNullOrEmpty(parametname))
                return null;
            string left = fieldname;
            if (MsSqlServerHelper.IsValidFieldName(fieldname))
            {
                left = MsSqlServerHelper.GetFeildNameInSqlExpress(fieldname);
            }
            if (parametvalue == null)
            {
                return string.Format("{0} IS NULL", left);
            }
            string right = MsSqlServerHelper.GetParametName(parametname);
            paramet = new DataParamet() { Direction = System.Data.ParameterDirection.Input, Name = parametname, Value = parametvalue, ValueType = parametvalue.GetType() };
            return string.Format("{0}={1}", left, right);
        }

        /// <summary>
        /// 创建等于条件表达式(左边和右边名称一样)
        /// </summary>
        /// <param name="fieldname">字段名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        internal static string CreateEqWhere(
            string fieldname,
            object parametvalue,
            out IDataParamet paramet)
        {
            return CreateEqWhere(fieldname, fieldname, parametvalue, out paramet);
        }


        /// <summary>
        /// 创建不等于条件表达式
        /// </summary>
        /// <param name="fieldname">左边字段名称</param>
        /// <param name="parametname">右边参数名称</param>
        /// <param name="parametvalue">右边参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        internal static string CreateNotEqWhere(
            string fieldname,
            string parametname,
            object parametvalue,
            out IDataParamet paramet)
        {
            paramet = null;
            if (string.IsNullOrEmpty(fieldname) || string.IsNullOrEmpty(parametname))
                return null;
            string left = fieldname;
            if (MsSqlServerHelper.IsValidFieldName(fieldname))
            {
                left = MsSqlServerHelper.GetFeildNameInSqlExpress(fieldname);
            }
            if (parametvalue == null)
            {
                return string.Format("{0} IS NOT NULL", left);
            }
            string right = MsSqlServerHelper.GetParametName(parametname);
            paramet = new DataParamet() { Direction = System.Data.ParameterDirection.Input, Name = parametname, Value = parametvalue, ValueType = parametvalue.GetType() };
            return string.Format("{0}<>{1}", left, right);
        }

        /// <summary>
        /// 创建不等于条件表达式(左边和右边名称一样)
        /// </summary>
        /// <param name="fieldname">字段名称</param>
        /// <param name="parametvalue">参数值</param>
        /// <param name="paramet">返回与条件表达式一起使用的参数表达式</param>
        /// <returns></returns>
        internal static string CreateNotEqWhere(
            string fieldname,
            object parametvalue,
            out IDataParamet paramet)
        {
            return CreateNotEqWhere(fieldname, fieldname, parametvalue, out paramet);
        }

        /// <summary>
        /// 合并条件表达式为并且表达式
        /// </summary>
        /// <param name="wheres">表达式数组</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="rtparamets">合并后的表达式参数数组</param>
        /// <returns>并且表达式</returns>
        internal static string UnionWhereForAnd(
           IEnumerable<string> wheres,
           IEnumerable<IDataParamet> paramets,
           out IEnumerable<IDataParamet> rtparamets)
        {
            rtparamets = null;
            if (paramets != null)
            {
                List<IDataParamet> rt = new List<IDataParamet>();
                foreach (var a in paramets)
                {
                    if (rt.FirstOrDefault(f => string.Equals(MsSqlServerHelper.GetParametName(f.Name), MsSqlServerHelper.GetParametName(a.Name), StringComparison.OrdinalIgnoreCase)) == null)
                        rt.Add(a);
                }
                if (rt.Count > 0)
                    rtparamets = rt.ToArray();
            }
            if (wheres != null)
            {
                StringBuilder xb = new StringBuilder();
                foreach (var s in wheres)
                {
                    if (xb.Length == 0)
                        xb.AppendFormat("({0})", s);
                    else
                        xb.AppendFormat(" AND ({0})", s);
                }
                return xb.ToString();
            }
            return null;
        }

        /// <summary>
        /// 合并条件表达式为或者表达式
        /// </summary>
        /// <param name="wheres">表达式数组</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="rtparamets">合并后的表达式参数数组</param>
        /// <returns>或者表达式</returns>
        internal static string UnionWhereForOr(
           IEnumerable<string> wheres,
           IEnumerable<IDataParamet> paramets,
           out IEnumerable<IDataParamet> rtparamets)
        {
            rtparamets = null;
            if (paramets != null)
            {
                List<IDataParamet> rt = new List<IDataParamet>();
                foreach (var a in paramets)
                {
                    if (rt.FirstOrDefault(f => string.Equals(MsSqlServerHelper.GetParametName(f.Name), MsSqlServerHelper.GetParametName(a.Name), StringComparison.OrdinalIgnoreCase)) == null)
                        rt.Add(a);
                }
                if (rt.Count > 0)
                    rtparamets = rt.ToArray();
            }
            if (wheres != null)
            {
                StringBuilder xb = new StringBuilder();
                foreach (var s in wheres)
                {
                    if (xb.Length == 0)
                        xb.AppendFormat("({0})", s);
                    else
                        xb.AppendFormat(" OR ({0})", s);
                }
                return xb.ToString();
            }
            return null;
        }
    }
}
