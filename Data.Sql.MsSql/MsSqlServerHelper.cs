using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data.Sql.MsSql
{
    public static class MsSqlServerHelper
    {

        public static readonly string PixParametName = "@";

        public static readonly string ParametFormate = "@{0}";

        /// <summary>
        /// 判断name是否是MS SQL类型数据库的参数名称(以@开头)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsParametName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.StartsWith(PixParametName);
        }

        /// <summary>
        /// 获取name的MS SQL类型数据库类型的参数格式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParametName(string name)
        {
            if (IsParametName(name))
                return name;
            return string.Format(ParametFormate, name);
        }

        /// <summary>
        /// 是否是MS SQL数据库类型的安全字段引用写法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsFeildNameInSqlExpress(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.StartsWith("[") && name.EndsWith("]");
            }
            return false;
        }

        /// <summary>
        /// 获取MS SQL数据库类型的安全字段引用写法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFeildNameInSqlExpress(string name)
        {
            if (IsParametName(name))
                return name;
            return string.Format("[{0}]", name);
        }

        /// <summary>
        /// 获取主键字段值组成的字符,用作后缀符号
        /// </summary>
        /// <param name="item"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetIdKeyStringValues(object item, IDataModel model)
        {
            StringBuilder keyvalues = new StringBuilder();
            foreach (var idk in model.PrimaryKeys)
            {
                var field = model.Fields[idk.Name];
                object v = field.GetModelFieldValue(item);
                if (keyvalues.Length == 0)
                    keyvalues.AppendFormat("{0}", v);
                else
                    keyvalues.AppendFormat("_{0}", v);
            }
            return keyvalues.ToString();
        }

        /// <summary>
        /// 使用fieldname和后缀hz生成参数名称
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="hz"></param>
        /// <returns></returns>
        private static string GenerateParametName(string fieldname, string hz)
        {
            string name;
            if (!string.IsNullOrEmpty(hz))
            {
                name = string.Format("{0}_{1}", fieldname, hz);
                name = name.Replace("-", "0");
            }
            else
                name = fieldname;
            return GetParametName(name);
        }


        /// <summary>
        /// 将字段值从数据库格式转换为本机格式
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static object ConvertFieldValueFromDbValue(IDataFieldModel field, object value)
        {
            if (field != null)
            {
                var c = field.GetDbValueConvertor();
                if (c != null)
                    return c.ParseValueFromDbValue(value, field.GetClrType());
                else
                {
                    if (value != null && value is IDbValueConvertor)
                        return ((IDbValueConvertor)value).ParseValueFromDbValue(value, value.GetType());
                }
            }
            return value;
        }

        /// <summary>
        /// 字段值转换为数据库格式
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static object ConvertFieldValueToDbValue(IDataFieldModel field, object value)
        {
            if (field != null)
            {
                var c = field.GetDbValueConvertor();
                if (c != null)
                    return c.FormateValueToDbValue(value);
                else
                {
                    Type clr = field.GetClrType();
                    if (clr.FindInterface(typeof(IDbValueConvertor)))
                    {
                        IDbValueConvertor xx = clr.CreateObject() as IDbValueConvertor;
                        return xx.FormateValueToDbValue(value);
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// 添加参数到paramets中去
        /// </summary>
        /// <param name="paramets">参数集合,返回的参数将添加到该集合中,如果不为空的话</param>
        /// <param name="ft">参数后缀,可为为null</param>
        /// <param name="field">字段信息</param>
        /// <param name="value">行值</param>
        /// <returns></returns>
        private static DataParamet AppendParamet(List<IDataParamet> paramets, string parhz, IDataFieldModel field, object rowvalue)
        {
            object value = field.GetModelFieldValue(rowvalue);
            value = ConvertFieldValueToDbValue(field, value);
            DataParamet p = new DataParamet()
            {
                Name = GenerateParametName(field.Name, parhz),
                Value = value,
                ValueType = field.GetClrType()
            };
            int s = field.GetFieldSize();
            if (s > 0)
                p.ValueSize = s;
            if (paramets != null)
                paramets.Add(p);
            return p;
        }

        /// <summary>
        /// 获取指定字段名称的参数值
        /// </summary>
        /// <param name="fieldname">字段名称</param>
        /// <param name="hz">参数后缀</param>
        /// <returns>参数值</returns>
        internal static object GetParametValue(string fieldname, string hz, IEnumerable<IDataParamet> p)
        {
            if (p != null)
            {
                string name = GenerateParametName(fieldname, hz);
                IDataParamet xx = p.FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));
                if (xx != null)
                    return xx.Value;
            }
            return null;
        }

        /// <summary>
        /// 生成添加数据的Sql表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newitem"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        internal static string GetAddNewSql<T>(
            T newitem, out AddNewSqlExpressOutputParamet par)
            where T : class
        {
            par = null;
            if (newitem == null)
                throw new ObjectIsNullException(LocalResource.AddNewItem);
            StringBuilder cols = new StringBuilder();
            StringBuilder values = new StringBuilder();
            StringBuilder setIdsql = new StringBuilder();
            IDataModel model = typeof(T).GetDataModel();
            string namehz = GetIdKeyStringValues(newitem, model);
            List<IDataParamet> paramets = new List<IDataParamet>();
            List<string> outputGenerateFieldNames = new List<string>();
            IDataFieldModelCollection fields = model.Fields;
            int c = 0;
            foreach (var f in fields)
            {
                if (f.IsRowVersion()) continue;
                if (f.GetDbGeneratedDefine() != null)
                {
                    if (f.IsPrimaryKey() != null)
                    {
                        DataParamet p = AppendParamet(paramets, namehz, f, newitem);
                        p.Direction = System.Data.ParameterDirection.InputOutput;
                        outputGenerateFieldNames.Add(f.Name);
                        if (setIdsql.Length == 0)
                            setIdsql.AppendFormat("SET {0} = SCOPE_IDENTITY()", p.Name);
                        else
                            setIdsql.AppendFormat("\r\n SET {0} = SCOPE_IDENTITY()", p.Name);
                    }
                    continue;
                }
                DataParamet fieldvalue = AppendParamet(paramets, namehz, f, newitem);
                if (c == 0)
                {
                    cols.Append(string.Format("[{0}]", f.Name));
                    values.Append(fieldvalue.Name);
                }
                else
                {
                    cols.AppendFormat(",[{0}]", f.Name);
                    values.AppendFormat(",{0}", fieldvalue.Name);
                }
                c++;
            }
            par = new AddNewSqlExpressOutputParamet(
                outputGenerateFieldNames.ToArray(),
                paramets.ToArray(),
                namehz);
            if (setIdsql.Length == 0)
                return string.Format("INSERT INTO [{0}] ({1}) \r\n VALUES({2})", model.Name, cols.ToString(), values.ToString());
            else
                return string.Format("INSERT INTO [{0}] ({1}) \r\n VALUES({2}) \r\n {3}", model.Name, cols.ToString(), values.ToString(), setIdsql.ToString());
        }

        /// <summary>
        /// 生成修改数据的Sql表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateItem"></param>
        /// <param name="updaterange"></param>
        /// <param name="updatefields"></param>
        /// <param name="paramet"></param>
        /// <returns></returns>
        internal static string GetUpdateSql<T>(
            T updateItem,
            FieldSelectRange updaterange,
            IEnumerable<string> updatefields, 
            out IEnumerable<IDataParamet> paramet)
            where T : class
        {
            paramet = null;
            if (updateItem == null)
                throw new ObjectIsNullException(LocalResource.UpdateItem);
            StringBuilder wheresql = new StringBuilder();
            StringBuilder setsql = new StringBuilder();
            List<IDataParamet> paramets = new List<IDataParamet>();
            IDataModel model = typeof(T).GetDataModel();
            string namehz = GetIdKeyStringValues(updateItem, model);
            int c = 0;
            foreach (var k in model.PrimaryKeys)
            {
                IDataFieldModel f = model.Fields[k.Name];
                DataParamet p = AppendParamet(paramets, namehz, f, updateItem);
                if (c == 0)
                    wheresql.Append(string.Format("[{0}]={1}", f.Name, p.Name));
                else
                    wheresql.Append(string.Format(" AND [{0}]={1}", f.Name, p.Name));
                c++;
            }
            if (wheresql.Length == 0) //没有任何主键信息
                throw new NotFoundPrimaryKeyException(typeof(T));
            IDataFieldModelCollection fields = model.Fields;
            c = 0;
            foreach (var f in fields)
            {
                if (f.IsRowVersion()) continue;
                if (f.GetDbGeneratedDefine() != null) continue;
                if (f.IsPrimaryKey() != null) continue;
                switch (updaterange)
                {
                    case FieldSelectRange.All: break;
                    case FieldSelectRange.OnlyFields:
                        {
                            if (updatefields == null)
                                continue;
                            if (updatefields.FirstOrDefault(
                                 ff => string.Equals(ff, f.Name, StringComparison.OrdinalIgnoreCase)) == null)
                                continue;
                            break;
                        }
                    case FieldSelectRange.IgnoreFields:
                        {
                            if (updatefields != null)
                            {
                                if (updatefields.FirstOrDefault(
                                 ff => string.Equals(ff, f.Name, StringComparison.OrdinalIgnoreCase)) != null)
                                    continue;
                            }
                            break;
                        }
                }
                DataParamet p = AppendParamet(paramets, namehz, f, updateItem);
                if (c == 0)
                    setsql.AppendLine(string.Format("[{0}]={1}", f.Name, p.Name));
                else
                    setsql.AppendLine(string.Format(",[{0}]={1}", f.Name, p.Name));
                c++;
            }
            if (setsql.Length == 0)
                return null;

            paramet = paramets.ToArray();
            return string.Format("UPDATE [{0}] SET {1} WHERE {2}", model.Name, setsql.ToString(), wheresql.ToString());
        }

        /// <summary>
        /// 生成删除数据的Sql表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deleteItem"></param>
        /// <param name="paramet"></param>
        /// <returns></returns>
        internal static string GetDeleteSql<T>(
            T deleteItem,
            out IEnumerable<IDataParamet> paramet)
        {
            paramet = null;
            if (deleteItem == null)
                throw new ObjectIsNullException(LocalResource.DeleteItem);

            StringBuilder wheresql = new StringBuilder();
            List<IDataParamet> paramets = new List<IDataParamet>();
            IDataModel model = typeof(T).GetDataModel();
            string namehz = GetIdKeyStringValues(deleteItem, model);
            int c = 0;
            foreach (var k in model.PrimaryKeys)
            {
                IDataFieldModel f = model.Fields[k.Name];
                DataParamet p = AppendParamet(paramets, namehz, f, deleteItem);
                if (c == 0)
                    wheresql.Append(string.Format("[{0}]={1}", f.Name, p.Name));
                else
                    wheresql.Append(string.Format(" AND [{0}]={1}", f.Name, p.Name));
                c++;
            }
            if (wheresql.Length == 0) //没有任何主键信息
                throw new NotFoundPrimaryKeyException(typeof(T));
            paramet = paramets.ToArray();
            string sdf = model.GetSoftDeleteFieldName();
            if (!string.IsNullOrEmpty(sdf))
                return string.Format("UPDATE {0} SET {1}=1 WHERE {2}",
                    MsSqlServerHelper.GetFeildNameInSqlExpress(model.Name),
                    MsSqlServerHelper.GetFeildNameInSqlExpress(sdf),
                    wheresql.ToString());
            else
                return string.Format("DELETE {0} WHERE {1}", GetFeildNameInSqlExpress(model.Name), wheresql.ToString());
        }


        /// <summary>
        /// 获取指定值的数据库表示形式
        /// </summary>
        /// <param name="valueType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string GetDbTypeValue(Type valueType, object value)
        {
            if (value == null)
                return null;
            var dbType = valueType.GetDbType();
            switch (dbType)
            {
                case System.Data.DbType.AnsiString:
                case System.Data.DbType.AnsiStringFixedLength:
                case System.Data.DbType.String:
                case System.Data.DbType.StringFixedLength:
                    return string.Format("N'{0}'", value);
                case System.Data.DbType.Date:
                case System.Data.DbType.DateTime:
                    {
                        return string.Format("'{0:yyyy-MM-dd HH:mm:ss.fffffff}'", value);
                    }
                case System.Data.DbType.Boolean:
                    {
                        if ((bool)value)
                            return "1";
                        else
                            return "0";
                    }
                default: return value.ToString();
            }
        }

        /// <summary>
        /// 判定指定的字符是否合法的字段名称
        /// 正确的是:以 _或英文字母开头,仅包含_、英文字母、数字
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsValidFieldName(string n)
        {
            if (string.IsNullOrWhiteSpace(n))
                return false;
            if (string.IsNullOrEmpty(n))
                return false;
            char[] chs = n.ToArray();
            for (int i = 0; i < chs.Length; i++)
            {
                char ch = chs[i];
                if (ch == '_') continue;
                if (i == 0)
                {
                    if (!char.IsLetter(ch)) return false;
                    continue;
                }
                if (char.IsLetter(ch) || char.IsDigit(ch)) continue;
                return false;
            }
            return true;
        }
    }

    internal class AddNewSqlExpressOutputParamet
    {

        internal AddNewSqlExpressOutputParamet(
            IEnumerable<string> outputGenerateFieldNames,
            IEnumerable<IDataParamet> paramets,
            string nameHz)
        {
            OutputGenerateFieldNames = outputGenerateFieldNames;
            Paramets = paramets;
            NameHz = nameHz;
        }

        /// <summary>
        /// 数据库自动设置字段集合
        /// </summary>
        public IEnumerable<string> OutputGenerateFieldNames { get; private set; }

        /// <summary>
        /// 表达式参数
        /// </summary>
        public IEnumerable<IDataParamet> Paramets { get; private set; }

        /// <summary>
        /// 参数名称后缀
        /// </summary>
        public string NameHz { get; private set; }
    }
}
