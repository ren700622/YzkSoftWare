using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    public static class ErrorCodes
    {

        /// <summary>
        /// 未知错误
        /// </summary>
        public static int UnkownError = -1;

        /// <summary>
        /// 无效字段名称
        /// </summary>
        public static int InvalidFieldName = 101;
        /// <summary>
        /// 对象成员值设置错误
        /// </summary>
        public static int SetMemberValueError = 102;
        /// <summary>
        /// 不支持的数据库类型错误
        /// </summary>
        public static int NotSupportDbType = 103;
        /// <summary>
        /// 指定的数据库对象不支持更新操作
        /// </summary>
        public static int NotSupportDatabaseObjectForUpdate = 104;


        public static int DatabaseErrorCodeBase = 1000;

        /// <summary>
        /// 字段值为空错误
        /// </summary>
        public static int Input_IsNull_Code = DatabaseErrorCodeBase + 1;
        /// <summary>
        /// 数据超出长度
        /// </summary>
        public static int Input_IsOutRang_Code = DatabaseErrorCodeBase + 2;
        /// <summary>
        /// 重复值错误
        /// </summary>
        public static int Input_IsRepeate_Code = DatabaseErrorCodeBase + 3;
        /// <summary>
        /// 记录不存在错误
        /// </summary>
        public static int Input_NotIsExists_Code = DatabaseErrorCodeBase + 4;

        /// <summary>
        /// 无效父级错误
        /// </summary>
        public static int Input_ValidParent_Code = DatabaseErrorCodeBase + 5;

        /// <summary>
        /// 记录正在被引用错误
        /// </summary>
        public static int Input_IsOnRefenced_Code = DatabaseErrorCodeBase + 6;
    }
}
