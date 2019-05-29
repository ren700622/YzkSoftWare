using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    public static class ErrorCreateHelper
    {

        /// <summary>
        /// 创建字段为空的错误
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static DataModalFieldException CreateFieldIsNullError(string fieldname,string fieldtitle)
        {
            return new DataModalFieldIsNullException(fieldname, string.IsNullOrEmpty(fieldtitle) ? fieldname : fieldtitle);
        }

        /// <summary>
        /// 创建字段值超出范围的错误
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fieldtitle"></param>
        /// <param name="mx">值的最大数据长度</param>
        /// <returns></returns>
        public static DataModalFieldException CreateFieldValueOutOfRangeError(string fieldname, string fieldtitle,int mx)
        {
            return new DataModalFieldIsOutRangleException(fieldname, string.IsNullOrEmpty(fieldtitle) ? fieldname : fieldtitle, mx);
        }

        /// <summary>
        /// 创建字段值超出范围的错误
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fieldtitle"></param>
        /// <param name="rangleexp">值的最大数据长度</param>
        /// <returns></returns>
        public static DataModalFieldException CreateFieldValueOutOfRangeError(string fieldname, string fieldtitle, string rangleexp)
        {
            return new DataModalFieldIsOutRangleException(fieldname, string.IsNullOrEmpty(fieldtitle) ? fieldname : fieldtitle, rangleexp);
        }
    }
}
