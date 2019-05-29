using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 数据字段错误
    /// </summary>
    public class DataModalFieldException : CodeExceptionBase
    {

        public DataModalFieldException(int errcode, string fieldname, string errmsg)
            : base(errcode, errmsg)
        {
            p_FieldName = fieldname;
        }

        private string p_FieldName;

        public string FieldName
        {
            get { return p_FieldName; }
        }
    }

    /// <summary>
    /// 数据错误
    /// </summary>
    public class DataModalFieldCollectionException : Exception
    {

        public DataModalFieldCollectionException(IEnumerable<DataModalFieldException> errors)
        {
            p_Errors = errors;
        }

        private IEnumerable<DataModalFieldException> p_Errors;

        public IEnumerable<DataModalFieldException> Errors
        {
            get { return p_Errors; }
        }

        private string GetErrorMessage()
        {
            if (p_Errors != null)
            {
                StringBuilder xb = new StringBuilder();
                foreach (var e in p_Errors)
                {
                    if (xb.Length > 0)
                        xb.AppendLine();
                    xb.AppendFormat("{0}", e.Message);
                }
                if (xb.Length > 0)
                    return xb.ToString();
            }
            return null;
        }

        public override string Message
        {
            get
            {
                return GetErrorMessage();
            }
        }
    }

    /// <summary>
    /// 字段值为null错误
    /// </summary>
    public class DataModalFieldIsNullException : DataModalFieldException
    {

        public DataModalFieldIsNullException(string fieldname, string displayname)
            : base(ErrorCodes.Input_IsNull_Code, fieldname, string.Format(LocalResource.DataModalFieldIsNullException, displayname))
        {

        }

    }

    /// <summary>
    /// 字段值超出范围错误
    /// </summary>
    public class DataModalFieldIsOutRangleException : DataModalFieldException
    {

        public DataModalFieldIsOutRangleException(string fieldname, string displayname, int maxrangle)
            : base(ErrorCodes.Input_IsOutRang_Code, fieldname, string.Format(LocalResource.DataModalFieldIsOutRangleException, displayname, maxrangle))
        {

        }

        public DataModalFieldIsOutRangleException(string fieldname, string displayname, string rangle)
            : base(ErrorCodes.Input_IsOutRang_Code, fieldname, string.Format(LocalResource.DataModalFieldIsOutRangleException, displayname, rangle))
        {

        }
    }

    /// <summary>
    /// 记录中字段值重复错误
    /// </summary>
    public class DataModalFieldIsRepeateException : DataModalFieldException
    {

        public DataModalFieldIsRepeateException(string fieldname, string displayname)
            : base(ErrorCodes.Input_IsRepeate_Code, fieldname, string.Format(LocalResource.DataModalFieldIsRepeateException, displayname))
        {

        }

    }

    /// <summary>
    /// 记录不存在错误
    /// </summary>
    public class DataModalFieldNotIsExistsException : DataModalFieldException
    {

        public DataModalFieldNotIsExistsException(string fieldname, string tablename, string notexistsexp)
            : base(ErrorCodes.Input_NotIsExists_Code, fieldname, string.Format(LocalResource.DataModalFieldNotIsExistsException, tablename, notexistsexp))
        {

        }

    }

    /// <summary>
    /// 无效父级错误
    /// </summary>
    public class DataModalFieldValidParentException : DataModalFieldException
    {

        public DataModalFieldValidParentException(string fieldname)
            : base(ErrorCodes.Input_ValidParent_Code, fieldname, LocalResource.DataModalFieldValidParentException)
        {

        }

    }

    /// <summary>
    /// 记录正在被引用错误
    /// </summary>
    public class DataModalFieldIsOnRefencedException : DataModalFieldException
    {

        public DataModalFieldIsOnRefencedException(string fieldname,string note)
            : base(ErrorCodes.Input_IsOnRefenced_Code, fieldname, note)
        {

        }

    }
}
