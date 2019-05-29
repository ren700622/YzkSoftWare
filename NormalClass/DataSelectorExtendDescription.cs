using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data;
using YzkSoftWare.Data.Sql;

namespace YzkSoftWare
{
    public static class DataSelectorExtendDescription
    {

        /// <summary>
        /// 获取指定Id的数据
        /// </summary>
        /// <param name="view"></param>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <param name="id">要获取数据的Id</param>
        /// <returns></returns>
        public static object GetDataById(this IDataSelector view, Type modelType,IEnumerable<string> selectFieldNames, long id)
        {
            return view.ExecuteReaderOfFirstFor(QuerySqlFactory.CreateQuerySqlByIdValue(view.DbTypeName, modelType, selectFieldNames, id));
        }

        /// <summary>
        /// 获取指定Id的数据
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="view"></param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <param name="id">要获取数据的Id</param>
        /// <returns></returns>
        public static T GetDataById<T>(this IDataSelector view, IEnumerable<string> selectFieldNames, long id) 
            where T : class,new()
        {
            return view.ExecuteReaderOfFirst<T>(QuerySqlFactory.CreateQuerySqlByIdValue(view.DbTypeName, typeof(T), selectFieldNames, id));
        }

        /// <summary>
        /// 获取指定anditems条件数组为并且的查询对象
        /// </summary>
        /// <param name="view"></param>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="anditems">并且表达式</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static object GetDataByAndFilterExpress(this IDataSelector view, Type modelType, IEnumerable<string> anditems, IEnumerable<IDataParamet> paramets, IEnumerable<string> selectFieldNames)
        {
            IEnumerable<IDataParamet> pvs;
            string andfilter = QuerySqlFactory.UnionWhereForAnd(view.DbTypeName, anditems, paramets, out pvs);
            return view.ExecuteReaderOfFirstFor(QuerySqlFactory.CreateQuerySqlByFilters(view.DbTypeName, modelType, andfilter, pvs, selectFieldNames, 1));
        }

        /// <summary>
        /// 获取指定anditems条件数组为并且的查询对象
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="view"></param>
        /// <param name="anditems">并且表达式</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static T GetDataByAndFilterExpress<T>(this IDataSelector view,IEnumerable<string> anditems, IEnumerable<IDataParamet> paramets, IEnumerable<string> selectFieldNames)
            where T :class,new()
        {
            IEnumerable<IDataParamet> pvs;
            string andfilter = QuerySqlFactory.UnionWhereForAnd(view.DbTypeName, anditems, paramets, out pvs);
            return view.ExecuteReaderOfFirst<T>(QuerySqlFactory.CreateQuerySqlByFilters(view.DbTypeName,typeof(T), andfilter, pvs, selectFieldNames, 1));
        }

        /// <summary>
        /// 获取指定字段值相等的数据
        /// </summary>
        /// <param name="view"></param>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="fieldname">字段名称数组</param>
        /// <param name="fieldvalue">字段值数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static object GetDataByFieldEqValue(this IDataSelector view, Type modelType, IEnumerable<string> fieldname, IEnumerable<object> fieldvalue, IEnumerable<string> selectFieldNames)
        {
            List<IDataParamet> pvs = new List<IDataParamet>();
            List<string> filters = new List<string>();
            if (fieldname != null && fieldvalue != null)
            {
                int c = fieldname.Count();
                for (int i = 0; i < c; i++)
                {
                    IDataParamet p;
                    string f = QuerySqlFactory.CreateEqWhere(view.DbTypeName, fieldname.ElementAt(i), fieldvalue.ElementAt(i), out p);
                    if (!string.IsNullOrEmpty(f))
                        filters.Add(f);
                    if (p != null)
                        pvs.Add(p);
                }
            }
            return GetDataByAndFilterExpress(view, modelType, filters, pvs, selectFieldNames);
        }

        /// <summary>
        /// 获取指定字段值相等的数据
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="view"></param>
        /// <param name="fieldname">字段名称数组</param>
        /// <param name="fieldvalue">字段值数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static T GetDataByFieldEqValue<T>(this IDataSelector view, IEnumerable<string> fieldname, IEnumerable<object> fieldvalue, IEnumerable<string> selectFieldNames)
            where T : class,new()
        {
            List<IDataParamet> pvs = new List<IDataParamet>();
            List<string> filters = new List<string>();
            if (fieldname != null && fieldvalue != null)
            {
                int c = fieldname.Count();
                for (int i = 0; i < c; i++)
                {
                    IDataParamet p;
                    string f = QuerySqlFactory.CreateEqWhere(view.DbTypeName, fieldname.ElementAt(i), fieldvalue.ElementAt(i), out p);
                    if (!string.IsNullOrEmpty(f))
                        filters.Add(f);
                    if (p != null)
                        pvs.Add(p);
                }
            }
            return GetDataByAndFilterExpress<T>(view, filters, pvs, selectFieldNames);
        }

        /// <summary>
        /// 获取指定anditems条件数组为并且的查询对象数组
        /// </summary>
        /// <param name="view"></param>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="anditems">并且表达式</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static System.Collections.IEnumerable GetDataListByAndFilterExpress(this IDataSelector view, Type modelType, IEnumerable<string> anditems, IEnumerable<IDataParamet> paramets, IEnumerable<string> selectFieldNames)
        {
            IEnumerable<IDataParamet> pvs;
            string andfilter = QuerySqlFactory.UnionWhereForAnd(view.DbTypeName, anditems, paramets, out pvs);
            return view.ExecuteReaderFor(QuerySqlFactory.CreateQuerySqlByFilters(view.DbTypeName, modelType, andfilter, pvs, selectFieldNames, null));
        }

        /// <summary>
        /// 获取指定anditems条件数组为并且的查询对象数组
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="view"></param>
        /// <param name="anditems">并且表达式</param>
        /// <param name="paramets">表达式参数数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDataListByAndFilterExpress<T>(this IDataSelector view, IEnumerable<string> anditems, IEnumerable<IDataParamet> paramets, IEnumerable<string> selectFieldNames)
            where T : class,new()
        {
            IEnumerable<IDataParamet> pvs;
            string andfilter = QuerySqlFactory.UnionWhereForAnd(view.DbTypeName, anditems, paramets, out pvs);
            return view.ExecuteReader<T>(QuerySqlFactory.CreateQuerySqlByFilters(view.DbTypeName, typeof(T), andfilter, pvs, selectFieldNames, null));
        }


        /// <summary>
        /// 获取指定字段值相等的数据数组
        /// </summary>
        /// <param name="view"></param>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="fieldname">字段名称数组</param>
        /// <param name="fieldvalue">字段值数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static System.Collections.IEnumerable GetDataListByFieldEqValue(this IDataSelector view, Type modelType, IEnumerable<string> fieldname, IEnumerable<object> fieldvalue, IEnumerable<string> selectFieldNames)
        {
            List<IDataParamet> pvs = new List<IDataParamet>();
            List<string> filters = new List<string>();
            if (fieldname != null && fieldvalue != null)
            {
                int c = fieldname.Count();
                for (int i = 0; i < c; i++)
                {
                    IDataParamet p;
                    string f = QuerySqlFactory.CreateEqWhere(view.DbTypeName, fieldname.ElementAt(i), fieldvalue.ElementAt(i), out p);
                    if (!string.IsNullOrEmpty(f))
                        filters.Add(f);
                    if (p != null)
                        pvs.Add(p);
                }
            }
            return GetDataListByAndFilterExpress(view, modelType, filters, pvs, selectFieldNames);
        }

        /// <summary>
        /// 获取指定字段值相等的数据
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="view"></param>
        /// <param name="fieldname">字段名称数组</param>
        /// <param name="fieldvalue">字段值数组</param>
        /// <param name="selectFieldNames">SELECT部分的字段名称数组,传null指示使用*选择</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDataListByFieldEqValue<T>(this IDataSelector view, IEnumerable<string> fieldname, IEnumerable<object> fieldvalue, IEnumerable<string> selectFieldNames)
            where T : class,new()
        {
            List<IDataParamet> pvs = new List<IDataParamet>();
            List<string> filters = new List<string>();
            if (fieldname != null && fieldvalue != null)
            {
                int c = fieldname.Count();
                for (int i = 0; i < c; i++)
                {
                    IDataParamet p;
                    string f = QuerySqlFactory.CreateEqWhere(view.DbTypeName, fieldname.ElementAt(i), fieldvalue.ElementAt(i), out p);
                    if (!string.IsNullOrEmpty(f))
                        filters.Add(f);
                    if (p != null)
                        pvs.Add(p);
                }
            }
            return GetDataListByAndFilterExpress<T>(view, filters, pvs, selectFieldNames);
        }
    }
}
