using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Encrypt;

namespace YzkSoftWare.Configuration
{
    public static class NormalServiceProvider
    {

        /// <summary>
        /// 创建加解密接口
        /// </summary>
        /// <returns></returns>
        public static ISimpleEncrypt CreateSimpleEncrypt()
        {
            return new DESSimpleEncrypt();
        }

    }
}
